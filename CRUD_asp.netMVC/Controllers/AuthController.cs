using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.DTO.Auth;
using CRUD_asp.netMVC.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol;
using SixLabors.ImageSharp.Formats.Bmp;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using IEmailSender = CRUD_asp.netMVC.Models.Service.IEmailSender;
using Login = CRUD_asp.netMVC.DTO.Auth.Login;
using Register = CRUD_asp.netMVC.DTO.Auth.Register;

namespace CRUD_asp.netMVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;
        private readonly RoleManager<Roles> _roleManager;
        private readonly AppDBContext _context;
        private readonly IMemoryCache _cache;

        public AuthController(UserManager<Users> userManager,
                                SignInManager<Users> signInManager,
                                AppDBContext appDBContext,
                                RoleManager<Roles> roleManager,
                                IMemoryCache cache)
        {

            _userManager = userManager;
            _signInManager = signInManager;
            _context = appDBContext;
            _roleManager = roleManager;
            _cache = cache;
        }

        // Kiem tra chuoi co dau
        public bool HasDiacritics(string text)
        {
            string removeDiacritics = RemoveDiacritics(text) ?? string.Empty;
            return text.Equals(removeDiacritics) ? true : false;
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
                    return Json(new
                    {
                        success = false,
                        message = "Nhập email và mật khẩu của bạn !!!",
                        errors = new { InfoGeneral = new[] { "Nhập email và mật khẩu của bạn !!!" } }
                        // tra ve loi moi khi submit neu co loi
                    });
                }

                var user = await _userManager.FindByEmailAsync(login.Email.Trim());
                if (user == null || !await _userManager.CheckPasswordAsync(user, login.Password))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Email hoặc mật khẩu không đúng !!!",
                        errors = new { InfoGeneral = new[] { "Email hoặc mật khẩu không đúng !!!" } }
                        // tra ve loi moi khi submit neu co loi
                    });
                }

                if (!user.EmailConfirmed)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Email chưa được xác thực !!!",
                        errors = new { InfoGeneral = new[] { "Bạn cần truy cập vào Gmail để xác thực tài khoản !" } }
                    });
                }

                var account = await _signInManager.PasswordSignInAsync(user, login.Password, login.RememberMe, lockoutOnFailure: false);
                if (account.Succeeded)
                {
                    var roleUser = await _userManager.GetRolesAsync(user);
                    if (roleUser.Contains("Admin"))
                    {
                        return Json(new { success = true, role = "Admin", message = "Đăng nhập thành công acc Admin." });
                    }
                    else
                    {
                        return Json(new { success = true, role = "Customer", message = "Đăng nhập thành công acc Customer." });

                    }
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = "Đã xảy ra lỗi khi đăng nhập !!!",
                        errors = new { InfoGeneral = new[] { "Đăng nhập thất bại !" } }
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new { responseText = "Đã xảy ra lỗi khi đăng nhập. " + ex.Message });
            }
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

                // Kiem tra username co dau
                if (!HasDiacritics(register.UserName.Trim()))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Vui lòng đặt tên người dùng không có dấu, khoảng cách. ",
                        errors = new { UserName = new[] { "Vui lòng đặt tên người dùng không có dấu, khoảng cách. " } }
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
                        errors = new { UserName = new[] { "Tên của bạn đã bị trùng, bạn cần đổi tên khác. " } }
                    });
                }

                var existEmail = await _userManager.FindByEmailAsync(register.Email.Trim());
                if (existEmail != null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Email đã tồn tại",
                        errors = new { Email = new[] { "Email đã tồn tại, vui lòng kiểm tra lại tải khoản !!!" } }
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

                // Tao truoc db user, kem mat khau duoc haspass
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
                //ModelState.AddModelError(string.Empty, "Lỗi đăng kí tài khoản")
                return Json(new { success = false, message = "Lỗi đăng kí tài khoản: " + ex.Message });
            }
        }

        [HttpPost, ValidateAntiForgeryToken] /// Gui ma email de doi mat khau moi
        public async Task<IActionResult> SendOTPCodeMail(ForgotPassword forgot, [FromServices] IEmailSender emailSender)
        {
            try
            {
                if (forgot.Email == null || string.IsNullOrWhiteSpace(forgot.Email.Trim()))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Vui lòng nhập Email !!!",
                        errors = new { Email = new[] { "Vui lòng nhập Email !!!" } }
                    });
                }

                var accout = await _userManager.FindByEmailAsync(forgot.Email.Trim());
                if (accout == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = $"Email {forgot.Email.Trim()} Không tồn tại !!!",
                        errors = new { Email = new[] { $"Email '{forgot.Email.Trim()}' Không tồn tại !!!" } }
                    });
                }

                var otpCode = new Random().Next(1000, 9999).ToString("0000");

                string subject = "[Mã xác nhận hỗ trợ]";

                // Nội dung email trả lời
                string htmlBody = $@"
                <p>Xin chào {HttpUtility.HtmlEncode(accout.UserName)},</p>
                <p>Chúng tôi đã nhận được yêu cầu liên hệ của bạn.</p>
                <p>Mã xác nhận của bạn là: <strong>{otpCode}</strong></p>
                <p>Vui lòng nhập mã này để tiếp tục quá trình hỗ trợ. sau 5 phút chúng tôi sẽ hủy bỏ mã này, nếu bạn muốn lấy lại mã vui lòng thực hiện lại các bước trên.</p>
                <br/>
                <p><strong>Thông tin người liên hệ:</strong></p>
                <ul>
                    <li><strong>Email:</strong> {HttpUtility.HtmlEncode(accout.Email)}</li>
                    <li><strong>Thời gian gửi:</strong> {DateTime.Now:dd/MM/yyyy HH:mm}</li>
                </ul>
                <p><strong>Liên hệ hỗ trợ:</strong> nguyenthanhtuankrp1@gmail.com | 1900 1234</p>";

                // Luu otp catche trong gmail -> 5p
                _cache.Set($"OTP_{accout.Email}", otpCode, TimeSpan.FromMinutes(5));

                // Gửi email trả lời
                await emailSender.SendEmailAsync(
                    email: accout.Email, // gửi lại cho người dùng
                    subject: subject,
                    message: htmlBody
                );

                return Json(new
                {
                    success = true,
                    message = "Đã gửi mã OTP thành công. ",
                    corrects = new { Email = new[] { "Đã gửi mã OTP qua Gmail thành công. " } }
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Đã gửi mã otp không thành công:  " + ex.Message
                });
            }
        }

        [HttpGet] // Quen mat khau
        public IActionResult ForgotPassword() => View();


        [HttpPost, ValidateAntiForgeryToken] // Xu li sau kho co code mail
        public async Task<IActionResult> ForgotPassword(ForgotPassword forgot)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Cần nhập các ô đang trống !!!",
                        errors = new { InfoGeneral = new[] { $"Cần nhập các ô đang trống !!!" } }
                    });
                }

                if (string.IsNullOrWhiteSpace(forgot.Code.Trim()) || forgot.Code.Length != 4)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Mã OPT phải có 4 số. ",
                        errors = new { InfoGeneral = new[] { $"Mã OPT phải có 4 số. " } }
                    });
                }

                var regex = new Regex(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[~!@#$%^&*()_+=?])[A-Za-z0-9~!@#$%^&*()_+=?]{8,20}$");
                if (!regex.IsMatch(forgot.NewPass.Trim()))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Mã chưa chính xác. ",
                        errors = new { InfoGeneral = new[] { "Mật khẩu phải từ 8-20 ký tự, bao gồm ít nhất 1 chữ hoa, 1 chữ thường, 1 số và 1 ký tự đặc biệt (~!@#$%^&*()_+=?).\"" } }
                    });
                }

                var user = await _userManager.FindByEmailAsync(forgot.Email.Trim());
                if (user == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Cần nhập đúng email tài khoản của bạn. ",
                        errors = new { InfoGeneral = new[] { $"cần nhập đúng email tài khoản của bạn. " } }
                    });
                }

                var verifyOTP = _cache.TryGetValue($"OTP_{forgot.Email}", out string? otpCode);
                if (!verifyOTP)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Mã OTP đã hết hạn hoặc không tồn tại.",
                        errors = new { InfoGeneral = new[] { "Mã OTP đã hết hạn hoặc không tồn tại." } }
                    });
                }

                if (otpCode != null && !otpCode.Equals(forgot.Code.Trim()))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Mã OTP không đúng.",
                        errors = new { InfoGeneral = new[] { "Mã OTP không đúng. " } }
                    });
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                await _userManager.ResetPasswordAsync(user, token, forgot.NewPass);

                _cache.Remove(forgot.Email); // Xoa otp code sau khi doi mat khau

                return Json(new { success = true, message = "Mật khẩu đươc đổi thành công. " });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Mã otp gửi không thành công: " + ex.Message });
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
                string roleName = "Customer";

                if (user.RoleID == 1)
                {
                    roleName = "Admin";
                }

                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new Roles { Name = roleName });
                }

                var RoleID = await _context.Roles.FirstOrDefaultAsync(p => p.Name == roleName);
                user.RoleID = RoleID.Id;

                // Tao db giua nguoi dung co vai tro 
                await _userManager.AddToRoleAsync(user, roleName);

                switch (roleName)
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
