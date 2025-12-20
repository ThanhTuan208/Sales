using Microsoft.AspNetCore.SignalR;

namespace CRUD_asp.netMVC.Hubs
{
    public class PaymentHub : Hub
    {   
        public async Task SendPaymentStatusAsync(string orderID, string transactionCode, bool isSuccess)
        {
            await Clients.All.SendAsync("ReceivePaymentStatus", orderID, transactionCode, isSuccess);
        }
    }
}
