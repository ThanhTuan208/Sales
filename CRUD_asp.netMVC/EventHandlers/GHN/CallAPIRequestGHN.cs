using CRUD_asp.netMVC.DTO.Order.GHN;
using CRUD_asp.netMVC.Hubs;
using CRUD_asp.netMVC.Models.Order;
using Microsoft.AspNetCore.SignalR;
using StackExchange.Redis;

namespace CRUD_asp.netMVC.EventHandlers.GHN
{
    public class CallAPIRequestGHN : IEventHandler<Orders>
    {
        private readonly IHubContext<RequestGHNHub> _hub;

        public CallAPIRequestGHN(IHubContext<RequestGHNHub> hub) => _hub = hub;

        public Task HandleAsync(Orders @event)
        {
            throw new NotImplementedException();
        }
    }
}
