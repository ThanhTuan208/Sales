namespace CRUD_asp.netMVC.DTO.Payments
{
    public class OrderPaidEvent
    {
        public OrderPaidEvent(string orderId, string transactionId)
        {
            OrderId = orderId;
            TransactionId = transactionId;
        }

        public string OrderId { get; set; }
        public string TransactionId { get; set; }
    }
}
