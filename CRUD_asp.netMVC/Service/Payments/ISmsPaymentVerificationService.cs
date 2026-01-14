using CRUD_asp.netMVC.Common;
using CRUD_asp.netMVC.DTO.Generic;
using CRUD_asp.netMVC.DTO.Payments;
using CRUD_asp.netMVC.ViewModels.Order;

namespace CRUD_asp.netMVC.Service.Payments
{
    public interface ISmsPaymentVerificationService
    {
        Task<Result<Unit>> ProcessResultAsync(string message);
        Task<Result<Unit>> UserConfirmWalletAsync(PaymentVerificationEvent evt);
        Task<Result<GeneralOrderViewModel>> ResponsePayStatusAsync(string orderID, string transactionCode, int userId);
    }
}
