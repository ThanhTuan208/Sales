using CRUD_asp.netMVC.Models.Order;

namespace CRUD_asp.netMVC.DTO.Payments
{
    public class PaymentVerificationEvent
    {
        public Orders? Order { get; set; }
        public decimal? AmountRecive { get; set; }

        public PaymentVerificationEvent(Orders? order, decimal? amountRecive)
        {
            Order = order;
            AmountRecive = amountRecive;
        }
    }
}
