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
        private const string DAU_KEY_PREFIX = "dau:"; // daily active users

        public VisitCountUserMiddleware(RequestDelegate next, IConnectionMultiplexer redis)
        {
            _next = next;
            _redis = redis;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var db = _redis.GetDatabase();
            var today = DateTime.UtcNow.ToString("yyyyMMdd");

            var dauKey = DAU_KEY_PREFIX + today; // Key de luu danh sach user da login hom nay
            var totalKey = TOTAL_KEY_PREFIX + today; // Key de luu so luong truy cap hom nay
            var cookieName = COOKIE_NAME_PREFIX + today; // Ten cookie danh dau user da truy cap hom nay

            if (!context.Request.Cookies.ContainsKey(cookieName))
            {
                await db.StringIncrementAsync(totalKey);
                var expires = DateTime.UtcNow.Date.AddDays(1).AddMinutes(-1); // thoi gian het han

                //Dùng để đánh dấu user đã truy cập hôm nay, tránh đếm trùng.
                context.Response.Cookies.Append(cookieName, "true", new CookieOptions
                {
                    Expires = expires, //Hôm sau visitor sẽ bị tính lại là “mới”.
                    HttpOnly = true, //Tăng bảo mật, tránh XSS đọc cookie.
                    Secure = true, //Tăng bảo mật, tránh XSS đọc cookie.
                    Path = "/", // Cookie áp dụng cho toàn bộ site.
                    SameSite = SameSiteMode.Lax, //Cookie không gửi trong request cross-site trừ khi user click link từ site khác,
                                                 //Giúp giảm rủi ro CSRF.
                });
            }

            // Dem DAU neu user da login
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                await db.SetAddAsync(dauKey, userId); // +1 nguoi dung da login
            }
            await _next(context);
        }
    }
}
