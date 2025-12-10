using CRUD_asp.netMVC.Hubs;
using Microsoft.AspNetCore.SignalR;
using StackExchange.Redis;
using System.Timers;

namespace CRUD_asp.netMVC.Service.Users
{
    public class RedisVisitListenerService : BackgroundService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IHubContext<DashboardHub> _hub;
        private readonly ILogger<RedisVisitListenerService> _logger;

        private const string ChannelName = "site:updates";

        public RedisVisitListenerService(
                IConnectionMultiplexer redis, 
                IHubContext<DashboardHub> hub, 
                ILogger<RedisVisitListenerService> logger)
        {
            _hub = hub;
            _redis = redis;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var subcriber = _redis.GetSubscriber();
            return subcriber.SubscribeAsync(ChannelName, async (channel, dateString) =>
            {
                _ = Task.Run(async () =>
                {
                    try
                    {
                        var db = _redis.GetDatabase();
                        string? today = dateString.HasValue ? (string?)dateString : DateTime.UtcNow.ToString("yyyyMMdd");

                        var totalVisitors = await db.StringGetAsync($"uv:total:{today}");
                        var totalCounts = await db.SetLengthAsync($"hll:dau:{today}");

                        var starts = new
                        {
                            TotalVisits = totalVisitors.IsNullOrEmpty ? 0 : (long)totalVisitors,
                            DailyActiveUsers = totalCounts,
                            Date = DateTime.UtcNow.AddHours(7).ToString("dd/MM/yyyy HH:mm:ss")
                        };

                        await _hub.Clients.All.SendAsync("ReceiveCurrentStatus", starts, stoppingToken);

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Lỗi chạy real-time RedisVisitListenerService Class", ex.Message, ex.InnerException?.Message ?? string.Empty);
                    }

                }, stoppingToken);
            });
        }
    }
}
