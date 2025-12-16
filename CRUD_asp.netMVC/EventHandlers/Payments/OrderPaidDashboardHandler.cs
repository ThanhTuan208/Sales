using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.DTO.Payments;
using CRUD_asp.netMVC.Extensions.SiteUsers;
using CRUD_asp.netMVC.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.VisualBasic;
using Org.BouncyCastle.Asn1;
using StackExchange.Redis;

namespace CRUD_asp.netMVC.EventHandlers.Payments
{
    public class OrderPaidDashboardHandler : IEventHandler<OrderPaidEvent>
    {
        private readonly AppDBContext _dbContext;
        private readonly IConnectionMultiplexer? _redis;
        private readonly IHubContext<DashboardHub>? _hub;

        private const string AMOUNT_TODAY_PREFIX = "amt:today:";
        private const string AMOUNT_MONTH_PREFIX = "amt:month:";

        public OrderPaidDashboardHandler
            (
                IConnectionMultiplexer? redis,
                IHubContext<DashboardHub>? hub,
                AppDBContext dbContext
            )
        {
            _redis = redis;
            _hub = hub;
            _dbContext = dbContext;
        }

        public async Task HandleAsync(OrderPaidEvent evt)
        {
            var db = _redis.GetDatabase();

            var order = _dbContext.Orders.FirstOrDefault(o => o.ID == evt.OrderId);

            if (order == null) return;

            var orderPaidAt = order.PaidAt ?? DateTime.UtcNow;

            var today = orderPaidAt.ToString("yyyyMMdd");
            var yesterday = orderPaidAt.AddDays(-1).ToString("yyyyMMdd");

            var month = orderPaidAt.ToString("yyyyMM");
            var lastMonth = orderPaidAt.AddMonths(-1).ToString("yyyyMM");

            var amtTodayKey = $"{AMOUNT_TODAY_PREFIX + today}";
            var amtYesterdayKey = $"{AMOUNT_TODAY_PREFIX + yesterday}";

            var amtMonthKey = $"{AMOUNT_MONTH_PREFIX + month}";
            var amtLastMonthKey = $"{AMOUNT_MONTH_PREFIX + lastMonth}";

            var presentdayAmounts = await db.StringIncrementAsync(amtTodayKey, (long)order.Amount);
            var yesterdayAmounts = await db.StringGetAsync(amtYesterdayKey);
            long yesterdayValue = yesterdayAmounts.HasValue ? (long)yesterdayAmounts : 0;

            var presentMonthAmounts = await db.StringIncrementAsync(amtMonthKey, (long)order.Amount);
            var lastMonthAmounts = await db.StringGetAsync(amtLastMonthKey);
            long lastMonthValue = lastMonthAmounts.HasValue ? (long)lastMonthAmounts : 0;

            var amtTodayPercents = SiteUserQueryExtensions.CalChangePercentByDay(presentdayAmounts, yesterdayValue);
            var amtMonthPercents = SiteUserQueryExtensions.CalChangePercentByDay(presentMonthAmounts, lastMonthValue);

            var tasks = new
            {
                TodayAmount = presentdayAmounts,
                MonthAmount = presentMonthAmounts,
                TAPercents = SiteUserQueryExtensions.InputPercents(amtTodayPercents),
                MAPercents = SiteUserQueryExtensions.InputPercents(amtMonthPercents) 
            };

            // Het han sau 1 ngay/thang
            await db.KeyExpireAsync(amtTodayKey, ExpiredTime("today"));
            await db.KeyExpireAsync(amtMonthKey, ExpiredTime("month"));


            await _hub.Clients.All.SendAsync("ReceiveCurrentStatus", tasks);
        }

        public TimeSpan ExpiredTime(string type)
        {
            var now = DateTime.UtcNow;

            DateTime expireAt = type switch
            {
                "today" => now.Date.AddDays(1),
                "month" => new DateTime(now.Year, now.Month, 1).AddMonths(1),
                _ => now.AddDays(1)
            };

            return expireAt - now;
        }
    }
}
