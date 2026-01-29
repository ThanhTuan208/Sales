using CRUD_asp.netMVC.Common;
using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.DTO.Generic;
using CRUD_asp.netMVC.DTO.Order.GHN;
using CRUD_asp.netMVC.Models.Order;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Org.BouncyCastle.Security.Certificates;
using System.Diagnostics;
using System.Text.Json;
using UsersAuth = CRUD_asp.netMVC.Models.Auth.Users;

namespace CRUD_asp.netMVC.Service.GHN
{
    public class GhnService : IGhnService
    {
        private readonly string _token;
        private readonly string _baseURL;

        private readonly HttpClient _httpClient;
        private readonly AppDBContext _dbContext;
        private readonly UserManager<UsersAuth> _userManager;

        public GhnService(IConfiguration config, HttpClient httpClient, AppDBContext dbContext, UserManager<UsersAuth> userManager)
        {
            _userManager = userManager;

            _token = config["GHN:Token"] ?? "";
            _baseURL = config["GHN:Base_URL"] ?? "";
            _dbContext = dbContext;
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("Token", _token);
            _httpClient.DefaultRequestHeaders.Add("_dbContext-Type", "application/json");
        }

        public async Task<Result<string>> CreateOrderGHNRequestAsync(string orderId, int userId)
        {
            try
            {
                var admins = await _userManager.GetUsersInRoleAsync("Admin");
                var admin = admins.FirstOrDefault(p => p.Email.StartsWith("nguyenthanhtuankrp1"));
                if (admin == null)
                {
                    return Result<string>.Fail("Không tìm thấy chủ cửa hàng!", 500, null);
                }

                var adminAddress = await _dbContext.Addresses.FirstOrDefaultAsync(p => p.UserID == admin.Id);
                if (adminAddress == null)
                {
                    return Result<string>.Fail("Chủ cửa hàng chưa có địa chỉ!", 500, null);
                }

                var order = await _dbContext.Orders
                    .Include(p => p.Address)
                    .Include(p => p.Users)
                    .Include(p => p.OrderDetail)
                    .ThenInclude(p => p.Product)
                    .FirstOrDefaultAsync(p => p.ID == orderId && p.UserID == userId) ?? null;

                if (order == null)
                {
                    return Result<string>.Fail("Đơn hàng không tồn tại!", 500, null);
                }

                var ghnRequest = new CreateOrderGHNRequestDTO
                {
                    PaymentTypeID = order.Amount > 1000000m ? 1 : 2, // 1: nguoi ban tra phi van chuyen, 2: nguoi nhan tra phi
                    ShopId = 2510403, // ma shop
                    RequiredNote = "KHONGCHOXEMHANG",

                    //FromName = "Nguyễn Thành Tuấn", cmt lay ten mac dinh tren he thong
                    FromPhone = admin.PhoneNumber ?? string.Empty,
                    FromAddressUser = $"{adminAddress.Street}\n{adminAddress.Ward}\n{adminAddress.Province}",
                    FromProvinceName = adminAddress.Province,
                    //FromDistrictName = ,
                    FromWardName = adminAddress.Ward,

                    ToName = $"{order.Users.LastName} {order.Users.FirstName}",
                    ToPhone = order.Users.PhoneNumber,
                    ToAddress = $"{order.Address.Street}, {order.Address.Ward}, {order.Address.Province}",
                    Weight = order.OrderDetail.Sum(p => p.Product.Weight),
                    ServiceID = 53320, // Mã dịch vụ giao hàng
                    ServiceTypeID = 1, // 1: Giao hang tieu chuan, 2: giao hang nhanh
                    ToDistrictID = 2152,
                    ToWardCode = "381101",
                    CodAmount = 0,
                    ConfigFeeID = 1,
                    ExstraCodeID = 0,


                    Items = order.OrderDetail.Select(p => new ProductItem
                    {
                        Name = p.Product.Name,
                        Code = p.Product.ID.ToString(),
                        Quantity = p.Quantity,
                        Price = (int)p.Product.NewPrice,
                        Length = 50,
                        Width = 200,
                        Height = 210,
                        Weight = p.Product.Weight ?? 0
                    }).ToList(),
                };

                var createShippingOrder = await CreateShippingOrderAsync(order.ID, ghnRequest);

                if (createShippingOrder == null)
                {
                    return Result<string>.Fail("Gửi yêu cầu cho đơn vị vận chuyển không thành công!", 502, null);
                }

                return Result<string>.OK("Gửi yêu cầu cho đơn vị vận chuyển thành công!", 201, createShippingOrder);
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
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_baseURL}/v2/shipping-order/create", content);

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


        // Doc du lieu tu order_code GHN
        public async Task<OrderStatusGHNResponse> GetOrderStatusAsync(string orderCode)
        {
            var response = await _httpClient.GetAsync($"{_baseURL}/v2/shipping-order/detail?order_code={orderCode}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<OrderStatusGHNResponse>() ?? new OrderStatusGHNResponse();
        }

        //public async Task<int> GetDistrictFromWardAsync(string ward)
        //{
        //    var response = await _httpClient.GetAsync($"{_baseURL}/master-data/ward?district_id={ward}");
        //    response.EnsureSuccessStatusCode();

        //    return await response.Content.ReadFromJsonAsync<OrderStatusGHNResponse>() ?? new OrderStatusGHNResponse();
        //}
    }
}
