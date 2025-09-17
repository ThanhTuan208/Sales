using System.Drawing.Imaging;

namespace CRUD_asp.netMVC.Service.Payment
{
    public class QrCodeService
    {
        public string GenerateBankQrCode(string accountNo, double amount, string description, string accountName)
        {
            // Su dung vietQR 
            var bankBin = "970422"; // Mã bin MB Bank

            string url = $"https://img.vietqr.io/image/{bankBin}-{accountNo}-compact.png" +
                         $"?amount={amount}&addInfo={Uri.EscapeDataString(description)}&accountName={Uri.EscapeDataString(accountName)}";
            return url;
        }

    }
}
