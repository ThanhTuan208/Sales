namespace CRUD_asp.netMVC.Service.Payment
{
    public class SmsNotification
    {
        public string TransactionId { get; set; }
        public string Content { get; set; }
        public DateTime ReceivedAt { get; set; }
        public string Sender { get; set; }
    }
}
