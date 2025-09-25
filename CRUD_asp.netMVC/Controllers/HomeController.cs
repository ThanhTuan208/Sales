using AspNetCoreGeneratedDocument;
using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.DTO.Home;
using CRUD_asp.netMVC.HubRealTime;
using CRUD_asp.netMVC.Migrations;
using CRUD_asp.netMVC.Models.Auth;
using CRUD_asp.netMVC.ViewModels.Home;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Globalization;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using IEmailSender = CRUD_asp.netMVC.Service.EmailSender.IEmailSender;

namespace CRUD_asp.netMVC.Controllers;

public class HomeController : Controller
{
    private readonly IMemoryCache _cache;
    private readonly AppDBContext _dbContext;
    private readonly UserManager<Users> _userManager;
    private readonly ILogger<HomeController> _logger;
    private readonly IHubContext<LoadViewHub> _hub;
    private readonly IWebHostEnvironment _environment;

    public HomeController(ILogger<HomeController> logger, AppDBContext _context, UserManager<Users> userManager, IMemoryCache cache, IHubContext<LoadViewHub> hub, IWebHostEnvironment environment)
    {
        _logger = logger;
        _dbContext = _context;
        _userManager = userManager;
        _cache = cache;
        _hub = hub;
        _environment = environment;
    }

    [HttpGet] // Lazy load du lieu cho sp da thanh toan
    public async Task<IActionResult> LoadMoreOrders(int offset = 0, int limit = 5)
    {
        var userID = User.Identity.IsAuthenticated ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0") : 0;
        if (userID == 0) return NotFound();

        var PaymentList = await _dbContext.Payment.Where(p => p.Order.UserID == userID).AsNoTracking()
                                                .Include(p => p.Order).ThenInclude(o => o.Users)
                                                .Include(p => p.Order).ThenInclude(o => o.Address)
                                                .Include(p => p.Order).ThenInclude(o => o.OrderDetail)
                                                                        .ThenInclude(od => od.Product)
                                                                        .ThenInclude(pr => pr.Cate)
                                                                        .OrderByDescending(p => p.ID)
                                                                        .Skip(offset)
                                                                        .Take(limit)
                                                                        .ToListAsync();

        if (!PaymentList.Any())
            return Content("");

        var orderPayList = await _dbContext.OrderDetail.AsNoTracking().Include(p => p.Product).ThenInclude(p => p.Cate).ToListAsync();
        ViewData["OrderPayList"] = orderPayList;

        return PartialView("_OrderPayPartial", PaymentList);
    }

    [HttpGet] // Hien thi trang theo doi don hang da dat
    public async Task<IActionResult> OrderTracking()
    {
        try
        {
            var userID = User.Identity.IsAuthenticated ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value ?? "0") : 0;
            if (userID == 0) return NotFound();

            var orderDetailList = await _dbContext.OrderDetail.AsNoTracking()
                                                                .Include(p => p.Product).ThenInclude(p => p.Cate)
                                                                .ToListAsync();

            var paymentOrderList = await _dbContext.Payment.AsNoTracking()
                                                            .Where(p => p.Order.UserID == userID)
                                                            .Include(p => p.Order).ThenInclude(p => p.Address)
                                                            .Include(p => p.Order).ThenInclude(p => p.Users)
                                                            .OrderByDescending(p => p.ID)
                                                            .Take(2)
                                                            .ToListAsync();

            foreach (var item in paymentOrderList)
            {
                orderDetailList.Where(p => p.OrderID == item.OrderID).ToList();
            }

            var viewModel = await MethodGeneral();
            viewModel.OrderPayList = orderDetailList;
            viewModel.PaymentList = paymentOrderList;

            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi load dữ liệu chung: OrderTracking");
            return BadRequest();
        }
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

