using CRUD_asp.netMVC.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class OrderCleanupService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public OrderCleanupService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    // Xóa các order Pending quá 15 phút
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDBContext>();

                // Tìm các order Pending quá 10 phút
                var expiredOrders = await db.Orders
                    .Where(o => o.Status == "Pending"
                             && o.OrderDate.AddMinutes(5) < DateTime.Now)
                    .ToListAsync();

                if (expiredOrders.Any())
                {
                    db.Orders.RemoveRange(expiredOrders);
                    await db.SaveChangesAsync();
                }
            }

            // Chạy lại sau 1 phút
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}
