using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.DTO.Payment;
using CRUD_asp.netMVC.DTO.Payments;
using CRUD_asp.netMVC.Service.GHN;
using CRUD_asp.netMVC.Service.Payments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CRUD_asp.netMVC.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class PaymentController : Controller
    {
        private readonly IGhnService _ghn;
        private readonly AppDBContext _dbContext;
        private readonly ILogger<PaymentController> _logger;
        private readonly ISmsPaymentVerificationService _smsPaymentVerificationService;

        public PaymentController
            (
                    IGhnService ghn,
                    AppDBContext dbContext,
                    ISmsPaymentVerificationService smsPaymentVerificationService,
                    ILogger<PaymentController> logger
            )
        {
            _ghn = ghn;
            _logger = logger;
            _dbContext = dbContext;
            _smsPaymentVerificationService = smsPaymentVerificationService;
        }

        [HttpPost("SmsReceive"), IgnoreAntiforgeryToken]// Nhan sms tu dien thoai
        public async Task<IActionResult> SmsReceive([FromBody] SmsMessage sms)
        {
            try
            {
                if (sms.Message == null || string.IsNullOrEmpty(sms.Message))
                {
                    return BadRequest(new { success = false, message = "Tin nhắn không hợp lệ" });
                }

                var verifyPayment = await _smsPaymentVerificationService.ProcessResultAsync(sms.Message);
                if (!verifyPayment.Success)
                {
                    return BadRequest(new { success = false, message = verifyPayment.Message });
                }

                return Ok(new { success = true, message = sms.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ",\nlỗi phản hồi nhận tin nhắn thanh toán của user!");
                return StatusCode(500, "Lỗi server: Thông báo trạng thái thanh toán không thành công!");
            }
        }

        [HttpPost("~/Payment/PaymentConfirmWallet"), IgnoreAntiforgeryToken]
        public async Task<IActionResult> PaymentConfirmWalletAsync([FromBody] PaymentVerificationEvent evt)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(x => x.Value.Errors.Count > 0)
                        .Select(x => new { x.Key, x.Value.Errors });

                    return BadRequest(errors);
                }

                var result = await _smsPaymentVerificationService.UserConfirmWalletAsync(evt);
                if (!result.Success)
                {
                    return BadRequest(new { success = false, message = "Thanh toán thất bại." });
                }

                return Ok(new { success = true, message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ",\nlỗi phản hồi thông báo thanh toán");
                return StatusCode(500, "Lỗi server: Thông báo phương thức thanh toán trả số dư tỏng ví không thành công!");
            }
        }

        [HttpGet("~/Payment/PaymentStatus")] // Goi trang thanh toan thanh cong
        public async Task<IActionResult> PaymentStatus([FromQuery] string orderId, [FromQuery] string transactionCode)
        {
            try
            {
                var userId = GetUserId();
                var result = await _smsPaymentVerificationService.ResponsePayStatusAsync(orderId, transactionCode, userId);

                if (result.Success)
                {
                    var resGHN = await _ghn.CreateOrderGHNRequestAsync(orderId, userId);

                    if (resGHN != null)
                    {
                        result.Data.Success = true;
                        return View(result.Data);
                    }
                }

                result.Data.Success = false;
                return View(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ", lỗi phản hồi thông báo thanh toán");
                return StatusCode(500, "Lỗi server: Thông báo trạng thái thanh toán không thành công!");
            }
        }

        [HttpPost("~/Payment/RequestGHN")] // Gui yeu cau tao don Giao Hang Nhanh khi thanh toan thanh cong (khong tinh don thieu tien)
        public async Task<IActionResult> RequestGHN([FromBody] string orderId)
        {
            if (string.IsNullOrEmpty(orderId))
            {
                return BadRequest(new { success = false, message = "dữ liệu đơn hàng không tồn tại" });
            }

            int userId = GetUserId();
            var result = await _ghn.CreateOrderGHNRequestAsync(orderId, userId);

            return StatusCode(result.StatusCode, new
            {
                success = result.Success,
                message = result.Message,
                data = result.Data
            });
        }

        private int GetUserId() => int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
    }
}
