using CRUD_asp.netMVC.Common;
using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.DTO.Admin;
using CRUD_asp.netMVC.DTO.Generic;
using CRUD_asp.netMVC.DTO.Payments;
using CRUD_asp.netMVC.Extensions.Admins;
using CRUD_asp.netMVC.Extensions.Payments;
using CRUD_asp.netMVC.Extensions.SiteUsers;
using CRUD_asp.netMVC.Extensions.Users;
using CRUD_asp.netMVC.Migrations;
using CRUD_asp.netMVC.ViewModels.Home;
using EFCoreSecondLevelCacheInterceptor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Org.BouncyCastle.Asn1.IsisMtt.X509;
using Org.BouncyCastle.Tls.Crypto.Impl.BC;
using Org.BouncyCastle.Utilities.Zlib;
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
                model.Users = await _dbContext.Users.FirstOrDefaultAsync(p => p.Id == userId) ?? new Models.Auth.Users();
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
            string relatedid = "";
            var excess = _dbContext.ExcessPayments;
            var paidAmountForOrders = new Dictionary<string, decimal?>()
            {
                { "wallet", 0m },
                { "payment", 0m },
                { "under", 0m },
            };

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
                    //m.Description,
                    m.CreatedAt,
                    m.AffectBalance,
                    OrderAmount = _dbContext.Orders.Where(o => o.ID == m.OrderId)
                                                    .Select(o => o.Amount)
                                                    .FirstOrDefault()
                }).ToListAsync();


            var moneyFlowLogs = rawLogs
                .GroupBy(x => x.OrderId)
                .Select(g => g.FirstOrDefault(p => !p.Type.ToLower().Contains("excess")))
                .Where(x => x != null)
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
                    OrderAmount = x.OrderAmount,
                    MissingAmount = x.PaidAmount < x.OrderAmount ? 1 : null,
                    ExcessAmount = x.PaidAmount > x.Amount ? 2 : null,
                    BalanceSnapshot = x.BalanceSnapshot,
                    CreatedAtRaw = x.CreatedAt,
                    AffectBalance = x.AffectBalance
                })
                .ToList();

            moneyFlowLogs.ForEach(p =>
            {
                p.CreatedAt = PaymentQueryExtensions.ConvertTimeAsia(p.CreatedAtRaw);
                p.WalletBalance = PaymentQueryExtensions.GetWalletAmountByUserId(_dbContext.UserWallets, int.Parse(p.User));
                p.User = UserQueryExtensions.GetNameByUserId(_dbContext.Users, int.Parse(p.User));

                if (p.MissingAmount.HasValue)
                {
                    p.MissingAmount = PaymentQueryExtensions.GetAmount(_dbContext, p.MissingAmount, p.RelatedId);
                }

                if (p.ExcessAmount.HasValue)
                {
                    relatedid = PaymentQueryExtensions.GetRealatedIdByExcessEntity(_dbContext.ExcessPayments, p.OrderId);
                    p.ExcessAmount = PaymentQueryExtensions.GetAmount(_dbContext, p.ExcessAmount, relatedid);
                }

                if (p.Type.Contains("Wallet"))
                {
                    paidAmountForOrders["wallet"] += p.PaidAmount;
                }
                else if (p.Type.Contains("Payment"))
                {
                    paidAmountForOrders["payment"] += p.Amount;
                }
                else if (p.Type.Contains("Under"))
                {
                    paidAmountForOrders["under"] += p.Amount;
                }
            });

            surplusMoney.MoneyFlowLogs = moneyFlowLogs;

            var lastUpdateAt = await excess.Where(p => p.UpdatedAt != null)
                                           .OrderByDescending(p => p.UpdatedAt)
                                           .Select(p => p.UpdatedAt)
                                           .FirstOrDefaultAsync();

            if (lastUpdateAt.HasValue)
            {
                surplusMoney.UpdateAt = PaymentQueryExtensions.ConvertTimeAsia(lastUpdateAt);
            }
            else
            {
                var dateExcess = excess.OrderByDescending(p => p.CreatedAt).Select(p => p.CreatedAt).FirstOrDefault();
                surplusMoney.UpdateAt = PaymentQueryExtensions.ConvertTimeAsia(dateExcess);
            }

            DateTime date = DateTime.UtcNow;
            surplusMoney.PaidMoney = paidAmountForOrders.Sum(p => p.Value) ?? 0m;

            surplusMoney.TotalAmountInMonth = moneyFlowLogs.Where(p => p.CreatedAtRaw.Month == date.Date.Month && p.CreatedAtRaw.Year == date.Date.Year)
                                                            .Sum(p => p.PaidAmount ?? 0M) ;

            surplusMoney.ExcessMoneyNow = _dbContext.UserWallets.FirstOrDefault(p => p.UserId == model.Users.Id)?.Balance ?? 0M;
        }
    }
}
