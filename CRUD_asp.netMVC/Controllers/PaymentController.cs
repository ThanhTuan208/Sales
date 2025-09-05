using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.DTO.Payment;
using CRUD_asp.netMVC.Models.Order;
using CRUD_asp.netMVC.Service.Payment;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace CRUD_asp.netMVC.Controllers
{
    public class PaymentController : Controller
    {
        private readonly QrCodeService _qrCodeService;
        private readonly ISmsPaymentVerificationService _smsPaymentVerificationService;
        private readonly AppDBContext _dbContext;

        public PaymentController(QrCodeService qrCodeService, ISmsPaymentVerificationService smsPaymentVerificationService, AppDBContext dbContext)
        {
            _qrCodeService = qrCodeService;
            _smsPaymentVerificationService = smsPaymentVerificationService;
            _dbContext = dbContext;
        }

        [HttpPost, ValidateAntiForgeryToken] // Tao phuong thuc thanh toan luu vao db Order
        public async Task<IActionResult> CreatePayment([FromBody] PaymentRequest request)
        {
            try
            {
                var order = new Orders()
                {
                    ID = Guid.NewGuid().ToString(),
                    Amount = request.Amount,
                    Status = "Pending",
                    OrderDate = DateTime.Now
                };

                await _dbContext.Orders.AddAsync(order);
                await _dbContext.SaveChangesAsync();

                return Json(new
                {
                    success = true,
                    orderId = order.ID,
                    message = "Thanh toán thành công. "
                });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Thanh toán thất bại !!! " });

            }
        }

        [HttpGet] // Hien thi QR len giao dien qua truy van orderID
        public async Task<IActionResult> ShowQRCode(string orderID)
        {
            var order = await _dbContext.Orders.FindAsync(orderID);
            if (order == null) return NotFound();

            string bankAccount = "0001335756540";
            // goi method generator QR ben class QrCodeService
            var qrUrl = _qrCodeService.GenerateBankQrCode(orderID, order.Amount, bankAccount);

            var model = new QrPaymentViewModel()
            {
                OrderId = orderID,
                Amount = order.Amount,
                QrCodeUrl = qrUrl,
                BankAccount = bankAccount,
                PollingUrl = Url.Action("CheckPaymentStatus", "Payment", new { orderId = orderID }, Request.Scheme)
            };

            return View(model);
        }

        [HttpGet("check-status/{orderId}")] // kiem tra 
        public async Task<IActionResult> CheckPaymentStatus(string orderId)
        {
            var isPaid = await _smsPaymentVerificationService.CheckPaymentAsync(orderId);
            if (isPaid)
            {
                return Json(new { success = true, message = "Kiểm tra thanh toán true" });
            }

            return Json(new { success = false, message = "Thanh toán thất bại" });
        }
    }
}
