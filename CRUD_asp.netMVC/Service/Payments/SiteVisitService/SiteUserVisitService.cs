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

            var dauKey = $"hll:dau:{yesterday}";
            var totalKey = $"uv:total:{yesterday}";

            var totalVisits = await db.StringGetAsync(totalKey);
            var totalCount = await db.SetLengthAsync(dauKey);

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
                    CreatedAt = DateTime.UtcNow,
                });

                await _dbContext.SaveChangesAsync();
            }

            // Xóa key cũ để tiết kiệm RAM
            await db.KeyDeleteAsync(dauKey);
            await db.KeyDeleteAsync(totalKey);
        }
    }
}
