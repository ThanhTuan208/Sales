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
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

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


        public string RemoveDiacritics(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            var stringNormal = text.Normalize(NormalizationForm.FormD);
            StringBuilder builder = new StringBuilder();

            foreach (char c in stringNormal)
            {
                var unicodeCate = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCate != UnicodeCategory.NonSpacingMark)
                {
                    builder.Append(c);
                }
            }

            return builder.ToString().Normalize(NormalizationForm.FormC).Replace(" ", "");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Register register)
        {
            try
            {
                if (!ModelState.IsValid) return View(register);

                var existEmail = await _userManager.FindByEmailAsync(register.Email.Trim());

                if (existEmail != null)
                {
                    ModelState.AddModelError("Email", "Email đã tồn tại");
                    return View(register);
                }

                var user = new Users
                {
                    UserName = RemoveDiacritics(register.UserName),
                    Email = register.Email,
                    PhoneNumber = register.Phone,
                    StartDate = register.StartDate,
                    PhoneNumberConfirmed = true, // Xac thuc sdt
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
                var RoleID = await _context.Roles.FirstOrDefaultAsync(p => p.Name == role);
                user.RoleID = RoleID.Id;

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
                    return RedirectToAction("Login", "Account");
                }

                foreach (var error in account.Errors)
                {
                    Console.WriteLine(error.Code, error.Description);
                    ModelState.AddModelError(error.Code, error.Description);
                }

                return View(register);
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Lỗi đăng kí tài khoản");
                return View(register);
            }

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

        [HttpGet]
        public IActionResult LoginByProductID(int productID)
        {
            ViewData["id"] = productID;
            return View(nameof(Login));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginByProductID(Login login, int productID)
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
                return RedirectToAction("ProductDetail", "Product", new { id = productID });
            }

            ModelState.AddModelError(string.Empty, "Error occurred while logging in");
            return View("Login", login);
        }

        // Dang xuat va dieu huong toi Login
        // Nen dung post thay get vi tin tac co the truy van qua link de pha trai nghiem client
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            var jsonCart = HttpContext.Session.GetString("Cart");

            await _signInManager.SignOutAsync();

            if (!string.IsNullOrWhiteSpace(jsonCart))
            {
                HttpContext.Session.SetString("Cart", jsonCart);
            }

            return RedirectToAction("Login", "Account");
        }

    }
}
