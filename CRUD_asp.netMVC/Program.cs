using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.Models.Account;
using CRUD_asp.netMVC.Models.ViewModels.Product;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
namespace CRUD_asp.netMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddSingleton<ProductCreateViewModel>();
            builder.Services.AddSingleton<ProductEditViewModel>();

            builder.Services.AddRazorPages();

            // Dky context 
            builder.Services.AddDbContext<AppDBContext>(option =>
                option.UseSqlServer(builder.Configuration.GetConnectionString("AppDBContext")) // appsettings.json
            );

            builder.Services.AddIdentity<Users, Roles>(options =>
            {
                // Cấu hình mật khẩu
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;

                // Cấu hình người dùng
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<AppDBContext>().AddDefaultTokenProviders(); // Để dùng chức năng reset password, confirm email, v.v.


            // Cấu hình Cookie
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.MapRazorPages();

            app.UseAuthorization();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
