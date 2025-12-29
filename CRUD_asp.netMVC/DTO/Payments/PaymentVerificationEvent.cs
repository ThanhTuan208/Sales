using CRUD_asp.netMVC.DTO.Order;
using CRUD_asp.netMVC.Models.Order;

namespace CRUD_asp.netMVC.DTO.Payments
{
    public class PaymentVerificationEvent
    {
        public decimal? AmountReceive { get; set; }
        public PaymentVerificationByOrderDTO Order { get; set; } = null!;

        public PaymentVerificationEvent() { }

        public PaymentVerificationEvent(PaymentVerificationByOrderDTO order, decimal? amountRecive)
        {
            Order = order;
            AmountReceive = amountRecive;
        }
    }
}
