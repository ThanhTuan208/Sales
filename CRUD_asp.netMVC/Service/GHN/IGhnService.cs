using CRUD_asp.netMVC.Common;
using CRUD_asp.netMVC.DTO.Generic;
using CRUD_asp.netMVC.DTO.Order.GHN;
using CRUD_asp.netMVC.Models.Order;

namespace CRUD_asp.netMVC.Service.GHN
{
    public interface IGhnService
    {
        Task<OrderStatusGHNResponse> GetOrderStatusAsync(string orderCode);
        Task<Result<string>> CreateOrderGHNRequestAsync(string orderId, int userId);
    }
}
