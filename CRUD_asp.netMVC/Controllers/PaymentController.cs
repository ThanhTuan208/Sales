using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.DTO.Payment;
using CRUD_asp.netMVC.HubRealTime;
using CRUD_asp.netMVC.Models.Order;
using CRUD_asp.netMVC.Models.Product;
using CRUD_asp.netMVC.Service.Payment;
using CRUD_asp.netMVC.ViewModels.Order;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace CRUD_asp.netMVC.Controllers
{
    public class PaymentController : Controller
    {
        private readonly AppDBContext _dbContext;
        private readonly QrCodeService _qrCodeService;
        private readonly ISmsPaymentVerificationService _smsPaymentVerificationService;

        public PaymentController(QrCodeService qrCodeService, ISmsPaymentVerificationService smsPaymentVerificationService, AppDBContext dbContext)
        {
            _qrCodeService = qrCodeService;
            _smsPaymentVerificationService = smsPaymentVerificationService;
            _dbContext = dbContext;
        }

        [HttpPost] // Nhan sms tu dien thoai
        public async Task<IActionResult> SmsReceive([FromBody] SmsMessage sms, [FromServices] IHubContext<PaymentHub> hub)
        {
            try
            {
                var NDMessage = sms.Message.Split("ND:")[1].Trim();
                var transactionCode = NDMessage.Split('-')[1];

                if (!transactionCode.Contains("ORD")) return BadRequest();

                // lay ma giao dich
                var formartTransaction = transactionCode.Split("ORD")[1];

                var order = await _dbContext.Orders
                    .AsNoTracking()
                    .Include(p => p.OrderDetail)
                    .FirstOrDefaultAsync(p => p.TransactionId == formartTransaction);

                if (order == null) return NotFound();

                var payment = new Payment()
                {
                    OrderID = order.ID,
                    paidAmount = order.Amount,
                    PaymentDate = DateTime.Now,
                    paymentMethod = order.PaymentMethod
                };

                order.Payment = payment;
                order.Status = "Paid"; // cap nhat lai trang thai sau khi chuyen khoan thanh cong
                order.PaidAt = DateTime.Now;

                var cart = await _dbContext.Carts.FirstOrDefaultAsync(p => p.ProductID == order.OrderDetail.FirstOrDefault().ProductID);

                if (cart != null)
                {
                    _dbContext.Carts.Remove(cart);
                }

                _dbContext.Orders.Update(order);

                await _dbContext.SaveChangesAsync();

                // phat real time sau khi cap nhat db
                await hub.Clients.All.SendAsync("ReceivePaymentStatus", order.ID, formartTransaction);

                return Json(new { sucess = true, message = "Thanh toán thành công." });
            }
            catch (Exception ex)
            {
                return Json(new { sucess = false, message = "Thanh toán thất bại: " + ex.Message });
            }
        }

        [HttpGet] // Goi trang thanh toan thanh cong
        public async Task<IActionResult> PaymentSuccess(string orderID, string transactionCode)
        {
            if (string.IsNullOrEmpty(orderID) || string.IsNullOrEmpty(transactionCode))
            {
                return BadRequest();
            }

            var order = await _dbContext.Orders
                .AsNoTracking()
                .Include(p => p.OrderDetail).ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(p => p.ID == orderID && p.TransactionId == transactionCode && p.Status == "Paid");

            var cateID = await _dbContext.OrderDetail.Where(p => p.OrderID == order.ID).Select(p => p.Product.CateID).FirstOrDefaultAsync();

            var product = await _dbContext.Products.Where(p => p.CateID == cateID).ToListAsync();

            Random rand = new Random();
            var shuffledProduct = product.OrderBy(p => rand.Next()).Take(4).ToList();

            var viewModel = new GeneralOrderViewModel()
            {
                Product = shuffledProduct,
                Order = order
            };

            if (viewModel == null) return BadRequest();

            return View(viewModel);
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

