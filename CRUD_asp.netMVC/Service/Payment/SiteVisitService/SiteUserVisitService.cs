using AspNetCoreGeneratedDocument;
using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.Models.Auth;
using Microsoft.EntityFrameworkCore;

namespace CRUD_asp.netMVC.Service.Payment.SiteVisitService
{
    public class SiteUserVisitService : ISiteUserVisitService
    {
        private readonly AppDBContext _dbContext;
        private string cookieKey = $"visited_{DateTime.UtcNow:yyyyMMdd}";
        public SiteUserVisitService(AppDBContext dbContext) => _dbContext = dbContext;

        // Dem so luong nguoi truy cap trong ngay
        public async Task<int> GetTodayVisitSiteAsysnc()
        {
            var today = DateTime.UtcNow.Date;
            return await _dbContext.SiteUser.Where(p => p.Date == today)
                                            .Select(p => p.TotalView)
                                            .FirstOrDefaultAsync();
        }

        // Cap nhat so luong nguoi truy cap
        public async Task IncreaseSiteVisit(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLowerInvariant() ?? "";
            if (context.Request.Cookies.ContainsKey(cookieKey))
            {
                return;
            }

            var today = DateTime.UtcNow.Date;

            // them indexing cho date truoc roi thuc hien cau lenh !
            await _dbContext.Database.ExecuteSqlRawAsync(
                @"
                UPDATE SiteUser SET TotalView = TotalView + 1 
                WHERE [Date] = {0};

                IF @@ROWCOUNT = 0
                    INSERT INTO SiteUser ([Date], TotalView) 
                    VALUES ({0}, 1);
                ", today);

          
            context.Response.Cookies.Append(cookieKey, "true", new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(1),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax
            });
        }
    }
}
