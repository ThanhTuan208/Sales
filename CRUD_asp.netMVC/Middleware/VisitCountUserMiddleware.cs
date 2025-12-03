using CRUD_asp.netMVC.Service.Payment.SiteVisitService;
using System.ComponentModel;
using System.IO;

namespace CRUD_asp.netMVC.Middleware
{
    public class VisitCountUserMiddleware
    {
        private readonly RequestDelegate _next;

        public VisitCountUserMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context, ISiteUserVisitService siteUser)
        {
            var path = context.Request.Path.Value?.ToLowerInvariant() ?? "";
            var method = context.Request.Method;

            // Bỏ qua bot, file tĩnh, api, hub...
            if (method != "GET" ||
                path.Contains("favicon") ||
                path.Contains("apple-touch-icon") ||
                path.Contains(".ico") ||
                path.Contains(".png") ||
                path.Contains(".jpg") ||
                path.Contains(".css") ||
                path.Contains(".js") ||
                path.Contains("/hub") ||
                path.Contains("/hangfire") ||
                path.Contains("robots.txt"))
            {
                await _next(context);
                return;
            }

            if (path == "/" || path == "/home/index" || path == "" || path == "/admin/dashboard")
            {
                await siteUser.IncreaseSiteVisit(context);
                await siteUser.GetTodayVisitSiteAsysnc();
            }
            await _next(context);
        }
    }
}
