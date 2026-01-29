namespace CRUD_asp.netMVC.DTO.Payments
{
    public class CallAPIRequestGHNEvent
    {
        public CallAPIRequestGHNEvent(string orderId, string userId)
        {
            OrderId = orderId;
            UserId = userId;
        }

        public string OrderId { get; set; } = null!;
        public string UserId { get; set; } = null!;

    }
}
