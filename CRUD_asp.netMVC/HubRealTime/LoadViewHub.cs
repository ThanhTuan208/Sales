using Microsoft.AspNetCore.SignalR;
using NuGet.Protocol;

namespace CRUD_asp.netMVC.HubRealTime
{
    public class LoadViewHub : Hub
    {
        public async Task LoadChangeEmail()
        {
            await Clients.All.SendAsync("ChangeEmailProfile");
        }

        public async Task LazyLoad()
        {
            await Clients.All.SendAsync("LazyLoad");
        }
    }
}