    [HttpPost, ValidateAntiForgeryToken] // Cap nhat du lieu ho so
    public async Task<IActionResult> UpdateProfile(UserProfileDTO userDTO)
    {
        try
        {
            var userID = User.Identity.IsAuthenticated ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0") : 0;

            var user = await _dbContext.Users.FirstOrDefaultAsync(p => p.Id == userID);
            if (user == null)
            {
                return Json(new { success = false, message = "Không tìm thấy người dùng này !" });
            }

            if (string.IsNullOrWhiteSpace(userDTO.UserName))
            {
                return Json(new
                {
                    success = false,
                    message = "Bạn cần điền tên người dùng !",
                    errors = new { UserName = new[] { "Bạn cần điền tên người dùng !" } }
                });
            }

            if (!HasDiacritics(userDTO.UserName.Trim()))
            {
                return Json(new
                {
                    success = false,
                    message = "Vui lòng đặt tên người dùng không có dấu, khoảng cách. ",
                    errors = new { UserName = new[] { "Vui lòng đặt tên người dùng không có dấu, khoảng cách. " } }
                });
            }

            if (string.IsNullOrWhiteSpace(userDTO.PhoneNumber))
            {
                return Json(new
                {
                    success = false,
                    message = "Bạn cần điền số điện thoại !",
                    errors = new { Phone = new[] { "Bạn cần điền số điện thoại !" } }
                });
            }

            if (!Regex.IsMatch(userDTO.PhoneNumber.Trim(), @"^(0|\+84)(3[2-9]|5[689]|7[06-9]|8[1-5]|9[0-46-9])[0-9]{7}$"))
            {
                return Json(new
                {
                    success = false,
                    message = "Số điện thoại không đúng định dạng !",
                    errors = new { Phone = new[] { "Số điện thoại không đúng định dạng !" } }
                });
            }

            if (userDTO.ProfileImage != null && userDTO.ProfileImage.Length > 0)
            {
                string nameFile = "";
                var getPathExtentions = Path.GetExtension(userDTO.ProfileImage.FileName).ToLower();
                var fileExtentions = new[] { ".jpg", ".png", ".jpeg", ".webp" };

                if (!fileExtentions.Contains(getPathExtentions))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Không thể tải lên file này, vui lòng chọn file có đuôi jpg, png, jpeg, webp",
                        errors = new { AvatarPreview = new[] { "Không thể tải lên file này, vui lòng chọn file có đuôi jpg, png, jpeg, webp" } }
                    });
                }

                nameFile = Guid.NewGuid().ToString() + getPathExtentions;
                var fileUpLoadPath = Path.Combine(_environment.WebRootPath, "images", "avatar", nameFile).Replace("\\", "/");

                using (var fileStream = new FileStream(fileUpLoadPath, FileMode.Create))
                {
                    await userDTO.ProfileImage.CopyToAsync(fileStream);
                }

