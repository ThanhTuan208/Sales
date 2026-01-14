using AspNetCoreGeneratedDocument;
using CRUD_asp.netMVC.Common;
using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.DTO.Generic;
using CRUD_asp.netMVC.DTO.Order.GHN;
using CRUD_asp.netMVC.DTO.Payment;
using CRUD_asp.netMVC.DTO.Payments;
using CRUD_asp.netMVC.Hubs;
using CRUD_asp.netMVC.Models.Order;
using CRUD_asp.netMVC.Models.Product;
using CRUD_asp.netMVC.Service.GHN;
using CRUD_asp.netMVC.Service.Payments;
using CRUD_asp.netMVC.ViewModels.Order;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using System.Security.Claims;
using System.Text;

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
                    GhnService ghn,
                    AppDBContext dbContext,
                    IConnectionMultiplexer reids,
                    ISmsPaymentVerificationService smsPaymentVerificationService,
                    IHubContext<PaymentHub> hub,
                    ILogger<PaymentController> logger
            )
        {
            _ghn = ghn;
            _dbContext = dbContext;
            //_qrCodeService = qrCodeService;
            _smsPaymentVerificationService = smsPaymentVerificationService;
            _logger = logger;

            //_token = config["GHN:Token"] ?? "";
            //_httpClient = httpClient;
            //_httpClient.DefaultRequestHeaders.Add("Token", _token);
            //_httpClient.DefaultRequestHeaders.Add("_dbContext-Type", "application/json"); 
        }

        [HttpGet("InfoPayment")]
        public async Task<IActionResult> InfoPayment()
        {
            var s = await _dbContext.Payment.ToListAsync();
            if (s.Any())
            {
                return Ok(s);
            }
            return BadRequest();
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
                    return BadRequest(new { success = false, message = result.Message });
                }

                return Ok(new { success = true, message = "Thanh toán thành công." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ",\nlỗi phản hồi thông báo thanh toán");
                return StatusCode(500, "Lỗi server: Thông báo phương thức thanh toán trả số dư tỏng ví không thành công!");
            }
        }

        [HttpGet("~/Payment/PaymentSuccess")] // Goi trang thanh toan thanh cong
        public async Task<IActionResult> PaymentSuccess([FromQuery] string orderId, [FromQuery] string transactionCode)
        {
            try
            {
                var userId = GetUserId();
                var result = await _smsPaymentVerificationService.ResponsePayStatusAsync(orderId, transactionCode, userId);

                if (result.Data == null) return BadRequest();

                return result.Data != null ? View(result.Data) : BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ", lỗi phản hồi thông báo thanh toán");
                return StatusCode(500, "Lỗi server: Thông báo trạng thái thanh toán không thành công!");
            }
        }

        [HttpPost("~/Payment/RequestGHN")] // Gui yeu cau tao don Giao Hang Nhanh khi thanh toan thanh cong (khong tinh don thieu tien)
        public async Task<IActionResult> RequestGHN([FromBody] Orders order)
        {
            if (order == null)
            {
                return BadRequest(new { success = false, message = "dữ liệu đơn hàng không tồn tại" });
            }

            var result = await _ghn.CreateOrderGHNRequestAsync(order);

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

