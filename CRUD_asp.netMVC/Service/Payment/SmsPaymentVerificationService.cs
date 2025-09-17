using CRUD_asp.netMVC.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using NuGet.Protocol;
using System.Data;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace CRUD_asp.netMVC.Service.Payment
{
    public interface ISmsPaymentVerificationService
    {
        Task<bool> CheckPaymentAsync(string orderId);
    }
    public class SmsPaymentVerificationService : ISmsPaymentVerificationService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<SmsPaymentVerificationService> _logger;
        private readonly AppDBContext _dbContext;

        public SmsPaymentVerificationService(IConfiguration configuration, ILogger<SmsPaymentVerificationService> logger, AppDBContext dbContext)
        {
            _configuration = configuration;
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<bool> CheckPaymentAsync(string orderId)
        {
            try
            {
                // Lấy thông tin đơn hàng
                var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.ID == orderId);
                if (order == null || order.Status == "Paid") return order?.Status == "Paid";

                // Đọc SMS notifications từ file (hoặc SMS API nếu có)
                var smsMessages = await ReadSmsNotificationsAsync();

                // Tìm SMS khớp với orderId
                var matchingSms = smsMessages.FirstOrDefault(sms =>
                    sms.Content.Contains($"Don hang {orderId}") || sms.Content.Contains(orderId));

                if (matchingSms != null)
                {
                    // Parse số tiền từ SMS
                    var amount = ParseAmountFromSms(matchingSms.Content);
                    if (amount >= order.Amount)
                    {
                        // Cập nhật trạng thái đơn hàng
                        order.Status = "Paid";
                        order.PaidAt = DateTime.Now;
                        order.TransactionId = matchingSms.TransactionId;
                        await _dbContext.SaveChangesAsync();

                        _logger.LogInformation("Payment confirmed for order {OrderId}", orderId);
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking payment for order {OrderId}", orderId);
                return false;
            }
        }

        private double ParseAmountFromSms(string smsContent)
        {
            // Regex để tìm số tiền, ví dụ: "1,234,567 VND"
            var amountMatch = Regex.Match(smsContent, @"(\d{1,3}(?:,\d{3})*(?:\.\d{2})?)");
            if (amountMatch.Success)
            {
                var amountStr = amountMatch.Value.Replace(",", "").Replace(".", "");
                return double.Parse(amountStr);
            }
            return 0;
        }

        private async Task<List<SmsNotification>> ReadSmsNotificationsAsync()
        {
            // Giả sử SMS được lưu trong file json (thay thế bằng SMS API nếu có)
            var smsFilePath = _configuration["SmsNotificationFilePath"];
            if (!File.Exists(smsFilePath))
            {
                return new List<SmsNotification>();
            }

            var json = await File.ReadAllTextAsync(smsFilePath);
            return JsonSerializer.Deserialize<List<SmsNotification>>(json) ?? new List<SmsNotification>();
        }
    }
}
