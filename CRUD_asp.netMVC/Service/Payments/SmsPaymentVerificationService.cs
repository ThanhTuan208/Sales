using CRUD_asp.netMVC.Common;
using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.DTO.Generic;
using CRUD_asp.netMVC.DTO.Payments;
using CRUD_asp.netMVC.EventHandlers;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace CRUD_asp.netMVC.Service.Payments
{
    public class SmsPaymentVerificationService : ISmsPaymentVerificationService
    {
        private readonly IEventBus _event;
        private readonly AppDBContext _dbContext;

        public SmsPaymentVerificationService(AppDBContext dbContext, IEventBus @event)
        {
            _dbContext = dbContext;
            _event = @event;
        }

        public async Task<Result<Unit>> ProcessResultAsync(string message)
        {
            try
            {
                if (string.IsNullOrEmpty(message))
                {
                    return Result<Unit>.Fail("Tin nhắn không được null hoặc rỗng!");
                }

                var transactionCode = Regex.Match(message, @"ORD([^-]+)");
                var amountPayment = Regex.Match(message, @"GD\s*\+([\d,]+)VND");

                if (!transactionCode.Success)
                {
                    return Result<Unit>.Fail("Mã đơn hàng được gửi đến không hợp lệ (giữ mã giao dịch ORD...)!");
                }

                var transactionId = transactionCode.Groups[1].Value;
                var amountPaymentMatch = amountPayment.Groups[1].Value.Replace(",", "");

                if (decimal.TryParse(amountPaymentMatch, out decimal amountRecive))
                {
                    if (amountRecive == 0)
                    {
                        return Result<Unit>.Fail("Thành toán đơn hàng thất bại!");
                    }
                }

                var order = await _dbContext.Orders
                                        .AsNoTracking()
                                        .Include(p => p.Address)
                                        .Include(p => p.Users)
                                        .Include(p => p.OrderDetail).ThenInclude(p => p.Product)
                                        .FirstOrDefaultAsync(p => p.TransactionId == transactionId);

                if (order == null)
                {
                    return Result<Unit>.Fail("Không tìm thấy đơn hàng!");
                }

                if (order.Status == "Paid")
                {
                    return Result<Unit>.OK("Đơn hàng đã được thanh toán trước đó!");
                }

                await _event.PaymentVerificationAsync(new PaymentVerificationEvent(order, amountRecive));

                return Result<Unit>.OK("Thanh toán thành công.");
            }
            catch (Exception ex)
            {
                return Result<Unit>.Fail(ex.Message);
            }
        }

    }
}
