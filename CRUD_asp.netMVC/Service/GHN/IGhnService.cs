using CRUD_asp.netMVC.DTO.Order.GHN;

namespace CRUD_asp.netMVC.Service.GHN
{
    public interface IGhnService
    {
        Task<string> CreateShippingOrderAsync(string orderID, CreateOrderGHNRequest GHNRequest);
        Task<OrderStatusGHNResponse> GetOrderStatusAsync(string orderCode);
    }
}
