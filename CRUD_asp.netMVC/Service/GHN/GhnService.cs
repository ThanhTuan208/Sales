using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.DTO.Order.GHN;
using CRUD_asp.netMVC.Models.Order;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CRUD_asp.netMVC.Service.GHN
{
    public class GhnService : IGhnService
    {
        private readonly string _token;
        private readonly string _baseURL;
        private readonly HttpClient _httpClient;
        private readonly AppDBContext _dbContext;

        public GhnService(IConfiguration config, HttpClient httpClient, AppDBContext dbContext)
        {
            _token = config["GHN:Token"] ?? "";
            _baseURL = config["GHN:Base_URL"] ?? "";
            _dbContext = dbContext;
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("Token", _token);
            _httpClient.DefaultRequestHeaders.Add("_dbContext-Type", "application/json");
        }

        [HttpPost] // Tao don hang cho GHN sau khi thanh toan thanh cong (thuc hien control Cart)
        public async Task<string> CreateShippingOrderAsync(string orderID, CreateOrderGHNRequest GHNRequest)
        {
            //GHNRequest.ToDistrictID = 0;

            var json = JsonSerializer.Serialize(GHNRequest);
            Console.WriteLine(json);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_baseURL}/v2/shipping-order/create", content);

            var body = await response.Content.ReadAsStringAsync();
            Console.WriteLine(body);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<CreateOrderGHNResponse>();
            var orderCode = result?.data.OrderCode ?? throw new Exception("Failed to create order with GHN");

            var order = await _dbContext.Orders.FindAsync(orderID);
            if (order != null)
            {
                order.TrackingNumber = orderCode; // Ma van don
                order.Status = OrderStatus.Confirmed.ToString();
                order.StatusTime = DateTime.Now;

                await _dbContext.SaveChangesAsync();
            }

            return orderCode;
        }


        // Doc du lieu tu order_code GHN
        public async Task<OrderStatusGHNResponse> GetOrderStatusAsync(string orderCode)
        {
            var response = await _httpClient.GetAsync($"{_baseURL}/v2/shipping-order/detail?order_code={orderCode}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<OrderStatusGHNResponse>();
        }
    }
}
