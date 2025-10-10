using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.DTO.Order.GHN;
using CRUD_asp.netMVC.DTO.Payment;
using CRUD_asp.netMVC.HubRealTime;
using CRUD_asp.netMVC.Models.Order;
using CRUD_asp.netMVC.Models.Product;
using CRUD_asp.netMVC.Service.GHN;
using CRUD_asp.netMVC.Service.Payment;
using CRUD_asp.netMVC.ViewModels.Order;
using EFCoreSecondLevelCacheInterceptor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Elfie.Model.Tree;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using System.Formats.Asn1;
using System.Globalization;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CRUD_asp.netMVC.Controllers
{
    public class PaymentController : Controller
    {

        private readonly GhnService _ghn;

        private readonly AppDBContext _dbContext;
        private readonly QrCodeService _qrCodeService;
        private readonly ISmsPaymentVerificationService _smsPaymentVerificationService;


        public PaymentController(QrCodeService qrCodeService, ISmsPaymentVerificationService smsPaymentVerificationService, AppDBContext dbContext, GhnService ghn, IConfiguration config)
        {
            _ghn = ghn;
            _qrCodeService = qrCodeService;
            _smsPaymentVerificationService = smsPaymentVerificationService;
            _dbContext = dbContext;
            //_token = config["GHN:Token"] ?? "";
            //_httpClient = httpClient;
            //_httpClient.DefaultRequestHeaders.Add("Token", _token);
            //_httpClient.DefaultRequestHeaders.Add("_dbContext-Type", "application/json");
        }

        // Thay doi name co cac ki tu co dau thanh khong dau
        public string RemoveDiacritics(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;

            var normalized = text.ToLowerInvariant().Normalize(NormalizationForm.FormD);
            var builder = new StringBuilder();

            foreach (char c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    builder.Append(c);
            }

            return builder.ToString().Replace(" ", "").Replace("đ", "d");
        }


        [HttpPost] // Nhan sms tu dien thoai
        public async Task<IActionResult> SmsReceive([FromBody] SmsMessage sms, [FromServices] IHubContext<PaymentHub> hub)
        {
            //Mở 1 giao dịch(transaction). -> thuc hien cho kieu toi uu sql "raw sql bulk update"
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var NDMessage = sms.Message.Split("ND:")[1].Trim();
                var transactionCode = NDMessage.Split('-')[1];

                if (!transactionCode.Contains("ORD")) return BadRequest();

                // lay ma giao dich
                var formartTransaction = transactionCode.Split("ORD")[1];

                var order = await _dbContext.Orders
                    .AsNoTracking()
                    .Include(p => p.Address)
                    .Include(p => p.Users)
                    .Include(p => p.OrderDetail).ThenInclude(p => p.Product)
                    .FirstOrDefaultAsync(p => p.TransactionId == formartTransaction);

                if (order == null) return NotFound();

                var payment = new Payment()
                {
                    OrderID = order.ID,
                    paidAmount = order.Amount,
                    PaymentDate = DateTime.Now,
                    paymentMethod = order.PaymentMethod
                };

                // Cap nhat lai trang thai sau khi chuyen khoan thanh cong
                order.Payment = payment; // add data payment
                order.Status = "Paid";
                order.PaidAt = DateTime.Now;

                var productIDList = order.OrderDetail.Where(p => p.OrderID == order.ID).Select(p => p.ProductID).ToList();

                var productQtyList = order.OrderDetail
                    .GroupBy(x => x.ProductID)
                    .Select(p => new { PorductID = p.Key, ToTalQty = p.Sum(s => s.Quantity) })
                    .ToList();

                StringBuilder build = new StringBuilder();
                foreach (var product in productQtyList)
                {
                    build.AppendLine($"WHEN {product.PorductID} THEN Quantity - {product.ToTalQty}");
                }

                var caseWhen = build.ToString();
                string productIDSelected = string.Join(", ", productQtyList.Select(p => p.PorductID));

                string sqlUpdateQty = @$"
                        UPDATE Products
                        SET Quantity = CASE ID
                            {caseWhen}
                            ELSE Quantity
                        END
                        WHERE ID IN ({productIDSelected});";

                var affectRow = await _dbContext.Database.ExecuteSqlRawAsync(sqlUpdateQty);

                if (affectRow > 0)
                {
                    await _dbContext.Carts
                        .Where(p => p.UserID == order.UserID && productIDList.Contains(p.ProductID) && p.IsDelete)
                        .ExecuteDeleteAsync();
                }

                _dbContext.Orders.Update(order);

                await _dbContext.SaveChangesAsync();

                await hub.Clients.All.SendAsync("ReceivePaymentStatus", order.ID, formartTransaction);

                // Tao van don GHN sau khi thanh toan
                //await RequestGHN(order);

                //Nếu không có lỗi → commit, lưu thay đổi.
                await transaction.CommitAsync();

                return Json(new { sucess = true, message = "Thanh toán thành công." });
            }
            catch (Exception ex)
            {
                //Nếu có lỗi ở bất kỳ bước nào → rollback, đảm bảo dữ liệu không bị nửa vời.
                await transaction.RollbackAsync();

                return Json(new { sucess = false, message = "Thanh toán thất bại: " + ex.Message });
            }
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

