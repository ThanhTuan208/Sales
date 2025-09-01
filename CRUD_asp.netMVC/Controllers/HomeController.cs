using AspNetCoreGeneratedDocument;
using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.DTO.Home;
using CRUD_asp.netMVC.Models;
using CRUD_asp.netMVC.Models.Auth;
using CRUD_asp.netMVC.Models.Cart;
using CRUD_asp.netMVC.Models.Product;
using CRUD_asp.netMVC.Models.ViewModels.Home;
using CRUD_asp.netMVC.Service.EmailSender;
using MailKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.CodeDom;
using System.Diagnostics;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace CRUD_asp.netMVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    public readonly AppDBContext context;
    public readonly UserManager<Users> userManager;

    public HomeController(ILogger<HomeController> logger, AppDBContext _context, UserManager<Users> _userManager)
    {
        _logger = logger;
        context = _context;
        userManager = _userManager;
    }

    public async Task<HomeViewModel> MethodGeneral()
    {
        var product = await context.Products.AsNoTracking()
            .Include(p => p.Brands)
            .Include(p => p.Cate)
            .Include(p => p.Gender)
            .Include(p => p.Carts)
            .Where(p => p.FeaturedID == 1)
            .Take(6).OrderByDescending(p => p.ID).ToListAsync();

        var userID = User.Identity.IsAuthenticated ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) : 0;

        var brand = await context.Brand.AsNoTracking().ToListAsync();
        var categories = await context.Category.AsNoTracking().ToListAsync();
        var carts = await context.Carts.Where(p => p.UserID == userID).ToListAsync();

        HomeViewModel ViewModel = new HomeViewModel()
        {
            Products = product,
            Brands = brand,
            Categories = categories,
            Carts = carts,
            MailContact = new MailContactDTO()
        };

        ViewData["cart"] = carts.Count;

        return ViewModel;
    }

    // Hien thi san pham noi bat (Featured) tren giao dien chinh
    public async Task<IActionResult> Index()
    {
        try
        {
            return View(await MethodGeneral());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi load dữ liệu chung: Index");
            return View("Error");
        }
    }

    public async Task<IActionResult> About()
    {
        try
        {
            return View(await MethodGeneral());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi load dữ liệu chung: About");
            return View("Error");
        }
    }

    [HttpGet]
    public async Task<IActionResult> Contact()
    {
        try
        {
            return View(await MethodGeneral());
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
                var FindUser = await userManager.FindByIdAsync(userCurrent.ToString());
                if (FindUser?.Email == null || string.IsNullOrWhiteSpace(FindUser.Email))
                {
                    return Json(new { success = false, message = "Bạn cần sử dụng email của tài khoản mà bạn đã đăng nhập !!!" });
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
            // Gui email den dia chi ho tro
            await emailSender.SendEmailAsync(
                email: "dieuhuong707@gmail.com",
                subject: subject,
                message: htmlBody
            );

            // Gui email xac nhan nguoi dung
            await emailSender.SendEmailAsync(
                email: mail.Email,
                subject: "Xác nhận yêu cầu hỗ trợ - E-commerce",
                message: htmlBodyUser
            );
            #endregion 

            return Json(new { success = true, message = "Yêu cầu của bạn đã được gửi thành công! Vui lòng kiểm tra email để xem xác nhận." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi gửi email hỗ trợ: Contact");
            return Json(new { success = false, message = "Có lỗi xảy ra khi gửi yêu cầu. Vui lòng thử lại sau hoặc liên hệ qua nguyenthanhtuankrp1@gmail.com." });
        }
    }

    // note: chua xu li xong
    public async Task<IActionResult> ProductFeatured()
    {
        try
        {
            return View(await MethodGeneral());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Lỗi load dữ liệu chung: featured");
            return View("Error");
        }
    }

    public string? TestClaimEmail()
    {
        return User.FindFirst(ClaimTypes.Email)?.Value;
    }

}
