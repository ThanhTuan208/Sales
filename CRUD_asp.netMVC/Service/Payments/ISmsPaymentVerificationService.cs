using CRUD_asp.netMVC.DTO.Payment;

namespace CRUD_asp.netMVC.Service.Payments
{
    public interface ISmsPaymentVerificationService
    {
        //Task<bool> CheckPaymentAsync(string orderId);
        Task<ResultDTO> ProcessResultAsync(string message);
    }
}
