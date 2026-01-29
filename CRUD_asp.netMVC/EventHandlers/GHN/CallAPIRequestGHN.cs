using CRUD_asp.netMVC.DTO.Order.GHN;
using CRUD_asp.netMVC.DTO.Payments;
using CRUD_asp.netMVC.Hubs;
using CRUD_asp.netMVC.Models.Order;
using Microsoft.AspNetCore.SignalR;
using StackExchange.Redis;

namespace CRUD_asp.netMVC.EventHandlers.GHN
{
    public class CallAPIRequestGHN : IEventHandler<CallAPIRequestGHNEvent>
    {
        private readonly IHubContext<RequestGHNHub> _hub;

        public CallAPIRequestGHN(IHubContext<RequestGHNHub> hub) => _hub = hub;

        public async Task HandleAsync(CallAPIRequestGHNEvent evt)
        {
            try
            {
                await _hub.Clients.User(evt.UserId).SendAsync("RequestGHN", evt.OrderId);
            }
            catch (IOException ex) when (ex.Message.Contains("client reset")) { return; }
        }
    }
}
