using CRUD_asp.netMVC.DTO.Order.GHN;

namespace CRUD_asp.netMVC.Service.GHN.Api
{
    public interface ICallAPI
    {
        Task<string?> CreateShippingOrderAsync(string orderID, CreateOrderGHNRequestDTO GHNRequest);
        Task<GHNApiResponse<ServiceGHN?>> GetServiceGHNAsync(int shopId,int? weight, int? fromDistrict, int? toDistrict);
    }   
}
