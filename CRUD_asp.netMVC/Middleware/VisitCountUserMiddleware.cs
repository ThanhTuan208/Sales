using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.FileSystemGlobbing.Internal.PatternContexts;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using StackExchange.Redis;
using System.Security.Claims;

namespace CRUD_asp.netMVC.Middleware
{

    public class VisitCountUserMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConnectionMultiplexer _redis;

        private const string COOKIE_NAME_PREFIX = "uv_"; // cookie user visitors
        private const string TOTAL_KEY_PREFIX = "uv:total:"; // total visits
        private const string DAU_KEY_PREFIX = "hll:dau:"; // daily active users

        private const string ChannelName = "site:updates";

        public VisitCountUserMiddleware(RequestDelegate next, IConnectionMultiplexer redis)
        {
            _next = next;
            _redis = redis;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var db = _redis.GetDatabase();
            var subscriber = _redis.GetSubscriber();
            
            var pathLink = context.Request.Path.ToString();

            var today = DateTime.UtcNow.ToString("yyyyMMdd");
            var month = DateTime.UtcNow.ToString("yyyyMM");

            var dauKey = DAU_KEY_PREFIX + today; // Key de luu danh sach user da login hom nay
            var totalKey = TOTAL_KEY_PREFIX + today; // Key de luu so luong truy cap hom na
            var cookieName = COOKIE_NAME_PREFIX + today; // Ten cookie danh dau user da truy cap hom nay

            if (!context.Request.Cookies.ContainsKey(cookieName))
            {
                await db.StringIncrementAsync(totalKey);
                var expires = DateTime.UtcNow.Date.AddDays(1).AddMinutes(-1); // thoi gian het han

                //Dùng để đánh dấu user đã truy cập hôm nay, tránh đếm trùng.
                context.Response.Cookies.Append(cookieName, "true", new CookieOptions
                {
                    Path = "/", // Cookie áp dụng cho toàn bộ site.
                    Secure = true, //Tăng bảo mật, tránh XSS đọc cookie.
                    HttpOnly = true, //Tăng bảo mật, tránh XSS đọc cookie.
                    Expires = expires, //Hôm sau visitor sẽ bị tính lại là “mới”.
                    SameSite = SameSiteMode.Lax, //Cookie không gửi trong request cross-site trừ khi user click link từ site khác,
                                                 //Giúp giảm rủi ro CSRF.
                });

                // Set TTL(Time To Live) cho uv
                await db.KeyExpireAsync(totalKey, TimeSpan.FromDays(2));

                // Publish event (payload nhẹ: chỉ ngày)
                await subscriber.PublishAsync(ChannelName, today);
            }

            // Dem DAU neu user da login
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = context.User.FindFirst(ClaimTypes.Role)?.Value.ToLower();

            if (!string.IsNullOrEmpty(userId) && isAdmin != "admin")
            {
                // DÙNG PFADD THAY SADD (nhanh, tiet kiem dung luong hon db.SetAddAsync())
                var isDau = await db.HyperLogLogAddAsync(dauKey, userId); // +1 nguoi dung da login
                if (isDau)
                {
                    // Set TTL 2 ngay de so sanh DAU hom nay voi hom qua (co the mo rong de so sanh tuan, thang)
                    await db.KeyExpireAsync(dauKey, TimeSpan.FromDays(2));
                    await subscriber.PublishAsync(ChannelName, today);
                }
            }

            await _next(context);
        }
    }
}
