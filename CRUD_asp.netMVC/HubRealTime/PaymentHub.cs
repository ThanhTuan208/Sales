using Microsoft.AspNetCore.SignalR;

namespace CRUD_asp.netMVC.HubRealTime
{
    public class PaymentHub : Hub
    {
        public async Task SendPaymentStatus(string orderID, string transactionCode)
        {
            await Clients.All.SendAsync("ReceivePaymentStatus", orderID, transactionCode);
        }
    }
}
