using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.SignalR;

namespace CRUD_asp.netMVC.Hubs
{
    public class QuestionResHub : Hub
    {
        public async Task QuestionResUserAsync(string userId, string orderId, decimal? missingAmount)
        {
            await Clients.User(userId).SendAsync("QuestionResquestUser", new
            {
                OrderId = orderId,
                MissingAmount = missingAmount
            });
        }
    }
}
