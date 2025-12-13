using CRUD_asp.netMVC.DTO.Admin;
using CRUD_asp.netMVC.Models.Auth;
using CRUD_asp.netMVC.Models.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CRUD_asp.netMVC.Extensions.Payments
{
    public static class PaymentQueryExtensions
    {
        public static async Task<decimal> ByToDayAsync(this DbSet<Payment> payment)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            return await payment.Where(p => EF.Property<DateOnly>(p, "StartDay") == today)
                                .SumAsync(s => s.paidAmount) ?? 0;
        }   

        public static async Task<decimal> ByToYesterdayAsync(this DbSet<Payment> payment)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1));
            return await payment.Where(p => EF.Property<DateOnly>(p, "StartDay") == today)
                                .SumAsync(s => s.paidAmount) ?? 0;
        }

        public static async Task<decimal> ByToLastMonthAsync(this DbSet<Payment> payment)
        {
            var year = DateTime.UtcNow.Year;
            var lastMonth = DateTime.UtcNow.AddMonths(-1).Month;

            return await payment.Where(p => EF.Property<int>(p, "StartMonth") == lastMonth
                                            && EF.Property<int>(p, "StartYear") == year)
                                .SumAsync(s => s.paidAmount) ?? 0;
        }

        public static async Task<decimal> ByMonthAsync(this DbSet<Payment> payment)
        {
            var month = DateTime.UtcNow.Month;
            var year = DateTime.UtcNow.Year;

            return await payment.Where(p => EF.Property<int>(p, "StartMonth") == month
                                            && EF.Property<int>(p, "StartYear") == year)
                                .SumAsync(s => s.paidAmount) ?? 0;
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
