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
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.Win32;
using NuGet.Protocol.Plugins;
using System.Collections.Immutable;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Register = CRUD_asp.netMVC.Models.Account.ActionViewModel.Register;
using Login = CRUD_asp.netMVC.Models.Account.ActionViewModel.Login;
using CRUD_asp.netMVC.Models.Service;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        /// Thay doi name co cac ki tu co dau thanh khong dau
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

        /// <summary>
        /// Dang ki tai khoan, xac thuc email
        /// </summary>
        /// <param name="register"></param>
        /// <param name="emailSender"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Register register, [FromServices] IEmailSender emailSender) // Tao emailSender de gui mail xac thuc
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
                    FirstName = register.FirstName,
                    LastName = register.LastName,
                    Email = register.Email,
                    PhoneNumber = register.Phone,
                    StartDate = register.StartDate,
                    PhoneNumberConfirmed = true, // Xac thuc sdt
                    EmailConfirmed = false, // false email truoc khi gui xac thuc
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                };

                // Tao truoc db user, khi co db thi moi xac dinh duoc du lieu nguoi dung co thong tin roi moi xac dinh vai tro cua ho
                var account = await _userManager.CreateAsync(user, register.Password);

                if (account.Succeeded)
                {
                    // Tao token 
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user); // yser co trong db thi moi tao duoc token
                    var confirmEmail = Url.Action("ConfirmEmail", "Account", new { UserID = user.Id, Token = token }, Request.Scheme); // scheme: http, https

                    try
                    {
                        // Gui email xac thuc
                        await emailSender.SendEmailAsync(
                            user.Email,
                            "Xác thực email",
                            $"<div style='font-family: Arial, sans-serif; text-align: center; padding: 20px; transition: 3s;'>" +
                            $"<h2>Xác thực tài khoản của bạn</h2>" +
                            $"<p>Vui lòng nhấn vào nút bên dưới để xác thực email của bạn:</p>" +
                            $"<a href='{confirmEmail}' style='display: inline-block; padding: 10px 20px; font-size: 16px; font-weight: 400; text-align: center; text-decoration: none; color: #6c757d; border: 1px solid #6c757d; border-radius: 4px; background-color: transparent;'>Xác thực email</a>" +
                            $"<p>Nếu bạn không đăng ký, vui lòng bỏ qua email này.</p>" +
                            $"</div>"
                        );
                        // tra ve thong bao xac thuc email
                        ModelState.AddModelError("Email", "Vui lòng xác thực Email của bạn tại trang https://mail.google.com/");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, "Lỗi khi gửi email xác nhận: " + ex.Message);
                        return View(register);
                    }
                }
                else
                {
                    //await _userManager.DeleteAsync(user);
                    //await _context.SaveChangesAsync();

                    foreach (var error in account.Errors)
                    {
                        Console.WriteLine(error.Code, error.Description);
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                }

                return View(register);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Lỗi đăng kí tài khoản" + ex);
                return View(register);
            }
        }

        public IActionResult Login() => View();

        /// <summary>
        /// Form dang nhap
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login login)
        {
            if (!ModelState.IsValid) return View(login);

            var user = await _userManager.FindByEmailAsync(login.Email.Trim());

            if (user == null || !await _userManager.CheckPasswordAsync(user, login.Password))
            {
                ModelState.AddModelError("Email", "Email không tồn tại !!!");
                return View(login);
            }

            if (!user.EmailConfirmed)
            {
                ModelState.AddModelError("Email", "Email chưa được xác thực !!!");
                return View(login);
            }

            var account = await _signInManager.PasswordSignInAsync(user, login.Password, login.RememberMe, lockoutOnFailure: false);
            if (account.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("InfoGeneral", "Đã xảy ra lỗi khi đăng nhập !!!");
            return View(login);
        }

        [HttpGet] // Duoc dung khi them san pham vao gio hang nhung chua dang nhap
        public IActionResult LoginByProductID(int productID)
        {
            ViewData["id"] = productID;
            return View(nameof(Login));
        }

        [HttpPost, ValidateAntiForgeryToken] // Xac thuc form dang nhap xong thi quay lai product detail de them san pham vao gio hang 
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

        /// <summary>
        /// Form dang xuat tai khoan
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Xac thuc email khi nguoi dung click vao link xac thuc trong email
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<IActionResult> ConfirmEmail(string UserID, string token)
        {
            var user = await _userManager.FindByIdAsync(UserID);
            if (user == null) return View("Không tìm thấy người dùng này");

            // kiem tra khi nguoi dung da xac thuc email lan truoc nhung lai xac thuc lan nua
            if (_context.Manager.Where(p => p.UserID == user.Id).Any() ||
               _context.Staff.Where(p => p.UserID == user.Id).Any() ||
               _context.Customer.Where(p => p.UserID == user.Id).Any())
            {
                return RedirectToAction("Login", "Account");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                string role = "Customer";

                if (user.Email.Contains("nhanvien", StringComparison.OrdinalIgnoreCase))
                {
                    role = "Staff";
                }
                else if (user.Email.Contains("nguyenthanhtuankrp1", StringComparison.OrdinalIgnoreCase))
                {
                    role = "Manager";
                }

                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new Roles { Name = role });
                }

                var RoleID = await _context.Roles.FirstOrDefaultAsync(p => p.Name == role);
                user.RoleID = RoleID.Id;

                // Tao db giua nguoi dung co vai tro 
                await _userManager.AddToRoleAsync(user, role);

                switch (role)
                {
                    case "Manager":
                        _context.Manager.Add(new Manager
                        {
                            UserID = user.Id,
                            UserName = user.UserName,
                            Email = user.Email,
                            StartDate = user.StartDate,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            PhoneNumber = user.PhoneNumber

                        });
                        break;

                    case "Staff":
                        _context.Staff.Add(new Staff
                        {
                            UserID = user.Id,
                            UserName = user.UserName,
                            Email = user.Email,
                            StartDate = user.StartDate,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            PhoneNumber = user.PhoneNumber

                        });
                        break;

                    case "Customer":
                        _context.Customer.Add(new Customer
                        {
                            UserID = user.Id,
                            UserName = user.UserName,
                            Email = user.Email,
                            JoinDate = user.StartDate,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            PhoneNumber = user.PhoneNumber
                        });
                        break;
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
