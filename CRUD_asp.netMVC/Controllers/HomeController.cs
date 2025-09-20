using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.DTO.Home;
using CRUD_asp.netMVC.HubRealTime;
using CRUD_asp.netMVC.Models.Auth;
using CRUD_asp.netMVC.ViewModels.Home;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Identity.Client;
using System.Security.Claims;
using System.Web;
using IEmailSender = CRUD_asp.netMVC.Service.EmailSender.IEmailSender;

namespace CRUD_asp.netMVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    public readonly AppDBContext _dbContext;
    public readonly UserManager<Users> _userManager;
    public readonly IMemoryCache _cache;
    public readonly IHubContext<LoadViewHub> _hub;


    public HomeController(ILogger<HomeController> logger, AppDBContext _context, UserManager<Users> userManager, IMemoryCache cache, IHubContext<LoadViewHub> hub)
    {
        _logger = logger;
        _dbContext = _context;
        _userManager = userManager;
        _cache = cache;
        _hub = hub;
    }

    [HttpPost, ValidateAntiForgeryToken] // Gui ma email de doi gmail moi
    public async Task<IActionResult> UpdateEmailProfile(string Email, [FromServices] IEmailSender emailSender)
    {
        var userID = User.Identity.IsAuthenticated ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) : 0;
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

    // Thay doi thuoc tinh Email, sdt cho nguoi dung
    //public async Task<IActionResult> UpdatePropProfile(string prop, bool IsProp)
    //{
    //    // IsProp == true => Email, false => PhoneNumber
    //    if (IsProp)
    //    {
    //        var existEmail = await _userManager.FindByEmailAsync(prop.Trim());
    //        if (existEmail != null)
    //        {
    //            return Json(new
    //            {
    //                success = false,
    //                message = "Email đã tồn tại",
    //                errors = new { Email = new[] { "Email đã tồn tại, vui lòng kiểm tra lại tải khoản !!!" } }
    //            });
    //        }
    //    }
    //}

    [HttpPost] // Dieu huong den trang cap nhat thuoc tinh (Email, phone)
    public async Task<IActionResult> RedirectPropProfile(bool IsProp)
    {
        // IsProp == true => Email, false => PhoneNumber
        var viewModel = await MethodGeneral();

        ViewBag.IsSelectProp = IsProp;
        ViewBag.NameProp = IsProp ? "địa chỉ Email" : "điện thoại";

        return PartialView("_ModalChangeProfilePartial", viewModel);
    }


    [HttpGet] // Hien thi giao dien trang chu
    public async Task<IActionResult> MyProfile()
    {
        try
        {
            var viewModel = await MethodGeneral();
            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi load dữ liệu chung: Profile");
            return View("Error");
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
            return View("Error");
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
            return View("Error");
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
            return View("Error");
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
            User = user,
            MailContact = null
        };

        ViewData["cart"] = carts.Count;

        return ViewModel;
    }

}
