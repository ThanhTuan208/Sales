using Azure.Core;
using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.DTO.Payment;
using CRUD_asp.netMVC.DTO.Payments;
using CRUD_asp.netMVC.EventHandlers;
using CRUD_asp.netMVC.Hubs;
using CRUD_asp.netMVC.Models.Order;
using CRUD_asp.netMVC.Service.Payments;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Transactions;
using PaymentModel = CRUD_asp.netMVC.Models.Product.Payment;

namespace CRUD_asp.netMVC.Service.Payments
{
    public class SmsPaymentVerificationService : ISmsPaymentVerificationService
    {
        private readonly IEventBus _event;
        private readonly AppDBContext _dbContext;

        public SmsPaymentVerificationService(AppDBContext dbContext, IEventBus @event)
        {
            _dbContext = dbContext;
            _event = @event;
        }

        public async Task<ResultDTO> ProcessResultAsync(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return ResultDTO.Fail("Tin nhắn không được null hoặc rỗng!");
            }

            var transactionCode = Regex.Match(message, @"ORD([^-]+)");
            var amountPayment = Regex.Match(message, @"GD\s*\+([\d,]+)VND");

            if (!transactionCode.Success)
            {
                return ResultDTO.Fail("Mã đơn hàng được gửi đến không hợp lệ (giữ mã giao dịch ORD...)!");
            }

            using var transactions = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var transactionId = transactionCode.Groups[1].Value;

                if (decimal.TryParse(amountPayment.Groups[1].Value, out decimal amountRecive))
                {
                    if (amountRecive == 0)
                    {
                        return ResultDTO.Fail("Thành toán đơn hàng thất bại!");
                    }
                }

                var order = await _dbContext.Orders
                                        .AsNoTracking()
                                        .Include(p => p.Address)
                                        .Include(p => p.Users)
                                        .Include(p => p.OrderDetail).ThenInclude(p => p.Product)
                                        .FirstOrDefaultAsync(p => p.TransactionId == transactionId);

                if (order == null)
                {
                    return ResultDTO.Fail("Không tìm thấy đơn hàng!");
                }

                if (order.Status == "Paid")
                {
                    return ResultDTO.OK("Đơn hàng đã được thanh toán trước đó!");
                }

                if (order.Amount != amountRecive)
                {

                    return ResultDTO.Fail("Số tiền thanh toán không đúng giá trị đơn hàng!");
                }

                _dbContext.Payment.Add(new PaymentModel()
                {
                    OrderID = order.ID,
                    paidAmount = order.Amount,
                    PaymentDate = DateTime.Now,
                    paymentMethod = order.PaymentMethod
                });

                foreach (var item in order.OrderDetail)
                {
                    var affectRow = await _dbContext.Products
                   .Where(p => p.ID == item.ProductID && p.Quantity >= item.Quantity)
                   .ExecuteUpdateAsync(s => s.SetProperty(p => p.Quantity, p => p.Quantity - item.Quantity));

                    if (affectRow > 0)
                    {
                        await _dbContext.Carts
                        .Where(p => p.UserID == order.UserID && p.ProductID == item.ProductID && p.IsDelete)
                        .ExecuteDeleteAsync();
                    }
                };

                _dbContext.Attach(order);
                order.Status = "Paid";
                order.PaidAt = DateTime.UtcNow.AddHours(7);
                _dbContext.Orders.Update(order);

                await _dbContext.SaveChangesAsync();
                await transactions.CommitAsync();

                await _event.PublishAsync(new OrderPaidEvent(order.ID, order.TransactionId, true));

                return ResultDTO.OK("Thanh toán thành công.");
            }
            catch (Exception ex)
            {
                await transactions.RollbackAsync();
                return ResultDTO.Fail(ex.Message);
            }
        }

        //public async Task<bool> CheckPaymentAsync(string orderId)
        //{
        //    try
        //    {
        //        // Lấy thông tin đơn hàng
        //        var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.ID == orderId);
        //        if (order == null || order.Status == "Paid") return order?.Status == "Paid";

        //        // Đọc SMS notifications từ file (hoặc SMS API nếu có)
        //        var smsMessages = await ReadSmsNotificationsAsync();

        //        // Tìm SMS khớp với transactionId
        //        var matchingSms = smsMessages.FirstOrDefault(sms =>
        //            sms.Content.Contains($"Don hang {orderId}") || sms.Content.Contains(orderId));

        //        if (matchingSms != null)
        //        {
        //            // Parse số tiền từ SMS
        //            var amount = ParseAmountFromSms(matchingSms.Content);
        //            if (amount >= order.Amount)
        //            {
        //                // Cập nhật trạng thái đơn hàng
        //                order.Status = "Paid";
        //                order.PaidAt = DateTime.Now;
        //                order.TransactionId = matchingSms.TransactionId;
        //                await _dbContext.SaveChangesAsync();

        //                _logger.LogInformation("Payment confirmed for order {OrderId}", orderId);
        //                return true;
        //            }
        //        }

        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error checking payment for order {OrderId}", orderId);
        //        return false;
        //    }
        //}

        private decimal ParseAmountFromSms(string smsContent)
        {
            // Regex để tìm số tiền, ví dụ: "1,234,567 VND"
            var amountMatch = Regex.Match(smsContent, @"(\d{1,3}(?:,\d{3})*(?:\.\d{2})?)");
            if (amountMatch.Success)
            {
                var amountStr = amountMatch.Value.Replace(",", "").Replace(".", "");
                return decimal.Parse(amountStr);
            }
            return 0;
        }

        //private async Task<List<SmsNotificationDTO>> ReadSmsNotificationsAsync()
        //{
        //    // Giả sử SMS được lưu trong file json (thay thế bằng SMS API nếu có)
        //    var smsFilePath = _configuration["SmsNotificationFilePath"];
        //    if (!File.Exists(smsFilePath))
        //    {
        //        return new List<SmsNotificationDTO>();
        //    }

        //    var json = await File.ReadAllTextAsync(smsFilePath);
        //    return JsonSerializer.Deserialize<List<SmsNotificationDTO>>(json) ?? new List<SmsNotificationDTO>();
        //}
    }
}
