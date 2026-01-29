using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.DTO.Payments;
using CRUD_asp.netMVC.Hubs;
using CRUD_asp.netMVC.Service.Payments;
using MessagePack;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MimeKit.Tnef;

namespace CRUD_asp.netMVC.EventHandlers.Payments
{
    public class PaymentVerificationHandler : IEventHandler<PaymentVerificationEvent>
    {
        private readonly AppDBContext _dbContext;
        private readonly IHubContext<QuestionResHub> _hub;
        private readonly ISmsPaymentVerificationService _smsPaymentVerificationService;

        public PaymentVerificationHandler(IHubContext<QuestionResHub> hub, AppDBContext dbContext, ISmsPaymentVerificationService smsPaymentVerificationService)
        {
            _hub = hub;
            _dbContext = dbContext;
            _smsPaymentVerificationService = smsPaymentVerificationService;
        }

        public async Task HandleAsync(PaymentVerificationEvent evt)
        {
            var order = evt.Order;
            string orderId = order.Id;
            int userId = order.UserId;
                    
            string status = order.Status;
            string transactionCode = order.TransactionId;
            string paymentMethod = order.PaymentMethod;

            decimal amountReceive = evt.AmountReceive;
            decimal orderAmount = order.Amount ?? 0;
            decimal? missingAmount = order.Amount - evt.AmountReceive;

            DateTime orderDate = order.OrderDate;

            var walletBalance = await _dbContext.UserWallets.Where(p => p.UserId == order.UserId).SumAsync(p => p.Balance);

            // Thieu tien nhung vi du
            if (evt.AmountReceive < orderAmount && walletBalance >= (orderAmount - evt.AmountReceive))
            {
                await _hub.Clients.User(userId.ToString()).SendAsync(
                    "QuestionResquestUser",
                        orderId,
                        userId,
                        orderAmount,
                        amountReceive,
                        missingAmount,
                        walletBalance,
                        paymentMethod,
                        status,
                        transactionCode,
                        orderDate
                    );
                return;
            }

            await _smsPaymentVerificationService.UserConfirmWalletAsync(evt);
        }
    }
}
