using CRUD_asp.netMVC.Extensions.SiteUsers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MimeKit;
using StackExchange.Redis;

namespace CRUD_asp.netMVC.Hubs
{
    [AllowAnonymous]
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
            var yesterday = DateTime.UtcNow.AddDays(-1).ToString("yyyyMMdd");

            var uvRedisValTodays = await db.StringGetAsync("uv:total:" + today);
            var uvRedisValYesterdays = await db.StringGetAsync("uv:total:" + yesterday);

            var totalCountTodays = await db.HyperLogLogLengthAsync("hll:dau:" + today);
            var totalCountYesterdays = await db.HyperLogLogLengthAsync("hll:dau:" + yesterday);

            long totalVisitTodays = uvRedisValTodays.HasValue ? (long)uvRedisValTodays : 0;
            long totalVisitYesterdays = uvRedisValYesterdays.HasValue ? (long)uvRedisValYesterdays : 0;

            var uvPercents = SiteUserQueryExtensions.CalChangePercentByDay(totalVisitTodays, totalVisitYesterdays);
            var dauPercents = SiteUserQueryExtensions.CalChangePercentByDay(totalCountTodays, totalCountYesterdays);

            return new
            {
                TotalVisits = totalVisitTodays,
                DailyActiveUsers = totalCountTodays,
                UVPercents = SiteUserQueryExtensions.InputPercents(uvPercents),
                DAUPercents= SiteUserQueryExtensions.InputPercents(dauPercents),
                Date = DateTime.UtcNow.AddHours(7).ToString("dd/MM/yyyy") // Gio VietNam
            };
        }
    }
}
