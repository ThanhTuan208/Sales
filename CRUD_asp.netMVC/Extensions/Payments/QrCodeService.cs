namespace CRUD_asp.netMVC.Extensions.Payments
{
    public class QrCodeService
    {
        // Su dung vietQR 
        public string GenerateBankQrCode(string accountNo, decimal? amount, string description, string accountName)
        {
            var bankBin = "970422"; // Mã bin MB Bank

            return $"https://img.vietqr.io/image/{bankBin}-{accountNo}-compact.png" +
                   $"?amount={amount}&addInfo={Uri.EscapeDataString(description)}&accountName={Uri.EscapeDataString(accountName)}";
        }
    }
}
