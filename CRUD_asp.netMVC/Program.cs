using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.Filters;
using CRUD_asp.netMVC.HubRealTime;
using CRUD_asp.netMVC.Middleware;
using CRUD_asp.netMVC.Models.Auth;
using CRUD_asp.netMVC.Service.EmailSender;
using CRUD_asp.netMVC.Service.GHN;
using CRUD_asp.netMVC.Service.Payment;
using CRUD_asp.netMVC.Service.Payment.SiteVisitService;
using Hangfire;
using Hangfire.Redis.StackExchange;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

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
            builder.Services.AddSignalR(); // Cau hinh SignalR real time

            //Dinh dang DB SQLServer
            builder.Services.AddDbContext<AppDBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("AppDBContext"))
            );

            //// Them HangFire => Queue
            //builder.Services.AddHangfire(config =>
            //    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("AppDBContext")));

            //builder.Services.AddHangfireServer();

            // lay chuoi connect Neon theo dinh dang DB PostgreSQL
            //builder.Services.AddDbContext<AppDBContext>(options =>
            //    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
            //);

            // ==================== REDIS CLOUD 30MB + HANGFIRE (KHÔNG MẤT DỮ LIỆU) ====================
            var redisConnectionString = builder.Configuration.GetConnectionString("Redis");

            var options = ConfigurationOptions.Parse(redisConnectionString);
            options.User = "default"; // BẮT BUỘC
            options.Password = "c6zIyl2kiVTRSZbkW5eUGwNsHeRxs2Sc"; // BẮT BUỘC
            options.AbortOnConnectFail = false;
            options.ConnectTimeout = 10000;
            options.SyncTimeout = 10000;
            options.Ssl = false;
            options.CertificateValidation += (sender, cert, chain, errors) => true;

            var multiplexer = await ConnectionMultiplexer.ConnectAsync(options);

            // Đăng ký Redis singleton để cả middleware + job dùng chung
            builder.Services.AddSingleton<IConnectionMultiplexer>(sp => multiplexer);

            // Dùng Redis làm storage cho Hangfire → không mất job khi tắt máy
            builder.Services.AddHangfire(config => config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseRedisStorage(redisConnectionString, new RedisStorageOptions
                {
                    Prefix = "hangfire:" // tránh xung đột key
                }));

            builder.Services.AddHangfireServer();

            // Dang ky backgroud service 
            builder.Services.AddHostedService<OrderCleanupService>();

            // Dang ky service dem va cap nhat so luong nguoi truy cap
            builder.Services.AddScoped<ISiteUserVisitService, SiteUserVisitService>();

            // Dang ky service QrCode
            builder.Services.AddScoped<QrCodeService>();
            builder.Services.AddScoped<ISmsPaymentVerificationService, SmsPaymentVerificationService>();

            // Dang ky tao don GHN
            builder.Services.AddScoped<GhnService>();
            builder.Services.AddHttpClient<IGhnService, GhnService>();

            // Dabg ky Session
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
                options.Configuration = builder.Configuration.GetConnectionString("Redis");
            });

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseSession();

            // 1. Ở môi trường Production thì bắt buộc dùng HTTPS
            app.UseHttpsRedirection();
            // → Tự động redirect http → https (bắt buộc để cookie Secure hoạt động)

            // 2. Phục vụ file tĩnh: images, css, js, favicon...
            app.UseStaticFiles();
            // → Truy cập /images/banner.jpg → không đi qua controller

            // 3. Định tuyến request đến đúng endpoint (Controller hoặc Minimal API)
            app.UseRouting();
            // → Quyết định request này sẽ đi vào Controller nào

            // 4. Xác thực người dùng (đọc JWT, cookie authentication, v.v.)
            app.UseAuthentication();
            // → Sau dòng này thì context.User mới có thông tin (đã login hay chưa)

            // 5. Phân quyền (Admin chỉ được vào /admin, User thường không được, v.v.)
            app.UseAuthorization();
            // → Phải đứng sau UseAuthentication

            app.MapRazorPages();

            // Dung cho cap nhat va dem so luong nguoi truy cap
            app.UseMiddleware<VisitCountUserMiddleware>();

            // Cau hinh endpoint hub
            app.MapHub<PaymentHub>("/paymentHub");
            app.MapHub<LoadViewHub>("/changeEmailProfile");
            app.MapHub<LoadViewHub>("/lazyLoad");

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthFilter() },
                DashboardTitle = "Hangfire – Admin"
            });

            // Job chạy 07:05 sáng giờ Việt Nam mỗi ngày → ghi UV/DAU vào SQL + dọn Redis
            RecurringJob.AddOrUpdate<SiteUserVisitService>(
                "store-daily-visits",
                job => job.IncreaseSiteVisit(),
                "5 7 * * *",
                new RecurringJobOptions
                {
                    TimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"),
                });

            app.MapControllerRoute(
                name: "default",
                /*pattern: "{controller=Home}/{action=Index}/{id?}");*/
                pattern: "{controller=Auth}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
