using CRUD_asp.netMVC.DTO.Admin;
using CRUD_asp.netMVC.Extensions.SiteUsers;
using CRUD_asp.netMVC.Hubs;
using Microsoft.AspNetCore.SignalR;
using StackExchange.Redis;

namespace CRUD_asp.netMVC.Service.Users
{
    public class RedisVisitListenerService : BackgroundService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IHubContext<DashboardHub> _hub;
        private readonly ILogger<RedisVisitListenerService> _logger;

        private const string DAU_KEY_PREFIX = "hll:dau:";
        private const string TOTAL_KEY_PREFIX = "uv:total:";
        private const string ChannelName = "site:updates";

        public RedisVisitListenerService(
            IConnectionMultiplexer redis,
            IHubContext<DashboardHub> hub,
            ILogger<RedisVisitListenerService> logger)
        {
            _redis = redis;
            _hub = hub;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogWarning("RedisVisitListenerService BẮT ĐẦU – sẵn sàng nhận publish");

            while (!stoppingToken.IsCancellationRequested)
            {
                ISubscriber? subscriber = null;
                try
                {
                    // 1. Luôn lấy subscriber MỚI mỗi lần reconnect
                    subscriber = _redis.GetSubscriber();

                    // 2. Subscribe với handler
                    subscriber.Subscribe(ChannelName, async (channel, dateString) =>
                    {
                        _logger.LogInformation($"NHẬN ĐƯỢC PUBLISH: {dateString}");

                        _ = Task.Run(async () =>
                        {
                            try
                            {
                                var db = _redis.GetDatabase();
                                string? today = dateString.HasValue ? (string?)dateString : DateTime.UtcNow.ToString("yyyyMMdd");
                                string? yesterday = !dateString.HasValue ? (string?)dateString : DateTime.UtcNow.AddDays(-1).ToString("yyyyMMdd");

                                var totalToday = await db.StringGetAsync($"{TOTAL_KEY_PREFIX + today}");
                                var totalYesterday = await db.StringGetAsync($"{TOTAL_KEY_PREFIX + yesterday}");

                                long dailyActiveUsersToday = await db.HyperLogLogLengthAsync($"{DAU_KEY_PREFIX + today}");
                                long dailyActiveUsersYesterday = await db.HyperLogLogLengthAsync($"{DAU_KEY_PREFIX + yesterday}");

                                long totalVisitorToday = totalToday.IsNullOrEmpty ? 0L : (long)totalToday;
                                long totalVisitorYesterday = totalYesterday.IsNullOrEmpty ? 0L : (long)totalYesterday;

                                var uvPercents = SiteUserQueryExtensions.CalChangePercentByDay(totalVisitorToday, totalVisitorYesterday);
                                var dauPercents = SiteUserQueryExtensions.CalChangePercentByDay(dailyActiveUsersToday, dailyActiveUsersYesterday);

                                var stats = new
                                {
                                    TotalVisits = totalVisitorToday,
                                    DailyActiveUsers = dailyActiveUsersToday,
                                    UVPercents = SiteUserQueryExtensions.InputPercents(uvPercents),
                                    DAUPercents = SiteUserQueryExtensions.InputPercents(dauPercents),
                                    Date = DateTime.UtcNow.AddHours(7).ToString("dd/MM/yyyy HH:mm:ss") // VN
                                };

                                await _hub.Clients.All.SendAsync("ReceiveCurrentStatus", stats);
                                _logger.LogInformation("ĐÃ GỬI REALTIME: Visits={V}, DAU={D}", stats.TotalVisits, stats.DailyActiveUsers);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Lỗi xử lý realtime trong callback");
                            }
                        }, stoppingToken);
                    });

                    _logger.LogWarning($"SUBSCRIBE THÀNH CÔNG kênh '{ChannelName}' – đang lắng nghe...");

                    // 3. Giữ sống cho đến khi Redis chết HOẶC app shutdown
                    while (_redis.IsConnected && !stoppingToken.IsCancellationRequested)
                    {
                        await Task.Delay(1000, stoppingToken);
                    }

                    _logger.LogWarning("Redis mất kết nối hoặc app shutdown → thoát vòng lặp để reconnect...");
                }
                catch (Exception ex) when (!(ex is TaskCanceledException))
                {
                    _logger.LogError(ex, "Lỗi nghiêm trọng khi subscribe Redis");
                }
                finally
                {
                    // 4. Dọn dẹp sạch sẽ trước khi reconnect
                    if (subscriber != null)
                    {
                        try { await subscriber.UnsubscribeAsync(ChannelName); }
                        catch (Exception ex) { _logger.LogError(ex, "Lỗi unsubscribe"); }
                    }
                }

                // 5. Nếu app đang shutdown → thoát luôn
                if (stoppingToken.IsCancellationRequested)
                {
                    _logger.LogWarning("App đang shutdown → dừng RedisVisitListenerService");
                    break;
                }

                _logger.LogWarning("Đang thử kết nối lại Redis sau 5 giây...");
                await Task.Delay(5000, stoppingToken);
            }

            _logger.LogWarning("RedisVisitListenerService ĐÃ DỪNG HOÀN TOÀN");
        }
    }

}
