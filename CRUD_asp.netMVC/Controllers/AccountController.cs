using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.Models.Account;
using CRUD_asp.netMVC.Models.Account.ActionViewModel;
using EFCoreSecondLevelCacheInterceptor;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using Microsoft.EntityFrameworkCore.Storage.Json;
using NuGet.Protocol.Plugins;
using System.Collections.Immutable;
using System.Diagnostics.Eventing.Reader;

namespace CRUD_asp.netMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;
        private readonly RoleManager<Roles> _roleManager;
        private readonly AppDBContext _context;

        public AccountController(UserManager<Users> userManager, SignInManager<Users> signInManager, AppDBContext appDBContext, RoleManager<Roles> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = appDBContext;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register() => View();


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Register register)
        {
            if (!ModelState.IsValid) return View(register);

            var user = new Users
            {
                UserName = register.UserName,
                Email = register.Email,
                PhoneNumber = register.Phone,
                StartDate = register.StartDate,
                PhoneNumberConfirmed = false, // Xac thuc sdt
                EmailConfirmed = true, // Xac thuc email
                SecurityStamp = Guid.NewGuid().ToString("D"),
            };


            string role = "Customer";

            if (register.Email.Contains("nhanvien", StringComparison.OrdinalIgnoreCase))
            {
                role = "Staff";
            }
            else if (register.Email.Contains("nguyenthanhtuankrp1", StringComparison.OrdinalIgnoreCase))
            {
                role = "Manager";
            }

            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new Roles { Name = role });
            }

            //var UserID = await _userManager.FindByNameAsync(role);
            var UserID = await _context.Roles.FirstOrDefaultAsync(p => p.Name == role);
            user.RoleID = UserID?.Id;

            var account = await _userManager.CreateAsync(user, register.Password);

            await _userManager.AddToRoleAsync(user, role);

            if (account.Succeeded)
            {
                switch (role)
                {
                    case "Manager":
                        _context.Manager.Add(new Manager
                        {
                            UserID = user.Id,
                            UserName = user.UserName,
                            Email = user.Email,
                            StartDate = user.StartDate,

                            FirstName = register.FirstName,
                            LastName = register.LastName,
                            PhoneNumber = register.Phone

                        });
                        break;

                    case "Staff":
                        _context.Staff.Add(new Staff
                        {
                            UserID = user.Id,
                            UserName = user.UserName,
                            Email = user.Email,
                            StartDate = user.StartDate,

                            FirstName = register.FirstName,
                            LastName = register.LastName,
                            PhoneNumber = register.Phone,

                        });
                        break;

                    case "Customer":
                        _context.Customer.Add(new Customer
                        {
                            UserID = user.Id,
                            UserName = user.UserName,
                            Email = user.Email,
                            JoinDate = user.StartDate,

                            FirstName = register.FirstName,
                            LastName = register.LastName,
                            PhoneNumber = register.Phone
                        });
                        break;

                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Account", "Login");
            }

            foreach (var error in account.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(register);
        }

        public IActionResult Login() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login login)
        {
            if (!ModelState.IsValid) return View(login);

            var user = await _userManager.FindByEmailAsync(login.Email.Trim());
            if (user == null || !await _userManager.CheckPasswordAsync(user, login.Password))
            {
                ModelState.AddModelError(string.Empty, "Email or Password is incorrect");
                return View(login);
            }

            var account = await _signInManager.PasswordSignInAsync(user, login.Password, login.RememberMe, lockoutOnFailure: false);
            if (account.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Error occurred while logging in");
            return View(login);
        }
    }
}
