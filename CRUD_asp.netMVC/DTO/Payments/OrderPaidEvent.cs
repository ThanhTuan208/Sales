namespace CRUD_asp.netMVC.DTO.Payments
{
    public class OrderPaidEvent
    {
        public OrderPaidEvent(string orderId, string userId, string transactionId, bool isSuccess)
        {
            OrderId = orderId;
            TransactionId = transactionId;
            IsSuccess = isSuccess;
            UserId = userId;
        }

        public string OrderId { get; set; }
        public string UserId { get; set; }
        public string TransactionId { get; set; }
        public bool IsSuccess { get; set; }
    }
}
