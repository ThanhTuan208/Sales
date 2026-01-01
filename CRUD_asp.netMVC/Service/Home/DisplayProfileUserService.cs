using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.DTO.Payments;
using CRUD_asp.netMVC.Extensions.Admins;
using CRUD_asp.netMVC.Extensions.Payments;
using CRUD_asp.netMVC.Extensions.SiteUsers;
using CRUD_asp.netMVC.Extensions.Users;
using CRUD_asp.netMVC.ViewModels.Home;
using EFCoreSecondLevelCacheInterceptor;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace CRUD_asp.netMVC.Service.Home
{
    public class DisplayProfileUserService : IDisplayProfileUserService
    {
        private readonly AppDBContext _dbContext;
        private readonly IHttpContextAccessor _http;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public DisplayProfileUserService(AppDBContext dbContext, IHttpContextAccessor http, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _http = http;
            _httpContextAccessor = httpContextAccessor;
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

            var rawLogs = await _dbContext.MoneyFlowLogs
                .Where(m => m.UserId == model.Users.Id)
                .Select(m => new
                {
                    m.Id,
                    m.UserId,
                    m.OrderId,
                    m.RelatedId,
                    m.Type,
                    m.Amount,
                    m.PaidAmount,
                    m.BalanceSnapshot,
                    m.Description,
                    m.CreatedAt,
                    m.AffectBalance,
                    OrderAmount = _dbContext.Orders.Where(o => o.ID == m.OrderId)
                                                    .Select(o => o.Amount)
                                                    .FirstOrDefault()
                })
                .ToListAsync();

            var MoneyFlowLogs = rawLogs
                .GroupBy(x => x.OrderId)
                .Select(g => g
                    .OrderByDescending(x => x.AffectBalance)
                    .ThenByDescending(x => x.CreatedAt)
                    .First()
                )
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new MoneyFlowLogDTO
                {
                    Id = x.Id,
                    User = x.UserId.ToString(),
                    OrderId = x.OrderId,
                    RelatedId = x.RelatedId,
                    Type = x.Type,
                    Amount = x.Amount,
                    PaidAmount = x.PaidAmount,
                    AmountOrder = x.OrderAmount,
                    MissingAmount = x.PaidAmount == x.Amount ? 1 : null,
                    ExcessAmount = x.PaidAmount > x.Amount ? 2 : null,
                    BalanceSnapshot = x.BalanceSnapshot,
                    Description = x.Description,
                    CreatedAtRaw = x.CreatedAt,
                    AffectBalance = x.AffectBalance
                })
                .ToList();

            MoneyFlowLogs.ForEach(p =>
            {
                p.CreatedAt = PaymentQueryExtensions.ConvertTimeAsia(p.CreatedAtRaw);
                p.WalletBalance = PaymentQueryExtensions.GetWalletAmountByUserId(_dbContext.UserWallets,int.Parse(p.User));
                p.User = UserQueryExtensions.GetNameByUserId(_dbContext.Users, int.Parse(p.User));

                if (p.MissingAmount.HasValue)
                {
                    p.MissingAmount = PaymentQueryExtensions.GetAmount(_dbContext, p.MissingAmount, p.RelatedId);
                }
                else if (p.ExcessAmount.HasValue)
                {
                    p.ExcessAmount = PaymentQueryExtensions.GetAmount(_dbContext, p.ExcessAmount, p.RelatedId);
                }
            });
        
            surplusMoney.MoneyFlowLogs = MoneyFlowLogs;

            if (lastUpdateAt.HasValue)
            {
                surplusMoney.UpdateAt = PaymentQueryExtensions.ConvertTimeAsia(lastUpdateAt);
            }
            else
            {
                var dateExcess = excess.OrderByDescending(p => p.CreatedAt).Select(p => p.CreatedAt).FirstOrDefault();
                surplusMoney.UpdateAt = PaymentQueryExtensions.ConvertTimeAsia(dateExcess);
            }
        }

        private string GetOrderIdByMoneyFlowLogEntity(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            var match = Regex.Match(text, @"đơn\s+(.+)$");

            return match.Success ? match.Groups[1].Value : string.Empty;
        }
    }
}
