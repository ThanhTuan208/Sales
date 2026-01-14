using CRUD_asp.netMVC.Common;
using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.DTO.Generic;
using CRUD_asp.netMVC.DTO.Order.GHN;
using CRUD_asp.netMVC.Models.Order;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
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

        public async Task<Result<string>> CreateOrderGHNRequestAsync(Orders order)
        {
            try
            {
                var ghnRequest = new CreateOrderGHNRequestDTO
                {
                    ShopId = 2510403,
                    ToName = $"{order.Users.LastName} {order.Users.FirstName}",
                    ToPhone = order.Users.PhoneNumber,
                    ToAddress = $"{order.Address.Street}, {order.Address.Ward}, {order.Address.Province}",
                    Weight = order.OrderDetail.Sum(p => p.Product.Weight),
                    ServiceID = 53320, // Mã dịch vụ giao hàng
                    ServiceTypeID = 1, // 1: Giao hang tieu chuan, 2: giao hang nhanh
                    ToDistrictID = 2152,
                    ToWardCode = 381101,
                    CodAmount = 0,
                    PaymentTypeID = 1, // 1: nguoi ban tra phi van chuyen, 2: nguoi nhan tra phi
                    RequiredNote = "KHONGCHOXEMHANG",
                    ConfigFeeID = 1,
                    ExstraCodeID = 0,

                    FromName = "Nguyễn Thành Tuấn",
                    FromPhone = "0358986823",
                    FromAddressUser = "219/4 Linh Xuân, Phường Thủ Đức, Thành phố Hồ Chí Minh",
                    FromDistrictId = 3695, // Thủ Đức (sau tái cấu trúc hành chính)
                    FromWardCode = "90735", // Phường Linh Xuân

                    Items = order.OrderDetail.Select(p => new ProductItem
                    {
                        Name = p.Product.Name,
                        Code = p.Product.ID.ToString(),
                        Quantity = p.Quantity,
                        Price = (int)p.Product.NewPrice,
                        Length = 50,
                        Width = 200,
                        Height = 210,
                        Weight = p.Product.Weight
                    }).ToList(),
                };

                var createShippingOrder = await CreateShippingOrderAsync(order.ID, ghnRequest);
                if (createShippingOrder == null)
                {
                    return Result<string>.Fail("Gửi yêu cầu cho đơn vị vận chuyển không thành công!", 502);
                }

                return Result<string>.OK("Gửi yêu cầu cho đơn vị vận chuyển thành công!", 201, createShippingOrder);
                //order.TrackingNumber = orderCode;
            }
            catch (Exception ex)
            {
                return Result<string>.Fail("Lỗi hệ thống khi tạo đơn GHN", 500, ex.Message);
            }
        }

        // Tao don hang cho GHN sau khi thanh toan thanh cong (thuc hien control Cart)
        private async Task<string?> CreateShippingOrderAsync(string orderID, CreateOrderGHNRequestDTO GHNRequest)
        {
            try
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

            catch { return null; }
        }


        // Doc du lieu tu order_code GHN
        public async Task<OrderStatusGHNResponse> GetOrderStatusAsync(string orderCode)
        {
            var response = await _httpClient.GetAsync($"{_baseURL}/v2/shipping-order/detail?order_code={orderCode}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<OrderStatusGHNResponse>() ?? new OrderStatusGHNResponse();
        }
    }
}
