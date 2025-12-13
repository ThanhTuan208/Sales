using CRUD_asp.netMVC.Data;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using CRUD_asp.netMVC.Models.Auth;

namespace CRUD_asp.netMVC.Service.Payment.SiteVisitService
{
    public class SiteUserVisitService : ISiteUserVisitService
    {
        private readonly AppDBContext _dbContext;
        private readonly IConnectionMultiplexer _redis;

        private const string DAU_KEY_PREFIX = "hll:dau:";
        private const string TOTAL_KEY_PREFIX = "uv:total:";

        private const string AMOUNT_TODAY_PREFIX = "amt:today:"; 
        private const string AMOUNT_MONTH_PREFIX = "amt:month:";

        public SiteUserVisitService(AppDBContext dbContext, IConnectionMultiplexer redis)
        {
            _redis = redis;
            _dbContext = dbContext;
        }

        // Cap nhat so luong nguoi truy cap
        public async Task IncreaseSiteVisitAsync()
        {
            var db = _redis.GetDatabase();
            var yesterday = DateTime.UtcNow.AddDays(-1).ToString("yyyyMMdd");

            var dauKey = $"{DAU_KEY_PREFIX + yesterday}";
            var totalKey = $"{TOTAL_KEY_PREFIX + yesterday}";

            var totalVisits = await db.StringGetAsync(totalKey);
            var totalCount = await db.HyperLogLogLengthAsync(dauKey);

            long userVisitor = !totalVisits.IsNullOrEmpty ? (long)totalVisits : 0;
            long dailyActiveUsers = totalCount;

            bool exists = await _dbContext.SiteUser.AnyAsync(p => p.Date == DateTime.UtcNow.AddDays(-1).Date);

            if (!exists)
            {
                _dbContext.SiteUser.Add(new SiteUsers()
                {
                    Date = DateTime.UtcNow.AddDays(-1).Date,
                    DailyActiveUsers = dailyActiveUsers,
                    UniqueVisitors = userVisitor,
                    TodayAmounts = 0,
                    MonthAmounts = 0,
                    CreatedAt = DateTime.UtcNow,
                });

                await _dbContext.SaveChangesAsync();
            }

            //// Xóa key cũ để tiết kiệm RAM
            //await db.KeyDeleteAsync(dauKey);
            //await db.KeyDeleteAsync(totalKey);
        }
    }
}
