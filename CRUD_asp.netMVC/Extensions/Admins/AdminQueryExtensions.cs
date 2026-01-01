using CRUD_asp.netMVC.Models.Auth;
using Microsoft.EntityFrameworkCore;
using User = CRUD_asp.netMVC.Models.Auth.Users;

namespace CRUD_asp.netMVC.Extensions.Admins
{
    public static class AdminQueryExtensions
    {
        public static async Task<int> ByToDayAsync(this DbSet<User> user)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            return await user.CountAsync(p => EF.Property<DateOnly>(p, "StartDay") == today);
        }

        public static async Task<int> ByToYesterdayAsync(this DbSet<User> user)
        {
            var yesterday = DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(-1));
            return await user.CountAsync(p => EF.Property<DateOnly>(p, "StartDay") == yesterday);
        }

        //public static async Task<int> ByQuarerAsync(this DbSet<Users> user)
        //{
        //    var quarter = (DateTime.UtcNow.Month - 1) / 3 + 1;
        //    var year = DateTime.UtcNow.Year;

        //    return await user.CountAsync(p =>
        //                EF.Property<int>(p, "StartQuarter") == quarter &&
        //                EF.Property<int>(p, "StartYear") == year);
        //}

        // Tinh ty le phan tram thay doi nguoi dung hom qua, hom nay
        public static decimal CalcChangePercent(int today, int yesterday)
        {
            if (yesterday == 0) return today == 0 ? 0m : 100m;
            return ((decimal)(today - yesterday) / yesterday) * 100m;
            //vd: hom nay 50, hom qua 40 => (50-40)/40 * 100 = 25%
        }
    }
}
