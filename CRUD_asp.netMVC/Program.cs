using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.Data.Seed;
using CRUD_asp.netMVC.DTO.Payments;
using CRUD_asp.netMVC.EventHandlers;
using CRUD_asp.netMVC.EventHandlers.GHN;
using CRUD_asp.netMVC.EventHandlers.Payments;
using CRUD_asp.netMVC.Extensions.Payments;
using CRUD_asp.netMVC.Filters;
using CRUD_asp.netMVC.Hubs;
using CRUD_asp.netMVC.Middleware;
using CRUD_asp.netMVC.Models.Auth;
using CRUD_asp.netMVC.Service.EmailSender;
using CRUD_asp.netMVC.Service.GHN;
using CRUD_asp.netMVC.Service.GHN.Api;
using CRUD_asp.netMVC.Service.GHN.Common;
using CRUD_asp.netMVC.Service.GHN.Fil;
using CRUD_asp.netMVC.Service.Home;
using CRUD_asp.netMVC.Service.HttpClientPolicies;
using CRUD_asp.netMVC.Service.Payments;
using CRUD_asp.netMVC.Service.Scopes;
using CRUD_asp.netMVC.Service.Users;
using Hangfire;
using Hangfire.Redis.StackExchange;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace CRUD_asp.netMVC
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // them ValidateAntiforgeryToken cho toan project
            builder.Services.AddControllersWithViews(ops => ops.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));
            builder.Services.AddRazorPages();
            //builder.Services.AddSignalR(); // Cau hinh SignalR real time

            //Dinh dang DB SQLServer
            builder.Services.AddDbContext<AppDBContext>(options =>
                options.UseSqlServer(LoadConnectString(builder, "AppDBContext"))
            );

            // lay chuoi connect Neon theo dinh dang DB PostgreSQL
            //builder.Services.AddDbContext<AppDBContext>(options =>
            //    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
            //);

            // ========= REDIS CLOUD 30MB + HANGFIRE (KHÔNG MẤT DỮ LIỆU) =============
            var redisConnectionString = LoadConnectString(builder, "Redis");

            var options = ConfigurationOptions.Parse(redisConnectionString);

            options.SyncTimeout = 10000;
            options.ConnectTimeout = 10000;

            options.SslProtocols = System.Security.Authentication.SslProtocols.Tls12 | System.Security.Authentication.SslProtocols.Tls13;

            var multiplexer = await ConnectionMultiplexer.ConnectAsync(options);

            // Đăng ký Redis singleton để cả middleware + job dùng chung
            builder.Services.AddSingleton<IConnectionMultiplexer>(sp => multiplexer);

            // SignalR + Redis Backplane (bắt buộc khi deploy nhiều server)
            builder.Services.AddSignalR().AddStackExchangeRedis(redisConnectionString);

            // Dang ky RedisVisitListenerService
            builder.Services.AddHostedService<RedisVisitListenerService>();

            // Dùng Redis làm storage cho Hangfire → không mất job khi tắt máy
            builder.Services.AddHangfire(config => config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseRedisStorage(redisConnectionString, new RedisStorageOptions
                {
                    Prefix = "hangfire:sitevisit" // tránh xung đột key
                }));

            // Redis Hangfire server (queue riêng)
            builder.Services.AddHangfireServer(options =>
            {
                options.ServerName = "RedisServer-SiteVisit";
                options.Queues = new[] { "sitevisit" };
            });

            // Hangfire cho SQL Server
            builder.Services.AddHangfire(config =>
            {
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                      .UseSimpleAssemblyNameTypeSerializer()
                      .UseRecommendedSerializerSettings()
                      .UseSqlServerStorage(
                            LoadConnectString(builder, "AppDBContext"), new SqlServerStorageOptions
                            {
                                PrepareSchemaIfNecessary = true,
                                QueuePollInterval = TimeSpan.FromSeconds(15),
                                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5)
                            });
            });

            // SQL Hangfire server (queue riêng)
            builder.Services.AddHangfireServer(options =>
            {
                options.ServerName = "SqlServer-OrderCleanup";
                options.Queues = new[] { "ordercleanup" };
            });

            // Dang ky OrderCleanup backgroud service 
            builder.Services.AddScoped<OrderCleanupService>();

            // Dang ky service create lìfecycle scoped AppDbContext
            builder.Services.AddScoped<IScopedExecutor, ScopedExecutor>();

            // Dang ky service dem va cap nhat so luong nguoi truy cap
            builder.Services.AddScoped<ISiteUserVisitService, SiteUserVisitService>();

            // Dang ky service Seed data GHN
            builder.Services.AddScoped<IGenenricDataGHN, GenericDataGHN>();

            // Dang ky service call API GHN
            builder.Services.AddScoped<ICallAPI, CallAPI>();

            // Dang ky service filter GHN
            builder.Services.AddScoped<IFilterData, FilterData>();

            // Dang ky service QrCode
            builder.Services.AddScoped<QrCodeService>();
            builder.Services.AddScoped<ISmsPaymentVerificationService, SmsPaymentVerificationService>();

            // Dnag ky service order tracking user
            builder.Services.AddScoped<IDisplayOrderTrackingService, DisplayOrderTrackingService>();

            // Check setting baseurl, token ghn
            var baseUrl = builder.Configuration["GHN:BaseURL"];
            var tokenGHN = builder.Configuration["GHN:Token"];
            if (string.IsNullOrEmpty(baseUrl) || string.IsNullOrEmpty(tokenGHN))
            {
                throw new InvalidOperationException("baseURL or Token of GHN is not configured");
            }

            // Dang ky tao don GHN
            //builder.Services.AddHttpClient<IGhnService, GhnService>(client =>
            //{
            //    client.BaseAddress = new Uri(baseUrl);
            //    client.DefaultRequestHeaders.Add("Token", tokenGHN);
            //    client.Timeout = TimeSpan.FromSeconds(10);
            //    client.DefaultRequestHeaders.Accept.Add(
            //        new MediaTypeWithQualityHeaderValue("application/json")
            //    );

            //}).AddPolicyHandler(GhnRetryPolicy.GetRetryPolicy());

            // Dnag ky service GHN
            builder.Services.AddScoped<IGhnService, GhnService>();

            builder.Services.AddHttpClient("GHN", client =>
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Add("Token", tokenGHN);
                client.Timeout = TimeSpan.FromSeconds(10);

            }).AddPolicyHandler(GhnRetryPolicy.GetRetryPolicy());

            builder.Services.AddScoped<DbInitializer>();

            builder.Services.AddScoped<IEventBus, InMemoryEventBus>();
            builder.Services.AddScoped<IEventHandler<OrderPaidEvent>, NotifyPaymentHandler>();
            builder.Services.AddScoped<IEventHandler<OrderPaidEvent>, OrderPaidDashboardHandler>();
            builder.Services.AddScoped<IEventHandler<CallAPIRequestGHNEvent>, CallAPIRequestGHN>();
            builder.Services.AddScoped<IEventHandler<PaymentVerificationEvent>, PaymentVerificationHandler>();

            // Dang ky service DisplayProfileUserService
            builder.Services.AddScoped<IDisplayProfileUserService, DisplayProfileUserService>();


            // Dang ky Session
            builder.Services.AddSession(options =>
            {
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Chỉ gui qua HTTPS
                options.Cookie.SameSite = SameSiteMode.Lax; // Ngăn CSRF
                options.IdleTimeout = TimeSpan.FromMinutes(120); // Thoi gian session ton tai
                options.Cookie.HttpOnly = true; // Chi cho server truy cap
                options.Cookie.IsEssential = true; // Bat buoc phai co session (neu false van lay cookie can thiet nhu luu tru sessionID de duy tri dang nhap)
            });

            // Dang ky identity
            builder.Services.AddIdentity<Users, Roles>(options =>
            {
                // Cau hinh mat khau
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;

                // Cau hinh nguoi dung
                options.User.RequireUniqueEmail = true;

            }).AddEntityFrameworkStores<AppDBContext>().AddDefaultTokenProviders(); // Để dùng chức năng reset password, confirm email, v.v.

            // Dang ky Cookie
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Auth/Login";
                options.LogoutPath = "/Auth/Logout";
                options.AccessDeniedPath = "/Auth/AccessDenied";
            });

            // Dang ky gmail
            builder.Services.AddTransient<IEmailSender, EmailSender>();

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = LoadConnectString(builder, "Redis");
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("CustomerOrGuest", policy =>
                {
                    policy.RequireAssertion(context =>
                    {
                        // Cho phép nếu:
                        // 1. Chưa đăng nhập (Guest)   HOẶC
                        // 2. Đã đăng nhập và có role "Customer"
                        return !context.User.Identity?.IsAuthenticated == true ||
                               context.User.IsInRole("Customer");
                    });
                });
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var seeder = services.GetRequiredService<DbInitializer>();

                await seeder.SeedAddressesAsync();
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseHsts();
                app.UseHttpsRedirection();
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            /// 1. Ở môi trường Production thì bắt buộc dùng HTTPS
            app.UseHttpsRedirection();
            // → Tự động redirect http → https (bắt buộc để cookie Secure hoạt động)

            // 2. Phục vụ file tĩnh: images, css, js, favicon...
            app.UseStaticFiles();
            // → Truy cập /images/banner.jpg → không đi qua controller

            // 3. Định tuyến request đến đúng endpoint (Controller hoặc Minimal API)
            app.UseRouting();
            // → Quyết định request này sẽ đi vào Controller nào

            app.UseSession();

            // 4. Xác thực người dùng (đọc JWT, cookie authentication, v.v.)
            app.UseAuthentication();
            // → Sau dòng này thì context.User mới có thông tin (đã login hay chưa)

            // 5. Phân quyền (Admin chỉ được vào /admin, User thường không được, v.v.)
            app.UseAuthorization();

            app.UseMiddleware<VisitCountUserMiddleware>();

            app.MapControllers();

            // Phải đứng sau UseAuthentication
            app.MapRazorPages();

            // Dung cho cap nhat va dem so luong nguoi truy cap

            // Cau hinh endpoint hub
            app.MapHub<PaymentHub>("/paymentHub");

            app.MapHub<LoadViewHub>("/lazyLoad");

            app.MapHub<LoadViewHub>("/changeEmailProfile");

            app.MapHub<DashboardHub>("/DashboardHub");

            app.MapHub<SurplusMoneyHub>("/SurplusMoney");

            app.MapHub<QuestionResHub>("/questionRes");

            app.MapHub<RequestGHNHub>("/requestGHN");

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthFilter() },
                DashboardTitle = "Hangfire – Admin"
            });

            // Job chạy 07:05 sáng giờ Việt Nam mỗi ngày → ghi UV/DAU vào SQL + dọn Redis
            RecurringJob.AddOrUpdate<SiteUserVisitService>(
                "store-daily-visits",
                job => job.IncreaseSiteVisitAsync(),
                "5 7 * * *", // 7h05 chay recurringJob
                new RecurringJobOptions
                {
                    TimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"),
                    QueueName = "sitevisit"
                });

            // Xoa pending order cart
            RecurringJob.AddOrUpdate<OrderCleanupService>(
                "order-cleanup",
                job => job.CleanExpiredOrderAsync(),
                "*/5 * * * *",
                new RecurringJobOptions
                {
                    TimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"),
                    QueueName = "ordercleanup"
                });

            // Trang chủ trỏ về Auth/Login
            app.MapControllerRoute(
                name: "root",
                pattern: "",
                defaults: new { controller = "Auth", action = "Login" });

            // Route mặc định chuẩn để tất cả controller hoạt động
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }

        // Tao chuoi lay ket noi DB
        public static string LoadConnectString(WebApplicationBuilder bulder, string key)
        {
            return bulder.Configuration.GetConnectionString(key) ?? string.Empty;
        }
    }
}
