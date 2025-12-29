using Microsoft.AspNetCore.SignalR;

namespace CRUD_asp.netMVC.Hubs
{
    public class PaymentHub : Hub
    {   
        public async Task SendPaymentStatusAsync(string orderID, string userId, string transactionCode, bool isSuccess)
        {
            await Clients.User(userId).SendAsync("ReceivePaymentStatus", orderID, transactionCode, isSuccess);
        }
    }   
}
