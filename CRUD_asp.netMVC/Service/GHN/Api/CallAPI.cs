using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.DTO.Order.GHN;
using CRUD_asp.netMVC.Models.Order;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CRUD_asp.netMVC.Service.GHN.Api
{
    public class CallAPI : ICallAPI
    {
        private readonly AppDBContext _dbContext;
        private readonly HttpClient _httpClient;

        public CallAPI(IHttpClientFactory httpClient, AppDBContext dbContext)
        {
            _httpClient = httpClient.CreateClient("GHN");
            _dbContext = dbContext;
        }

        // Tao don hang cho GHN sau khi thanh toan thanh cong (thuc hien control Cart
        public async Task<string?> CreateShippingOrderAsync(string orderID, CreateOrderGHNRequestDTO GHNRequest)
        {
            try
            {
                var json = JsonSerializer.Serialize(GHNRequest);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"v2/shipping-order/create", content);

                var body = await response.Content.ReadAsStringAsync();
                //Console.WriteLine(body);

                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<CreateOrderGHNResponse>();
                var orderCode = result?.data.OrderCode ?? throw new Exception("Failed to create order with GHN");

                var order = await _dbContext.Orders.FindAsync(orderID);
                if (order != null)
                {
                    order.TrackingNumber = orderCode;
                    order.Status = OrderStatus.Confirmed.ToString();
                    order.StatusTime = DateTime.Now;

                    await _dbContext.SaveChangesAsync();
                }

                return orderCode;
            }

            catch { return null; }
        }

        public async Task<GHNApiResponse<ServiceGHN?>> GetServiceGHNAsync(int shopId, int? weight, int? fromDistrict, int? toDistrict)
        {
            if (!fromDistrict.HasValue || !toDistrict.HasValue || weight > 50000)
            {
                return new ()
                {
                    Code = 400,
                    Message = "giá trị đầu vào không hợp lệ!",
                    Data = null
                };
            }
          
            var response = await _httpClient.GetAsync(
                $"v2/shipping-order/available-services" +
                $"?shop_id={shopId}" +
                $"&from_district={fromDistrict}" +
                $"&to_district={toDistrict}");

            response.EnsureSuccessStatusCode();

            var resServideId = await response.Content.ReadFromJsonAsync<GHNApiResponse<List<ServiceGHN>>>();

            if (resServideId == null || resServideId.Code != 200)
            {
                return new ()
                {
                    Code = resServideId.Code,
                    Message = resServideId.Message,
                    Data = null
                };
            }

            int serviceTypeId = weight < 20000 ? 2 : 5;

            var data = resServideId.Data.FirstOrDefault(p => p.TypeId == serviceTypeId);

            if (data == null)
            {
                return new ()
                {
                    Code = resServideId.Code,
                    Message = resServideId.Message,
                    Data = null
                };
            }

            return new ()
            {
                Code = resServideId.Code,
                Message = resServideId.Message,
                Data = data
            };
        }
    }
}