                user.ProfileImage = Path.Combine("images", "avatar", nameFile).ToLower().Replace("\\", "/");
            }

            user.UserName = userDTO.UserName;
            user.PhoneNumber = userDTO.PhoneNumber;
            user.DateOfBirth = userDTO.DateOfBirth;
            user.Gender = userDTO.Gender;

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

            return Json(new { success = true, message = "Cập nhật hồ sơ thành công. " });
        }
        catch (Exception ex)
        {

            return Json(new { success = false, message = "Lỗi cập nhật: " + ex.Message });
        }
    }

    [HttpPost, ValidateAntiForgeryToken] // Gui ma email de doi gmail moi
    public async Task<IActionResult> UpdateEmailProfile(string Email, [FromServices] IEmailSender emailSender)
    {
        var userID = User.Identity.IsAuthenticated ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0") : 0;
        if (userID == 0) return NotFound();

        var user = await _dbContext.Users.FirstOrDefaultAsync(p => p.Id == userID);

        if (string.IsNullOrWhiteSpace(Email))
        {
            return Json(new
            {
                success = false,
                message = "Bạn cần điền Email muốn thay đổi !",
                errors = new { EmailNew = new[] { "Bạn cần điền Email muốn thay đổi !" } }
            });
        }

        if (user == null)
        {
            return Json(new { success = false, message = "Không tìm thấy User" });
        }

        if (user?.Email == Email)
        {
            return Json(new
            {
                success = false,
                message = "Email đang được sử dụng.",
                errors = new { EmailNew = new[] { "Email đang được sử dụng." } }
            });
        }

        // Tao token cho action ConfirmEmail
        var otpCode = new Random().Next(0, 9999).ToString("0000");

        string subject = "[Mã xác nhận hỗ trợ]";

        // Nội dung email trả lời
        string htmlBody = $@"
                <p>Xin chào {HttpUtility.HtmlEncode(user.UserName)},</p>
                <p>Chúng tôi đã nhận được yêu cầu liên hệ của bạn.</p>
                <p>Mã xác nhận của bạn là: <strong>{otpCode}</strong></p>
                <p>Vui lòng nhập mã này để tiếp tục quá trình hỗ trợ. sau 5 phút chúng tôi sẽ hủy bỏ mã này, nếu bạn muốn lấy lại mã vui lòng thực hiện lại các bước trên.</p>
                <br/>
                <p><strong>Thông tin người liên hệ:</strong></p>
                <ul>
                    <li><strong>Email:</strong> {HttpUtility.HtmlEncode(Email)}</li>
                    <li><strong>Thời gian gửi:</strong> {DateTime.Now:dd/MM/yyyy HH:mm}</li>
                </ul>
                <p><strong>Liên hệ hỗ trợ:</strong> nguyenthanhtuankrp1@gmail.com | 1900 1234</p>";

        // Luu otp catche trong gmail -> 5p
        _cache.Set($"OTP_{Email}", otpCode, TimeSpan.FromMinutes(5));

        await _hub.Clients.All.SendAsync("LazyLoad");

        // Gửi email trả lời
        await emailSender.SendEmailAsync(
            email: Email.Trim(), // gửi lại cho người dùng
            subject: subject,
            message: htmlBody
        );

        return Json(new
        {
            success = true,
            message = "Đã gửi mã OTP thành công. ",
            email = Email,
            userid = user.Id,
            errors = new { EmailNew = new[] { "Đã gửi mã OTP qua Gmail thành công. " } }
        });
    }

    [HttpPost, ValidateAntiForgeryToken] // Quay lai trang modal 
    public async Task<IActionResult> ConfirmEmail(string NewEmail, string UserID, string OTPCode)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(UserID);
            if (user == null) return View("Không tim thấy người dùng này !!!");

            var verifyOTP = _cache.TryGetValue($"OTP_{NewEmail}", out string? otpCode);
            if (!verifyOTP)
            {
                return Json(new
                {
                    success = false,
                    message = "Mã OTP đã hết hạn hoặc không tồn tại.",
                    errors = new { ConfirmOTPCode = new[] { "Mã OTP đã hết hạn hoặc không tồn tại." } }
                });
            }

            if (otpCode != null && !otpCode.Equals(OTPCode.Trim()))
            {
                return Json(new
                {
                    success = false,
                    message = "Mã OTP không đúng.",
                    errors = new { ConfirmOTPCode = new[] { "Mã OTP không đúng. " } }
                });
            }

            user.Email = NewEmail;
            user.NormalizedEmail = NewEmail.ToUpper();

            await _dbContext.SaveChangesAsync();
            await _hub.Clients.All.SendAsync("ChangeEmailProfile");

            _cache.Remove(NewEmail); // Xoa otp code sau khi doi mat khau

            return Json(new { success = true, message = "Cập nhật Email thành công. " });
        }
        catch (Exception ex)
        {

            return Json(new { success = false, message = "Cập nhật Email lỗi: " + ex.Message });
        }

    }

    [HttpPost] // Dieu huong den trang cap nhat thuoc tinh (Email, phone)
    public async Task<IActionResult> RedirecToEmailProfile()
    {
        try
        {
            var viewModel = await MethodGeneral();
            return PartialView("_ModalChangeProfilePartial", viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi load dữ liệu chung: Profile");
            return BadRequest();
        }
    }


    [HttpGet] // Hien thi giao dien trang chu
    public async Task<IActionResult> MyProfile()
    {
        try
        {
            var viewModel = await MethodGeneral();
            if (viewModel.User == null) return NotFound();

            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi load dữ liệu chung: Profile");
            return BadRequest();
        }
    }

    [HttpGet] // Hien thi giao dien trang chu
    public async Task<IActionResult> Index()
    {
        try
        {
            var viewModel = await MethodGeneral();
            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi load dữ liệu chung: Index");
            return BadRequest();
        }
    }

    [HttpGet] // Hien thi form gioi thieu
    public async Task<IActionResult> About()
    {
        try
        {
            var viewModel = await MethodGeneral();
            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi load dữ liệu chung: About");
            return BadRequest();
        }
    }

    [HttpGet] // Hien thi form lien he
    public async Task<IActionResult> Contact()
    {
        try
        {
            var viewModel = await MethodGeneral();
            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi load dữ liệu chung: Contact");
            return BadRequest();
        }
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Contact(MailContactDTO mail, [FromServices] IEmailSender emailSender)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, message = "Vui lòng nhập " + string.Join(", ", errors.OrderBy(e => e)) });
            }

            if (!User.Identity.IsAuthenticated || !int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userCurrent))
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập tài khoản trước khi gửi yêu cầu của bạn" });
            }

            var ClaimCurrentEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrWhiteSpace(ClaimCurrentEmail))
            {
                var FindUser = await _userManager.FindByIdAsync(userCurrent.ToString());
                if (FindUser?.Email == null || string.IsNullOrWhiteSpace(FindUser.Email))
                {
                    return Json(new { success = false, message = "Bạn cần sử dụng Email của tài khoản mà bạn đã đăng nhập !!!" });
                }
            }

            string subject = $"[Liên hệ hỗ trợ] - {HttpUtility.HtmlEncode(mail.Subject)}";

            string htmlBody = $@"
            <p><strong>Thông tin người liên hệ:</strong></p>
            <ul>
                <li><strong>Họ tên:</strong> {HttpUtility.HtmlEncode(mail.LastName)} {HttpUtility.HtmlEncode(mail.FirstName)}</li>
                <li><strong>Email:</strong> {HttpUtility.HtmlEncode(mail.Email)}</li>
            </ul>
            <p><strong>Tiêu đề:</strong> {HttpUtility.HtmlEncode(mail.Subject)}</p>
            <p><strong>Nội dung tin nhắn:</strong></p>
            <p>{HttpUtility.HtmlEncode(mail.Message)}</p>
            <p><strong>Thời gian gửi:</strong> {DateTime.Now:dd/MM/yyyy HH:mm}</p>
            <p><strong>Liên hệ hỗ trợ:</strong> nguyenthanhtuankrp1@gmail.com | 1900 1234</p>";

            string htmlBodyUser = $@"<p>Xin chào {HttpUtility.HtmlEncode(mail.FirstName)},</p>
            <p>Cảm ơn bạn đã liên hệ với chúng tôi. Yêu cầu của bạn với tiêu đề '<strong>{HttpUtility.HtmlEncode(mail.Subject)}</strong>' đã được ghi nhận.</p>
            <p>Chúng tôi sẽ phản hồi trong vòng 24 giờ. Nếu cần hỗ trợ ngay, vui lòng liên hệ: nguyenthanhtuankrp1@gmail.com hoặc một chín không không một không không có.</p>
            <p>Trân trọng,<br>E-commerce</p>";

            #region Tối ưu code bằng hangfire (Gửi email nhanh hơn với queue (FIFO))
            // Gui Email den dia chi ho tro
            await emailSender.SendEmailAsync(
                email: "dieuhuong707@gmail.com",
                subject: subject,
                message: htmlBody
            );

            // Gui Email xac nhan nguoi dung
            await emailSender.SendEmailAsync(
                email: mail.Email,
                subject: "Xác nhận yêu cầu hỗ trợ - E-commerce",
                message: htmlBodyUser
            );
            #endregion 

            return Json(new { success = true, message = "Yêu cầu của bạn đã được gửi thành công! Vui lòng kiểm tra Email để xem xác nhận." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi gửi Email hỗ trợ: Contact");
            return Json(new { success = false, message = "Có lỗi xảy ra khi gửi yêu cầu. Vui lòng thử lại sau hoặc liên hệ qua nguyenthanhtuankrp1@gmail.com." });
        }
    }

    // Phuong thuc load du lieu chung
    public async Task<HomeViewModel> MethodGeneral()
    {
        var product = await _dbContext.Products.AsNoTracking()
            .Include(p => p.Brands)
            .Include(p => p.Cate)
            .Include(p => p.Gender)
            .Include(p => p.Carts)
            .Where(p => p.FeaturedID == 1)
            .Take(6).OrderByDescending(p => p.ID).ToListAsync();

        var userID = User.Identity.IsAuthenticated ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) : 0;

        var brand = await _dbContext.Brand.AsNoTracking().ToListAsync();
        var categories = await _dbContext.Category.AsNoTracking().ToListAsync();
        var carts = await _dbContext.Carts.Where(p => p.UserID == userID).ToListAsync();
        var user = await _dbContext.Users.FirstOrDefaultAsync(p => p.Id == userID);

        HomeViewModel ViewModel = new HomeViewModel()
        {
            Products = product,
            Brands = brand,
            Categories = categories,
            Carts = carts,
            User = user
        };

        ViewData["cart"] = carts.Count;

        return ViewModel;
    }

}
