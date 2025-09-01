namespace CRUD_asp.netMVC.Service.Payment
{
    public class QrCodeService
    {
        public string GenerateBankQrCode(string orderId, decimal amount, string bankAccount)
        {
            var bankInfo = new
            {
                BankName = "MBBank", // Thay bằng ngân hàng của bạn
                AccountNumber = bankAccount, // Ví dụ: "1234567890"
                AccountName = "Công ty TNHH một thành viên",
                Content = $"Don hang {orderId}",
                Amount = amount.ToString("N0")
            };

            var qrContent = $"Bank: {bankInfo.BankName}\n" +
                           $"Account: {bankInfo.AccountNumber}\n" +
                           $"Name: {bankInfo.AccountName}\n" +
                           $"Content: {bankInfo.Content}";

            // Sử dụng Google Chart API để tạo QR code
            var qrUrl = $"https://chart.googleapis.com/chart?chs=300x300&cht=qr&chl={Uri.EscapeDataString(qrContent)}&choe=UTF-8";
            return qrUrl;
        }
    }
}
