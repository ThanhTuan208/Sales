namespace CRUD_asp.netMVC.DTO.Order
{
    public class PaymentVerificationByOrderDTO
    {
        public string Id { get; set; } = null!;
        public int UserId { get; set; }
        public decimal? Amount { get; set; }
        public decimal MissingAmount { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string TransactionId { get; set; } = null!;
    }
}
