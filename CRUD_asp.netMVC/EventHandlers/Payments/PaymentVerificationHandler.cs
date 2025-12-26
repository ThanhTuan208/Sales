using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.DTO.Payments;
using CRUD_asp.netMVC.Migrations;
using CRUD_asp.netMVC.Models.Auth;
using CRUD_asp.netMVC.Models.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Security.Claims;
using System.Threading.Tasks;
using PaymentModel = CRUD_asp.netMVC.Models.Payments.Payment;

namespace CRUD_asp.netMVC.EventHandlers.Payments
{
    public class PaymentVerificationHandler : IEventHandler<PaymentVerificationEvent>
    {
        private readonly IEventBus _event;
        private readonly AppDBContext _dbContext;

        public PaymentVerificationHandler(IEventBus @event, AppDBContext dbContext)
        {
            _event = @event;
            _dbContext = dbContext;
        }

        public async Task HandleAsync(PaymentVerificationEvent evt)
        {
            using var transactions = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var order = evt.Order;
                ExcessPayment? excess = null;
                UnderpaidOrder? underPaid = null;
                PaymentModel? payment = null;

                // Can tao them messageBox UI hoi xem nguoi dung co muon thanh toan so tien du ca nhan neu so tien thanh toan don hang ko du khong ?
                var checkExcessPayent = await CheckExcessPaymentAsync(evt.AmountRecive, order.Amount, order.Users.Id);
                if (checkExcessPayent)
                {
                    //var paymentInExcessMoney = await _dbContext.ExcessPayments.SumAsync(p => )
                    payment = new PaymentModel()
                    {
                        OrderID = order.ID,
                        paidAmount = order.Amount,
                        PaymentDate = DateTime.UtcNow,
                        paymentMethod = order.PaymentMethod
                    };

                    _dbContext.Payment.Add(payment);
                }
                else
                {
                    if (order.Amount > evt.AmountRecive) // thieu tien
                    {
                        underPaid = new UnderpaidOrder()
                        {
                            Id = Guid.NewGuid().ToString("N").Substring(0, 12).ToUpper(),
                            OrderId = order.ID,
                            UserId = order.UserID,
                            OrderAmount = order.Amount,
                            PaidAmount = evt.AmountRecive ?? 0,
                            MissingAmount = order.Amount - evt.AmountRecive ?? 0,
                            Status = "Pending",
                            CreatedAt = DateTime.UtcNow.AddHours(7)
                        };

                        _dbContext.UnderpaidOrders.Add(underPaid);
                    }
                    else if (order.Amount <= evt.AmountRecive) // du hoac du tien
                    {
                        if (order.Amount < evt.AmountRecive)
                        {
                            excess = new ExcessPayment()
                            {
                                Id = Guid.NewGuid().ToString("N").Substring(0, 12).ToUpper(),
                                UserId = order.UserID,
                                OrderId = order.ID,
                                OriginalAmount = order.Amount,
                                PaidAmount = evt.AmountRecive ?? 0,
                                ExcessAmount = evt.AmountRecive - order.Amount ?? 0,
                                Status = "Available",
                                CreatedAt = DateTime.UtcNow.AddHours(7)
                            };

                            _dbContext.ExcessPayments.Add(excess);
                        }

                        if (!checkExcessPayent)
                        {
                            payment = new PaymentModel()
                            {
                                OrderID = order.ID,
                                paidAmount = order.Amount,
                                PaymentDate = DateTime.Now,
                                paymentMethod = order.PaymentMethod
                            };

                            _dbContext.Payment.Add(payment);
                        }
                    }
                }

                await _dbContext.SaveChangesAsync();

                var excessPayentLog = await _dbContext.ExcessPayments.Where(p => p.UserId == order.UserID)
                                                        .SumAsync(p => p.ExcessAmount);

                var userWalletExist = await _dbContext.UserWallets.FirstOrDefaultAsync(p => p.UserId == order.UserID);
                if (userWalletExist == null)
                {
                    _dbContext.UserWallets.Add(new UserWallet()
                    {
                        UserId = order.UserID,
                        Balance = excess != null ? excess.ExcessAmount : 0,
                        LockedBalance = 0,
                        UpdatedAt = DateTime.UtcNow
                    });
                }
                else
                {
                    if (excess != null)
                    {
                        userWalletExist.Balance += excess.ExcessAmount;
                        userWalletExist.UpdatedAt = DateTime.UtcNow;
                    }
                    else if (checkExcessPayent)
                    {
                        userWalletExist.Balance -= underPaid.MissingAmount;
                        userWalletExist.UpdatedAt = DateTime.UtcNow;
                    }
                }

                _dbContext.MoneyFlowLogs.Add(new MoneyFlowLog()
                {
                    Id = Guid.NewGuid().ToString("N").Substring(0, 12).ToUpper(),
                    UserId = order.UserID,
                    RelatedId = excess != null
                                    ? excess.Id
                                    : underPaid != null
                                        ? underPaid.Id
                                        : $"paymentID_{payment?.ID}",
                    Type = excess != null
                                ? "ExcessCreated"
                                : underPaid != null
                                    ? "UnderpaidCreated"
                                    : "NormalCreated",

                    Amount = excess != null
                                ? excess.ExcessAmount
                                : underPaid != null
                                    ? underPaid.PaidAmount
                                    : order.Amount ?? 0,

                    BalanceSnapshot = 0,
                    AffectBalance = excess != null ? true : false,
                    Description = $"{order.ID}",
                    CreatedAt = DateTime.UtcNow.AddHours(7),
                });

                _dbContext.Attach(order);
                order.Status = "Paid";
                order.PaidAt = DateTime.UtcNow.AddHours(7);
                _dbContext.Orders.Update(order);

                foreach (var item in order.OrderDetail)
                {
                    var affectRow = await _dbContext.Products
                   .Where(p => p.ID == item.ProductID && p.Quantity >= item.Quantity)
                   .ExecuteUpdateAsync(s => s.SetProperty(p => p.Quantity, p => p.Quantity - item.Quantity));

                    if (affectRow > 0)
                    {
                        await _dbContext.Carts
                        .Where(p => p.UserID == evt.Order.UserID && p.ProductID == item.ProductID && p.IsDelete)
                        .ExecuteDeleteAsync();
                    }
                }

                await _dbContext.SaveChangesAsync();
                await transactions.CommitAsync();

                if (excess != null)
                {
                    await _event.PublishAsync(new OrderPaidEvent(order.ID, order.TransactionId, true));
                }
            }
            catch
            {
                await transactions.RollbackAsync();
            }
        }

        private async Task<bool> CheckExcessPaymentAsync(decimal? receivePay, decimal? orderAmount, int userId)
        {
            var PaymentAmount = receivePay - orderAmount; // PaymentAmount: so tien am
            if (PaymentAmount < 0M) // thieu tien
            {
                // xu ly thieu tien, lay tien du trong tui tru di tien thieu
                var excessAmount = await _dbContext.ExcessPayments.Where(p => p.UserId == userId).SumAsync(p => p.ExcessAmount);

                // kiem tra xem thieu trong tui du tien tru tien bij thieu ko
                return (excessAmount + PaymentAmount) > 0;
            }
            return false;
        }
    }
}
