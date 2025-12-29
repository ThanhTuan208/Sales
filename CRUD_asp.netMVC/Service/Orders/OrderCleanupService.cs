using CRUD_asp.netMVC.Data;
using Microsoft.EntityFrameworkCore;

public class OrderCleanupService
{
    private readonly AppDBContext _dbContext;

    public OrderCleanupService(AppDBContext dbContext) => _dbContext = dbContext;

    // Xoa ds order Pending qua 5 phut
    public async Task CleanExpiredOrderAsync()
    {
        var expiredOrders = await _dbContext.Orders
            .Where(o => o.Status == "Pending" 
                    && o.OrderDate < DateTime.UtcNow.AddMinutes(-5))
            .ToListAsync();

        if (expiredOrders.Any())
        {
            _dbContext.Orders.RemoveRange(expiredOrders);
            await _dbContext.SaveChangesAsync();
        }
    }
}
