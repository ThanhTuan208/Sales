﻿using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.Models.Auth;
using CRUD_asp.netMVC.Models.Service;
using CRUD_asp.netMVC.Models.ViewModels.Product;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;

namespace CRUD_asp.netMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            // Dky AppDBContext 
            builder.Services.AddDbContext<AppDBContext>(option =>
                option.UseSqlServer(builder.Configuration.GetConnectionString("AppDBContext")) // appsettings.json
            );

            // Them dich vu Session
            builder.Services.AddSession(options =>
            {
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Chỉ gửi qua HTTPS
                options.Cookie.SameSite = SameSiteMode.Lax; // Ngăn CSRF
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Thoi gian session ton tai
                options.Cookie.HttpOnly = true; // Chi cho server truy cap
                options.Cookie.IsEssential = true; // Bat buoc phai co session (neu false van lay cookie can thiet nhu luu tru sessionID de duy tri dang nhap)
            });

            // Dang ky identity
            builder.Services.AddIdentity<Users, Roles>(options =>
            {
                // Cấu hình mật khẩu
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;

                // Cấu hình người dùng
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<AppDBContext>().AddDefaultTokenProviders(); // Để dùng chức năng reset password, confirm email, v.v.

            // Dang ky Cookie
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Auth/Login";
                options.LogoutPath = "/Auth/Logout";
                options.AccessDeniedPath = "/Auth/AccessDenied";
            });

            // Dang ky dich vu gmail
            builder.Services.AddTransient<IEmailSender, EmailSender>();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseSession();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.MapRazorPages();

            app.UseAuthorization();
            app.UseAuthentication();

            app.MapControllerRoute(
                name: "default",
                /*pattern: "{controller=Home}/{action=Index}/{id?}");*/
                pattern: "{controller=Auth}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
