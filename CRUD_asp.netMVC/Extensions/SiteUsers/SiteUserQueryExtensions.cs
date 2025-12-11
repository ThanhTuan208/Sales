using StackExchange.Redis;
using CRUD_asp.netMVC.Models.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SiteUser = CRUD_asp.netMVC.Models.Auth.SiteUsers;
using Redis = StackExchange.Redis.IDatabase;

namespace CRUD_asp.netMVC.Extensions.SiteUsers
{
    public static class SiteUserQueryExtensions
    {
        private const string DAU_KEY_PREFIX = "hll:dau:";
        private const string TOTAL_KEY_PREFIX = "uv:total:";
        public static async Task<long> DAUTodayAsync(this DbSet<SiteUser> siteUsers, Redis? db)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow).ToString("yyyyMMdd");
            var dauKey = $"{DAU_KEY_PREFIX + today}";

            var DAUDay = await db.HyperLogLogLengthAsync(dauKey);
            return DAUDay;
        }
        public static async Task<long> DAUYesterdayAsync(this DbSet<SiteUser> siteUsers, Redis? db)
        {
            var yesterday = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)).ToString("yyyyMMdd");
            var dauKey = $"{DAU_KEY_PREFIX + yesterday}";

            var DAUYesterday = await db.HyperLogLogLengthAsync(dauKey);
            return DAUYesterday;
        }

        public static async Task<long> UVTodayAsync(this DbSet<SiteUser> siteUsers, Redis? db)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow).ToString("yyyyMMdd");
            var totalKey = $"{TOTAL_KEY_PREFIX + today}";

            var UVDay = await db.StringGetAsync(totalKey);
            return UVDay.HasValue ? (long)UVDay : 0;
        }
        public static async Task<long> UVYesterdayAsync(this DbSet<SiteUser> siteUsers, Redis? db)
        {
            var yesterday = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)).ToString("yyyyMMdd");
            var totalKey = $"{TOTAL_KEY_PREFIX + yesterday}";

            var UVyesterday = await db.StringGetAsync(totalKey);
            return UVyesterday.HasValue ? (long)UVyesterday : 0;
        }

        // Tinh ty le phan tram thay doi nguoi dung hom qua, hom nay
        public static decimal CalChangePercentByDay(decimal today, decimal yesterday)
        {
            if (yesterday == 0) return today == 0 ? 0m : 100m;
            return ((today - yesterday) / yesterday) * 100m;
        }

        #region Lay database SQLServer (Old Code)
        //public static async Task<long> DAUTodayAsync(this DbSet<SiteUser> siteUsers)
        //{
        //    var yesterday = DateOnly.FromDateTime(DateTime.UtcNow);
        //    var DAUYesterday = await siteUsers.FirstOrDefaultAsync(p => EF.Property<DateOnly>(p, "Day") == yesterday);
        //    return DAUYesterday != null ? DAUYesterday.DailyActiveUsers : 0;
        //}   
        //public static async Task<long> DAUYesterdayAsync(this DbSet<SiteUser> siteUsers)
        //{
        //    var yesterday = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1));
        //    var DAUYesterday = await siteUsers.FirstOrDefaultAsync(p => EF.Property<DateOnly>(p, "Day") == yesterday);
        //    return DAUYesterday != null ? DAUYesterday.DailyActiveUsers : 0;
        //}   

        //public static async Task<long> UVTodayAsync(this DbSet<SiteUser> siteUsers, Redis? db)
        //{
        //    var yesterday = DateOnly.FromDateTime(DateTime.UtcNow);
        //    var DAUYesterday = await siteUsers.FirstOrDefaultAsync(p => EF.Property<DateOnly>(p, "Day") == yesterday);
        //    return DAUYesterday != null ? DAUYesterday.UniqueVisitors : 0;
        //}
        //public static async Task<long> UVYesterdayAsync(this DbSet<SiteUser> siteUsers, Redis? db)
        //{
        //    var yesterday = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1));
        //    var DAUYesterday = await siteUsers.FirstOrDefaultAsync(p => EF.Property<DateOnly>(p, "Day") == yesterday);
        //    return DAUYesterday != null ? DAUYesterday.UniqueVisitors : 0;
        //}
        #endregion 
    }
}
