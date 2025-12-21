using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.ViewModels.Home;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
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
            var model = new T();
            var userIdClaim = _http.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                model.Users = await _dbContext.Users.FirstOrDefaultAsync(p => p.Id == userId);
            }
            model.Brands = await _dbContext.Brand.ToListAsync();
            model.Categories = await _dbContext.Category.ToListAsync();

            if (model is SurplusMoneyViewModel surplusMoney)
            {
                var totalInMonth = 0M;
                totalInMonth = await _dbContext.ExcessPayments
                                        .Where(p => p.UserId == model.Users.Id && p.CreatedAt.Date.Month == DateTime.UtcNow.Date.Month)
                                        .SumAsync(p => p.PaidAmount);
                totalInMonth += await _dbContext.UnderpaidOrders
                                        .Where(p => p.UserId == model.Users.Id && p.CreatedAt.Date.Month == DateTime.UtcNow.Date.Month)
                                        .SumAsync(p => p.PaidAmount);
                
                surplusMoney.TotalAmountInMonth = totalInMonth;
            }

            return model;
        }
    }
}
