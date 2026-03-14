using CRUD_asp.netMVC.Common;
using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.DTO.Generic;
using CRUD_asp.netMVC.DTO.Order;
using CRUD_asp.netMVC.DTO.Payments;
using CRUD_asp.netMVC.EventHandlers;
using CRUD_asp.netMVC.Models.Auth;
using CRUD_asp.netMVC.Models.Order;
using CRUD_asp.netMVC.Models.Payments;
using CRUD_asp.netMVC.ViewModels.Order;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using PaymentModel = CRUD_asp.netMVC.Models.Payments.Payment;

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
                    return Result<Unit>.Fail("Tin nhắn không được null hoặc rỗng!", 400);
                }

                var transactionCode = Regex.Match(message, @"ORD([^-]+)");
                var amountPayment = Regex.Match(message, @"GD\s*\+([\d,]+)VND");

                if (!transactionCode.Success)
                {
                    return Result<Unit>.Fail("Mã đơn hàng được gửi đến không hợp lệ (giữ mã giao dịch ORD...)!", 400);
                }

                var transactionId = transactionCode.Groups[1].Value;
                var amountPaymentMatch = amountPayment.Groups[1].Value.Replace(",", "");

                if (decimal.TryParse(amountPaymentMatch, out decimal amountRecive))
                {
                    if (amountRecive == 0)
                    {
                        return Result<Unit>.Fail("Thành toán đơn hàng thất bại!", 400);
                    }
                }

                var order = await _dbContext.Orders
                                        .AsNoTracking()
                                        .Include(p => p.Address)
                                        .Include(p => p.Users)
                                        .Include(p => p.OrderDetail).ThenInclude(p => p.Product)
                                        .FirstOrDefaultAsync(p => p.TransactionId == transactionId);

                PaymentVerificationByOrderDTO orderDTO = new()
                {
                    Id = order.ID,
                    UserId = order.UserID,
                    Amount = order.Amount,
                    PaymentMethod = order.PaymentMethod,
                    Status = order.Status ?? string.Empty,
                    TransactionId = order.TransactionId,
                    OrderDate = order.OrderDate
                };

                if (order == null)
                {
                    return Result<Unit>.Fail("Không tìm thấy đơn hàng!", 404);
                }

                if (order.Status == "Paid")
                {
                    return Result<Unit>.OK("Đơn hàng đã được thanh toán trước đó!", 200);
                }

                await _event.PaymentVerificationAsync(new PaymentVerificationEvent(orderDTO, amountRecive));

                return Result<Unit>.OK("Gửi yêu cầu hỏi người dùng thành công.", 200);
            }
            catch (Exception ex)
            {
                return Result<Unit>.Fail(ex.Message, 500);
            }
        }

        public async Task<Result<Unit>> UserConfirmWalletAsync(PaymentVerificationEvent evt)
        {
            using var transactions = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                PaymentVerificationByOrderDTO orderDto = evt.Order;
                Orders order = new Orders()
                {
                    ID = orderDto.Id,
                    UserID = orderDto.UserId,
                    Amount = orderDto.Amount,
                    PaymentMethod = orderDto.PaymentMethod,
                    Status = orderDto.Status,
                    TransactionId = orderDto.TransactionId,
                    TrackingNumber = string.Empty,
                    OrderDate = orderDto.OrderDate,
                    OrderDetail = _dbContext.OrderDetail.Where(p => p.OrderID == orderDto.Id).ToList()
                };

                decimal? beforeUpdateWallet = 0M;
                ExcessPayment? excess = null;
                PaymentModel? payment = null;
                UnderpaidOrder? underPaid = null;

                var checkExcessPayment = await CheckExcessPaymentAsync(evt.AmountReceive, order.Amount, order.UserID);
                // Hoi nguoi dung co muon lay so du trong vi bu vao so tien ck thieu hay ko ?
                if (checkExcessPayment)
                {
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
                    if (order.Amount > evt.AmountReceive) // thieu tien
                    {
                        underPaid = new UnderpaidOrder()
                        {
                            Id = Guid.NewGuid().ToString("N").Substring(0, 12).ToUpper(),
                            OrderId = order.ID,
                            UserId = order.UserID,
                            OrderAmount = order.Amount,
                            PaidAmount = evt.AmountReceive,
                            MissingAmount = order.Amount - evt.AmountReceive ?? 0,
                            Status = "PartiallyPaid",
                            CreatedAt = DateTime.UtcNow
                        };

                        _dbContext.UnderpaidOrders.Add(underPaid);
                    }
                    else if (order.Amount <= evt.AmountReceive) // du hoac du tien
                    {
                        if (order.Amount < evt.AmountReceive)
                        {
                            excess = new ExcessPayment()
                            {
                                Id = Guid.NewGuid().ToString("N").Substring(0, 12).ToUpper(),
                                UserId = order.UserID,
                                OrderId = order.ID,
                                OriginalAmount = order.Amount,
                                PaidAmount = evt.AmountReceive,
                                ExcessAmount = evt.AmountReceive - order.Amount ?? 0,
                                Status = "Available",
                                CreatedAt = DateTime.UtcNow
                            };

                            _dbContext.ExcessPayments.Add(excess);
                        }

                        if (!checkExcessPayment)
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

                var userWalletExist = await _dbContext.UserWallets.FirstOrDefaultAsync(p => p.UserId == order.UserID);
                if (userWalletExist == null)
                {
                    userWalletExist = new UserWallet()
                    {
                        UserId = order.UserID,
                        Balance = excess != null ? excess.ExcessAmount : 0,
                        LockedBalance = 0,
                        UpdatedAt = DateTime.UtcNow
                    };

                    _dbContext.UserWallets.Add(userWalletExist);

                    beforeUpdateWallet = 0M;
                }
                else
                {
                    beforeUpdateWallet = userWalletExist.Balance;
                    if (excess != null)
                    {
                        userWalletExist.Balance += excess.ExcessAmount;
                        userWalletExist.UpdatedAt = DateTime.UtcNow;

                        _dbContext.UserWallets.Update(userWalletExist);
                    }
                    else if (checkExcessPayment)
                    {
                        userWalletExist.Balance -= orderDto.MissingAmount;
                        userWalletExist.UpdatedAt = DateTime.UtcNow;

                        _dbContext.UserWallets.Update(userWalletExist);
                    }
                }
                await _dbContext.SaveChangesAsync();

                if (excess != null)
                {
                    string[] types = { "PaymentCompleted", "ExcessCreated" };
                    string[] relatedIds = { $"PaymentId_{payment?.ID}", excess.Id };
                    decimal[] amounts = { payment.paidAmount ?? 0, excess.ExcessAmount };
                    decimal[] balanceSnapshots = { beforeUpdateWallet ?? 0, userWalletExist?.Balance ?? 0 };
                    bool[] isAffectBalance = { false, true };
                    string[] descriptions = { $"Thanh toán đủ", $"Tiền dư từ đơn hàng" };

                    for (int i = 0; i < 2; i++)
                    {
                        _dbContext.MoneyFlowLogs.Add(new MoneyFlowLog()
                        {
                            Id = Guid.NewGuid().ToString("N").Substring(0, 12).ToUpper(),
                            UserId = order.UserID,
                            OrderId = order.ID,
                            RelatedId = relatedIds[i],
                            Type = types[i],
                            Amount = amounts[i],
                            PaidAmount = evt.AmountReceive,
                            BalanceSnapshot = balanceSnapshots[i],
                            AffectBalance = isAffectBalance[i],
                            Description = descriptions[i],
                            CreatedAt = DateTime.UtcNow,
                        });
                    }
                }
                else
                {
                    _dbContext.MoneyFlowLogs.Add(new MoneyFlowLog()
                    {
                        Id = Guid.NewGuid().ToString("N").Substring(0, 12).ToUpper(),
                        UserId = order.UserID,
                        OrderId = order.ID,
                        RelatedId = underPaid != null
                                            ? underPaid.Id
                                            : $"paymentId_{payment?.ID}",
                        Type = underPaid != null
                                        ? "UnderpaidCreated"
                                        : checkExcessPayment ? "WalletCompleted" : "PaymentCompleted",

                        Amount = underPaid != null
                                        ? underPaid.PaidAmount
                                        : order.Amount ?? 0,

                        PaidAmount = evt.AmountReceive,
                        BalanceSnapshot = userWalletExist.Balance,
                        AffectBalance = checkExcessPayment ? true : false,
                        Description = underPaid != null
                                        ? $"Thanh toán thiếu từ đơn hàng"
                                        : checkExcessPayment ? $"Thanh toán bù từ ví" : $"Thanh toán đủ",

                        CreatedAt = DateTime.UtcNow,
                    });
                }

                _dbContext.Attach(order);
                order.Status = underPaid != null ? "PartiallyPaid" : "Paid";
                order.PaidAt = underPaid != null ? null : DateTime.UtcNow;
                _dbContext.Orders.Update(order);

                foreach (var item in order.OrderDetail)
                {
                    var affectRow = await _dbContext.Products
                   .Where(p => p.ID == item.ProductID && p.Quantity >= item.Quantity)
                   .ExecuteUpdateAsync(s => s.SetProperty(p => p.Quantity, p => p.Quantity - item.Quantity));

                    if (affectRow > 0)
                    {
                        await _dbContext.Carts
                        .Where(p => p.UserID == evt.Order.UserId && p.ProductID == item.ProductID && p.IsDelete)
                        .ExecuteDeleteAsync();
                    }
                }

                await _dbContext.SaveChangesAsync();
                await transactions.CommitAsync();

                if (excess != null || payment != null)
                {
                    await _event.PublishAsync(new OrderPaidEvent(orderId: order.ID, userId: order.UserID.ToString(), transactionId: order.TransactionId, true));
                }
                else await _event.PublishAsync(new OrderPaidEvent(orderId: order.ID, userId: order.UserID.ToString(), transactionId: order.TransactionId, false));

                return Result<Unit>.OK("Thanh toán thành công.", 201);
            }
            catch (Exception ex)
            {
                await transactions.RollbackAsync();
                return Result<Unit>.Fail(ex.Message, 500);
            }
        }

        public async Task<Result<GeneralOrderViewModel>> ResponsePayStatusAsync(string orderId, string transactionCode, int userId)
        {
            try
            {
                if (string.IsNullOrEmpty(orderId) || string.IsNullOrEmpty(transactionCode))
                {
                    return Result<GeneralOrderViewModel>.Fail("Dữ liệu không hợp lệ!", 400, null);
                }

                var order = await _dbContext.Orders
                    .Include(p => p.OrderDetail).ThenInclude(p => p.Product)
                    .FirstOrDefaultAsync(p => p.ID == orderId && p.TransactionId == transactionCode && p.Status == "Paid") ?? null;

                if (order == null)
                {
                    return Result<GeneralOrderViewModel>.Fail("Đơn hàng không tồn tại!", 404, null);
                }

                if (userId > 0)
                {
                    var Address = await _dbContext.Addresses.FirstOrDefaultAsync(p => p.UserID == userId && p.IsDefault);

                    order.AddressID = Address.ID;
                    order.ShipRecipientName = Address.RecipientName;
                    order.ShipPhoneNumber = Address.PhoneNumber;
                    order.ShipStreet = Address.Street;
                    order.ShipProvince = Address.Province;
                    order.ShipWard = Address.Ward;

                    _dbContext.Orders.Update(order);
                    await _dbContext.SaveChangesAsync();
                }

                var cateID = await _dbContext.OrderDetail.Where(p => p.OrderID == order.ID).Select(p => p.Product.CateID).FirstOrDefaultAsync();

                var product = await _dbContext.Products.Where(p => p.CateID == cateID).ToListAsync();

                Random rand = new Random();
                var shuffledProduct = product.OrderBy(p => rand.Next()).Take(4).ToList();


                GeneralOrderViewModel viewModel = new ()
                {
                    Product = shuffledProduct,
                    Order = order
                };

                return Result<GeneralOrderViewModel>.OK("trạng thái thanh toán thành công", 201, viewModel);
            }
            catch
            {
                return Result<GeneralOrderViewModel>.Fail("Lỗi hệ thống khi trả về trạng thái thanh toán!", 500);
            }
        }

        private async Task<bool> CheckExcessPaymentAsync(decimal? receivePay, decimal? orderAmount, int userId)
        {
            // tien ck = tien don hang
            if (receivePay >= orderAmount) return false;

            var PaymentAmount = receivePay - orderAmount; // PaymentAmount: chuyen thieu
            if (PaymentAmount < 0M)
            {
                // xu ly thieu tien, lay tien du trong tui tru di tien thieu
                var walletAmount = await _dbContext.UserWallets.Where(p => p.UserId == userId).SumAsync(p => p.Balance);

                // kiem tra xem thieu trong tui du tien tru tien bi thieu ko
                return (walletAmount + PaymentAmount) > 0;
            }
            return false;
        }


    }
}
