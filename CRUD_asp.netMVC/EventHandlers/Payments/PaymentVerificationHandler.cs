using CRUD_asp.netMVC.Common;
using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.DTO.Generic;
using CRUD_asp.netMVC.DTO.Payments;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
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

                if (evt.Order.Amount > evt.AmountRecive)
                {

                }
                else if (evt.Order.Amount < evt.AmountRecive || evt.Order.Amount == evt.AmountRecive)
                {
                    decimal? remainingAmount = 0;

                    if (evt.Order.Amount < evt.AmountRecive)
                    {
                        remainingAmount = evt.AmountRecive - evt.Order.Amount;
                    }

                    _dbContext.Payment.Add(new PaymentModel()
                    {
                        OrderID = evt.Order.ID,
                        paidAmount = evt.Order.Amount,
                        PaymentDate = DateTime.Now,
                        paymentMethod = evt.Order.PaymentMethod
                    });

                    foreach (var item in evt.Order.OrderDetail)
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
                    ;

                    _dbContext.Attach(evt.Order);
                    evt.Order.Status = "Paid";
                    evt.Order.PaidAt = DateTime.UtcNow.AddHours(7);
                    _dbContext.Orders.Update(evt.Order);

                    await _dbContext.SaveChangesAsync();
                }

                await transactions.CommitAsync();
                await _event.PublishAsync(new OrderPaidEvent(evt.Order.ID, evt.Order.TransactionId, true));
            }
            catch
            {
                await transactions.RollbackAsync();
            }
        }
    }
}
