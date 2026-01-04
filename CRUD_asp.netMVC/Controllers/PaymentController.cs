using CRUD_asp.netMVC.Data;
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
using StackExchange.Redis;
using System.Security.Claims;
using System.Text;

namespace CRUD_asp.netMVC.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class PaymentController : Controller
    {
        private readonly GhnService _ghn;
        private readonly AppDBContext _dbContext;
        private readonly ISmsPaymentVerificationService _smsPaymentVerificationService;

        public PaymentController
            (
                    GhnService ghn,
                    AppDBContext dbContext,
                    IConnectionMultiplexer reids,
                    ISmsPaymentVerificationService smsPaymentVerificationService,
                    IHubContext<PaymentHub> hub
            )
        {
            _ghn = ghn;
            _dbContext = dbContext;
            //_qrCodeService = qrCodeService;
            _smsPaymentVerificationService = smsPaymentVerificationService;

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

        [HttpPost("~/Payment/PaymentConfirmWallet"), IgnoreAntiforgeryToken]
        public async Task<IActionResult> PaymentConfirmWalletAsync([FromBody] PaymentVerificationEvent evt)
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

        [HttpGet("~/Payment/PaymentSuccess")] // Goi trang thanh toan thanh cong
        public async Task<IActionResult> PaymentSuccess([FromQuery] string orderID, [FromQuery] string transactionCode)
        {
            if (string.IsNullOrEmpty(orderID) || string.IsNullOrEmpty(transactionCode))
            {
                return BadRequest();
            }

            var order = await _dbContext.Orders
                .Include(p => p.OrderDetail).ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(p => p.ID == orderID && p.TransactionId == transactionCode && p.Status == "Paid");

            var userID = User.Identity.IsAuthenticated ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0") : 0;
            if (userID > 0)
            {
                var Address = await _dbContext.Addresses.FirstOrDefaultAsync(p => p.UserID == userID && p.IsDefault);

                order.AddressID = Address.ID;
                order.ShipRecipientName = Address.RecipientName;
                order.ShipPhoneNumber = Address.PhoneNumber;
                order.ShipStreet = Address.Street;
                order.ShipProvince = Address.Province;
                order.ShipWard = Address.Ward;

                _dbContext.Orders.Update(order);
                await _dbContext.SaveChangesAsync();
            }

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

        public TimeSpan ExpiredTime(string time)
        {
            var dateCurrent = DateTime.UtcNow;
            var chooseTime = dateCurrent.Date.AddDays(1);
            if (time == "month")
            {
                chooseTime = dateCurrent.Date.AddMonths(1);
            }
            return chooseTime - dateCurrent;
        }
        #region CallWardAPIGHN
        public async Task<string> RequestGHN(Orders order)
        {

            var ghnRequest = new CreateOrderGHNRequest
            {
                ShopId = 2510403,
                ToName = $"{order.Users.LastName} {order.Users.FirstName}",
                ToPhone = order.Users.PhoneNumber,
                ToAddress = $"{order.Address.Street}, {order.Address.Ward}, {order.Address.Province}",
                Weight = order.OrderDetail.Sum(p => p.Product.Weight),
                ServiceID = 53320, // Mã dịch vụ giao hàng
                ServiceTypeID = 1, // 1: Giao hang tieu chuan, 2: giao hang nhanh
                ToDistrictID = 2152,
                ToWardCode = "381101",
                CodAmount = 0,
                PaymentTypeID = 1, // 1: nguoi ban tra phi van chuyen, 2: nguoi nhan tra phi
                RequiredNote = "KHONGCHOXEMHANG",
                ConfigFeeID = 1,
                ExstraCodeID = 0,

                FromName = "Nguyễn Thành Tuấn",
                FromPhone = "0358986823",
                FromAddressUser = "219/4 Linh Xuân, Phường Thủ Đức, Thành phố Hồ Chí Minh",
                FromDistrictId = 3695, // Thủ Đức (sau tái cấu trúc hành chính)
                FromWardCode = "90735", // Phường Linh Xuân

                Items = order.OrderDetail.Select(p => new ProductItem
                {
                    Name = p.Product.Name,
                    Code = p.Product.ID.ToString(),
                    Quantity = p.Quantity,
                    Price = (int)p.Product.NewPrice,
                    Length = 50,
                    Width = 200,
                    Height = 210,
                    Weight = p.Product.Weight
                }).ToList(),
            };

            return await _ghn.CreateShippingOrderAsync(order.ID, ghnRequest);
            //order.TrackingNumber = orderCode;
        }

        //public async Task<(string WardCode, int DistrictID)?> CallWardAPI(string wardName)
        //{
        //    // Chuẩn hóa input
        //    string input = RemoveDiacritics(wardName);

        //    // Setup request
        //    _httpClient.DefaultRequestHeaders.Clear();
        //    _httpClient.DefaultRequestHeaders.Add("Token", _token);

        //    // ⚡ Bạn có 2 cách:
        //    // (a) Nếu biết districtId => truyền vào payload { "district_id": xxx }
        //    // (b) Nếu không biết district => vòng lặp qua tất cả district (nặng hơn)

        //    // Ví dụ: chưa biết District => duyệt hết
        //    var allDistricts = await _httpClient.GetStringAsync("https://dev-online-gateway.ghn.vn/shiip/public-api/master-data/district");
        //    var districtResult = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(allDistricts);
        //    var districts = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(districtResult["data"].ToString());

        //    foreach (var district in districts)
        //    {
        //        var payload = JsonSerializer.Serialize(new { district_id = (int)district["DistrictID"] });
        //        var content = new StringContent(payload, Encoding.UTF8, "application/json");

        //        var response = await _httpClient.PostAsync("https://dev-online-gateway.ghn.vn/shiip/public-api/master-data/ward", content);
        //        response.EnsureSuccessStatusCode();

        //        var body = await response.Content.ReadAsStringAsync();
        //        var result = JsonSerializer.Deserialize<WardResponse>(body);

        //        if (result?.data == null) continue;

        //        // So khớp ward
        //        var matched = result.data.FirstOrDefault(w =>
        //            RemoveDiacritics(w.WardName).Equals(input, StringComparison.OrdinalIgnoreCase) ||
        //            w.NameExtension.Any(ext => RemoveDiacritics(ext).Equals(input, StringComparison.OrdinalIgnoreCase))
        //        );

        //        if (matched != null)
        //        {
        //            return (matched.WardCode, matched.DistrictID);
        //        }
        //    }

        //    return null; // không tìm thấy
        //}
        #endregion
    }
}

