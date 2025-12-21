using CRUD_asp.netMVC.DTO.Admin;
using CRUD_asp.netMVC.Models.Auth;
using CRUD_asp.netMVC.Models.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Redis = StackExchange.Redis.IDatabase;

namespace CRUD_asp.netMVC.Extensions.Payments
{
    public static class PaymentQueryExtensions
    {
        private const string AMOUNT_TODAY_PREFIX = "amt:today:";
        private const string AMOUNT_MONTH_PREFIX = "amt:month:";

        public static async Task<decimal> ByToDayAsync(this DbSet<Payment> payment, Redis db)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow).ToString("yyyyMMdd");
            var todayAmountKey = $"{AMOUNT_TODAY_PREFIX + today}";

            var todayAmount = await db.StringGetAsync(todayAmountKey);
            return todayAmount.HasValue ? (long)todayAmount : 0;
        }

        public static async Task<decimal> ByToYesterdayAsync(this DbSet<Payment> payment, Redis db)
        {
            var yesterday = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)).ToString("yyyyMMdd");
            var yesterdayAmountKey = $"{AMOUNT_TODAY_PREFIX + yesterday}";

            var yesterdayAmount = await db.StringGetAsync(yesterdayAmountKey);
            return yesterdayAmount.HasValue ? (long)yesterdayAmount : 0;
        }

        public static async Task<decimal> ByToLastMonthAsync(this DbSet<Payment> payment, Redis db)
        {
            var lastMonth = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-1)).ToString("yyyyMM");
            var lastMonthAmountKey = $"{AMOUNT_MONTH_PREFIX + lastMonth}";

            var lastMonthAmount = await db.StringGetAsync(lastMonthAmountKey);
            return lastMonthAmount.HasValue ? (long)lastMonthAmount : 0;
        }

        public static async Task<decimal> ByMonthAsync(this DbSet<Payment> payment, Redis db)
        {
            var month = DateOnly.FromDateTime(DateTime.UtcNow).ToString("yyyyMM");
            var monthAmountKey = $"{AMOUNT_MONTH_PREFIX + month}";

            var monthAmount = await db.StringGetAsync(monthAmountKey);
            return monthAmount.HasValue ? (long)monthAmount : 0;
        }

        // Tinh ty le phan tram thay doi nguoi dung hom qua, hom nay
        public static decimal CalChangePercentByDay(decimal today, decimal yesterday)
        {
            if (yesterday == 0) return today == 0 ? 0m : 100m;
            return ((today - yesterday) / yesterday) * 100m;
        }

        // Tinh ty le phan tram thay doi nguoi dung hom truoc, thang hien tai
        public static decimal CalChangePercentByMonth(decimal month, decimal lastMonth)
        {
            if (lastMonth == 0) return month == 0 ? 0m : 100m;
            return ((month - lastMonth) / lastMonth) * 100m;
        }
    }
}
