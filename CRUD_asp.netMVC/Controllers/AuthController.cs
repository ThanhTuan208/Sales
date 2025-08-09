using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;
using Register = CRUD_asp.netMVC.Models.Auth.ActionViewModel.Register;
using Login = CRUD_asp.netMVC.Models.Auth.ActionViewModel.Login;
using CRUD_asp.netMVC.Models.Service;
using System.Diagnostics.Eventing.Reader;
using Microsoft.AspNetCore.Authorization;
using Org.BouncyCastle.Tls;

namespace CRUD_asp.netMVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;
        private readonly RoleManager<Roles> _roleManager;
        private readonly AppDBContext _context;

        public AuthController(UserManager<Users> userManager, SignInManager<Users> signInManager, AppDBContext appDBContext, RoleManager<Roles> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = appDBContext;
            _roleManager = roleManager;
        }

        [HttpGet, Route("Auth/Register")]
        public IActionResult Register() => View();

        // Tro toi url dang ky admin de tao tk noi bo
        //[Authorize(Roles = "Admin")]
        [HttpGet, Route("Auth/Register/Admin")]
        public IActionResult Register(string pass)
        {
            if (pass != null && pass.ToLower().Trim() == "nhonaovay1")
            {
                ViewData["pass"] = pass;

                return View();
            }

            return Redirect("/Auth/Register"); // Link tuyet doi
        }

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
                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(ms => ms.Value.Errors.Count > 0)
                        .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );
                    return Json(new
                    {
                        success = false,
                        message = "Nhập thông tin đăng kí tài khoản của bạn !!!",
                        errors = errors
                    });
                }

                // Kiem tra loi trung username trong identity
                var UserNameExists = await _userManager.FindByNameAsync(RemoveDiacritics(register.UserName));
                if (UserNameExists != null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Tên của bạn đã bị trùng, bạn cần đổi tên khác. ",
                        errors = new {  UserName = new[] { "Tên của bạn đã bị trùng, bạn cần đổi tên khác. " } }
                    });
                }

                var existEmail = await _userManager.FindByEmailAsync(register.Email.Trim());
                if (existEmail != null)
                {
                    //ModelState.AddModelError("Email", "Email đã tồn tại");
                    return Json(new
                    {
                        success = false,
                        message = "Email đã tồn tại",
                        errors = new { Email = new[] { "Email đã tồn tại" } }
                    });
                }

                var user = new Users
                {
                    UserName = RemoveDiacritics(register.UserName),
                    FirstName = register.FirstName,
                    LastName = register.LastName,
                    Email = register.Email,
                    PhoneNumber = register.Phone,
                    RoleID = register.RoleID,
                    StartDate = register.StartDate,
                    PhoneNumberConfirmed = true, // Xac thuc sdt
                    EmailConfirmed = false, // false email truoc khi gui xac thuc
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                };

                // Tao truoc db user, khi co db thi moi xac dinh duoc du lieu nguoi dung co thong tin roi moi xac dinh vai tro cua ho
                var account = await _userManager.CreateAsync(user, register.Password);

                if (account.Succeeded)
                {
                    // Tao token cho action ConfirmEmail
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user); // user co trong db thi moi tao duoc token
                    var confirmEmail = Url.Action("ConfirmEmail", "Auth", new { UserID = user.Id, Token = token }, Request.Scheme); // scheme: http, https

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
                        //ModelState.AddModelError("Email", "Vui lòng xác thực Email của bạn tại trang https://mail.google.com/");
                        return Json(new
                        {
                            success = true,
                            message = "Vui lòng xác thực Email của bạn tại trang https://mail.google.com/",
                            errors = new { Email = new[] { "Vui lòng xác thực Email của bạn tại trang https://mail.google.com/" } }
                        });
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, "Lỗi khi gửi email xác nhận: " + ex.Message);
                        return Json(new { success = false, message = "Lỗi khi gửi email xác nhận: " + ex.Message });
                    }
                }
                else
                {
                    foreach (var error in account.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                }

                return View(register);
            }
            catch (Exception ex)
            {
                //ModelState.AddModelError(string.Empty, "Lỗi đăng kí tài khoản");
                return Json(new { success = false, message = "Lỗi đăng kí tài khoản: " + ex.Message });
            }
        }

        [HttpGet]
        public IActionResult Login() => View();

        /// <summary>
        /// Form dang nhap
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login login)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("InfoGeneral", "Cần nhập đủ email và mật khẩu của bạn!!!");
                    return Json(new
                    {
                        success = false,
                        message = "Nhập email và mật khẩu của bạn !!!",
                        errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                        // tra ve loi moi khi submit neu co loi
                    });
                }

                var user = await _userManager.FindByEmailAsync(login.Email.Trim());

                if (user == null || !await _userManager.CheckPasswordAsync(user, login.Password))
                {
                    ModelState.AddModelError("InfoGeneral", "Email hoặc mật khẩu không đúng !!!");
                    return Json(new
                    {
                        success = false,
                        message = "Email hoặc mật khẩu không đúng !!!",
                        errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                        // tra ve loi moi khi submit neu co loi
                    });
                }

                if (!user.EmailConfirmed)
                {
                    //ModelState.AddModelError("Email", "Email chưa được xác thực !!!");
                    return Json(new
                    {
                        success = false,
                        message = "Email chưa được xác thực !!!",
                        errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                    });
                }

                var account = await _signInManager.PasswordSignInAsync(user, login.Password, login.RememberMe, lockoutOnFailure: false);
                if (account.Succeeded)
                {
                    return Json(new { success = true, message = "Đăng nhập thành công." });
                }

                ModelState.AddModelError("InfoGeneral", "Đã xảy ra lỗi khi đăng nhập !!!");

                return Json(new
                {
                    success = false,
                    message = "Đã xảy ra lỗi khi đăng nhập !!!",
                    errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                });
            }
            catch (Exception ex)
            {
                return Json(new { responseText = "Lỗi đăng nhập tài khoản: " + ex.Message });
            }
        }

        [HttpGet] // Duoc dung khi them san pham vao gio hang nhung chua dang nhap
        public IActionResult LoginByProductID(int id)
        {
            ViewData["id"] = id;
            return View(nameof(Login));
        }

        [HttpPost, ValidateAntiForgeryToken] // Xac thuc form dang nhap xong thi quay lai product detail de them san pham vao gio hang 
        public async Task<IActionResult> LoginByProductID(Login login, int id)
        {
            if (!ModelState.IsValid) return View(login);

            ViewData["id"] = id;

            var user = await _userManager.FindByEmailAsync(login.Email.Trim());
            if (user == null || !await _userManager.CheckPasswordAsync(user, login.Password))
            {
                ModelState.AddModelError("InfoGeneral", "Email hoặc mật khẩu không đúng !!!");
                return Json(new
                {
                    success = false,
                    message = "Email hoặc mật khẩu không đúng !!!",
                    errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                    // tra ve loi moi khi submit neu co loi
                });
            }

            var account = await _signInManager.PasswordSignInAsync(user, login.Password, login.RememberMe, lockoutOnFailure: false);
            if (account.Succeeded)
            {
                return Json(new { success = true, authenticated = 1, message = $"đăng nhập thành công, điều hướng tới sản phẩm ID: {id}", productID = id });
                //return RedirectToAction("ProductDetail", "Product", new { id = id });
            }

            ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi trong quá trình đăng nhập !!!");
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

            return RedirectToAction("Login", "Auth");
        }

        /// <summary>
        /// Xac thuc email khi nguoi dung click vao link xac thuc trong email
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string UserID, string token)
        {
            var user = await _userManager.FindByIdAsync(UserID);
            if (user == null) return View("Không tim thấy người dùng này !!!");

            // kiem tra khi nguoi dung da xac thuc email lan truoc nhung lai xac thuc lan nua
            if (_context.Manager.Where(p => p.UserID == user.Id).Any() ||
               _context.Customer.Where(p => p.UserID == user.Id).Any())
            {
                return RedirectToAction("Login", "Auth");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                string role = "Customer";

                if (user.RoleID == 1)
                {
                    role = "Admin";
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
            return RedirectToAction("Login", "Auth");
        }
    }
}
