using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.ViewModels.Home;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.VisualBasic;
using System.Security.Claims;

namespace CRUD_asp.netMVC.Service.Home
{
    public class DisplayProfileUserService : IDisplayProfileUserService
    {
        private readonly AppDBContext _dbContext;
        private readonly IHttpContextAccessor _http;

        public DisplayProfileUserService(AppDBContext dbContext, IHttpContextAccessor http)
        {
            _dbContext = dbContext;
            _http = http;
        }

        public Task<string> HandleProfileDisplayAsync(string? options)
        {
            return Task.FromResult(options?.ToLower() switch
            {
                "profile" => "_MyProfilePartial",
                "surplus" => "_SurplusManagerPartial",
                _ => "_MyProfilePartial"
            });
        }

        public async Task<IBaseProfileViewModel> DisplayViewModel(string? option)
        {
            return option?.ToLower() switch
            {
                "profile" => await BuildBaseViewModelAsync<HomeViewModel>(),
                "surplus" => await BuildBaseViewModelAsync<SurplusMoneyViewModel>(),
                _ => await BuildBaseViewModelAsync<HomeViewModel>(),
            };
        }

        public async Task<T> BuildBaseViewModelAsync<T>() where T : class, IBaseProfileViewModel, new()
        {
            T model = new T();
            var userIdClaim = _http.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                model.Users = await _dbContext.Users.FirstOrDefaultAsync(p => p.Id == userId);
            }
            model.Brands = await _dbContext.Brand.ToListAsync();
            model.Categories = await _dbContext.Category.ToListAsync();

            if (model is SurplusMoneyViewModel surplusMoney)
            {
                await SurplusMoneyMethodAsync(model, surplusMoney);
            }

            return model;
        }

        public async Task SurplusMoneyMethodAsync(IBaseProfileViewModel model, SurplusMoneyViewModel surplusMoney)
        {
            var totalInMonth = 0M;
            var excess = _dbContext.ExcessPayments;

            surplusMoney.TotalAmountInMonth = await excess.Where(p => p.UserId == model.Users.Id && p.CreatedAt.Date.Month == DateTime.UtcNow.Date.Month)
                                                            .SumAsync(p => p.PaidAmount);
            surplusMoney.TotalAmountInMonth += await _dbContext.UnderpaidOrders.Where(p => p.UserId == model.Users.Id && p.CreatedAt.Date.Month == DateTime.UtcNow.Date.Month && p.Status != "Pending")
                                                                            .SumAsync(p => p.PaidAmount);
             
            surplusMoney.ExcessMoney = await excess.Where(p => p.UserId == model.Users.Id)
                                                        .SumAsync(p => p.ExcessAmount);

            surplusMoney.PaidMoney = await excess.Where(p => p.UserId == model.Users.Id)
                                                        .SumAsync(p => p.OriginalAmount.Value);

            surplusMoney.PaidMoney += await _dbContext.UnderpaidOrders.Where(p => p.UserId == model.Users.Id && p.Status != "Pending")
                                                        .SumAsync(p => p.PaidAmount);

            var lastUpdateAt = await excess.Where(p => p.UpdatedAt != null)
                                           .OrderByDescending(p => p.UpdatedAt)
                                           .Select(p => p.UpdatedAt)
                                           .FirstOrDefaultAsync();

            surplusMoney.MoneyFlowLogs = await _dbContext.MoneyFlowLogs
                                        .Where(p => p.UserId == model.Users.Id)
                                        .OrderByDescending(p => p.CreatedAt)
                                        .ToListAsync();
            if (lastUpdateAt.HasValue)
            {
                surplusMoney.UpdateAt = lastUpdateAt.Value.ToString("dd/MM/yyyy HH:mm");
            }
            else surplusMoney.UpdateAt = excess.OrderByDescending(p => p.CreatedAt)
                                               .Select(p => p.CreatedAt)
                                               .FirstOrDefault().ToString("dd/MM/yyyy HH:mm");
        }
    }
}
