using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.ViewModels.Home;
using Microsoft.EntityFrameworkCore;

namespace CRUD_asp.netMVC.Service.Home
{
    public class DisplayOrderTrackingService : IDisplayOrderTrackingService
    {
        private readonly AppDBContext _dbContext;

        public DisplayOrderTrackingService(AppDBContext dbContext) => _dbContext = dbContext;

        public async Task<HomeViewModel> DisplayOrderPaidItemsAsync(HomeViewModel model, int userId)
        {
            try
            {
                var orderDetailList = await _dbContext.OrderDetail.AsNoTracking()
                                                                    .Include(p => p.Product).ThenInclude(p => p.Cate)
                                                                    .ToListAsync();

                var paymentOrderList = await _dbContext.Payment.AsNoTracking()
                                                                .Where(p => p.Order.UserID == userId)
                                                                .Include(p => p.Order).ThenInclude(p => p.Address)
                                                                .Include(p => p.Order).ThenInclude(p => p.Users)
                                                                .OrderByDescending(p => p.ID)
                                                                .Take(4)
                                                                .ToListAsync();

                model.OrderPayList = orderDetailList;
                model.PaymentList = paymentOrderList;

                return model;
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}
