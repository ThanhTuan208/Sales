using Microsoft.AspNetCore.SignalR;

namespace CRUD_asp.netMVC.Hubs
{
    public class SurplusMoneyHub : Hub
    {
        public async Task ReceiveSurplusMoneyAsync(decimal newSurplusMoney)
        {
            await Clients.All.SendAsync("ReceiveSurplusMoneyUpdate", newSurplusMoney);
        }
    }
}
