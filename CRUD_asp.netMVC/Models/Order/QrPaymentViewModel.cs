namespace CRUD_asp.netMVC.Models.Order
{
    public class QrPaymentViewModel
    {
        public string OrderId { get; set; }
        public double Amount { get; set; }
        public string QrCodeUrl { get; set; }
        public string BankAccount { get; set; }
        public string PollingUrl { get; set; }
    }
}
