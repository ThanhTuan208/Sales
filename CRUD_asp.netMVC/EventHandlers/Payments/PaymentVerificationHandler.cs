using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.DTO.Payments;
using CRUD_asp.netMVC.Models.Auth;
using CRUD_asp.netMVC.Models.Payments;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
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

                if (order.Amount > evt.AmountRecive) // thieu tien
                {
                    underPaid = new UnderpaidOrder()
                    {
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
                else if (order.Amount < evt.AmountRecive) // du hoac du tien
                {
                    excess = new ExcessPayment()
                    {
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

                await _dbContext.SaveChangesAsync();

                _dbContext.MoneyFlowLogs.Add(new MoneyFlowLog()
                {
                    UserId = order.UserID,
                    RelatedId = excess != null ? excess.Id : underPaid.Id,
                    Type = excess != null ? "ExcessCreated" : "UnderpaidCreated",
                    CreatedAt = DateTime.UtcNow.AddHours(7),
                    Description = $"Lịch sử cho đơn hàng {order.ID}",
                    Amount = excess != null
                            ? excess.ExcessAmount
                            : underPaid.MissingAmount,
                });

                _dbContext.Payment.Add(new PaymentModel()
                {
                    OrderID = order.ID,
                    paidAmount = order.Amount,
                    PaymentDate = DateTime.Now,
                    paymentMethod = order.PaymentMethod
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
    }
}
