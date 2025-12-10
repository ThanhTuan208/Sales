using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MimeKit;
using StackExchange.Redis;

namespace CRUD_asp.netMVC.Hubs
{
    [Authorize(Roles = "Admin")]
    public class DashboardHub : Hub
    {
        private readonly IConnectionMultiplexer _redis;

        public DashboardHub(IConnectionMultiplexer redis) => _redis = redis;

        public override async Task OnConnectedAsync()
        {
            var status = await GetCurrentStatus();
            await Clients.Caller.SendAsync("ReceiveCurrentStatus", status);
        }  

        private async Task<object> GetCurrentStatus()
        {
            var db = _redis.GetDatabase();
            var today = DateTime.UtcNow.ToString("yyyyMMdd");

            var totalVisits = await db.StringGetAsync("uv:total:" + today);
            var totalCount = await db.SetLengthAsync("dau:" + today);

            return new
            {
                UniqueVisitors = !totalVisits.IsNullOrEmpty ? (long)totalVisits : 0,
                DailyActiveUsers = totalCount,
                Date = DateTime.UtcNow.AddHours(7).ToString("dd/MM/yyyy") // Gio VietNam
            };

        }
    }
}
