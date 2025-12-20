using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.DTO.Payments;
using CRUD_asp.netMVC.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace CRUD_asp.netMVC.EventHandlers.Payments
{
    public class NotifyPaymentHandler : IEventHandler<OrderPaidEvent>
    {
        private readonly IHubContext<PaymentHub>? _hub;

        public NotifyPaymentHandler(IHubContext<PaymentHub>? hub) => _hub = hub;

        public async Task HandleAsync(OrderPaidEvent evt)
        {
            await _hub.Clients.All.SendAsync("ReceivePaymentStatus", evt.OrderId, evt.TransactionId, evt.IsSuccess);
        }
    }
}
