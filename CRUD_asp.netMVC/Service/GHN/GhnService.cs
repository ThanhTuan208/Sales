using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.DTO.Generic;
using CRUD_asp.netMVC.DTO.Order.GHN;
using CRUD_asp.netMVC.Models.Auth;
using CRUD_asp.netMVC.Models.Order;
using Microsoft.AspNetCore.Components.Forms.Mapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Text.Json;
using System.Threading.Tasks;
using UsersAuth = CRUD_asp.netMVC.Models.Auth.Users;

namespace CRUD_asp.netMVC.Service.GHN
{
    public class GhnService : IGhnService
    {
        //private readonly string _token;
        //private readonly string _baseURL;

        private readonly HttpClient _httpClient;
        private readonly AppDBContext _dbContext;
        private readonly UserManager<UsersAuth> _userManager;
        private readonly IWebHostEnvironment _env;

        public GhnService(HttpClient httpClient, AppDBContext dbContext, UserManager<UsersAuth> userManager, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _httpClient = httpClient;
            _env = env;
        }

        public async Task<Result<string>> CreateOrderGHNRequestAsync(string orderId, int userId)
        {
            try
            {
                Address userAddress = null!;
                Address adminAddress = null!;

                var getDataAdmin = await _userManager.GetUsersInRoleAsync("Admin");
                var admin = getDataAdmin.FirstOrDefault(p => p.Email.StartsWith("23211TT1243") && p.RoleId == 1);

                if (admin == null)
                {
                    return Result<string>.Fail("Không tìm thấy chủ cửa hàng!", 500, null);
                }

                if (admin.RoleId != 1)
                {
                    userAddress = await _dbContext.Addresses.FirstOrDefaultAsync(p => p.IsDefault) ?? new Address();
                }
                else adminAddress = await _dbContext.Addresses.FirstOrDefaultAsync(p => p.UserID == admin.Id && p.IsDefault) ?? new Address();

                if (adminAddress == null)
                {
                    return Result<string>.Fail("Chủ cửa hàng chưa có địa chỉ!", 500, null);
                }

                if (userAddress == null)
                {
                    return Result<string>.Fail("Người dùng chưa có địa chỉ!", 500, null);
                }

                var getWardNameUser = GetWardJsonData(userAddress.GovernmentCode);
                var getWardNameAdmin = GetWardJsonData(adminAddress.GovernmentCode);

                if (string.IsNullOrEmpty(getWardNameAdmin) || string.IsNullOrEmpty(getWardNameUser))
                {
                    return Result<string>.Fail("Địa chỉ phường không tồn tại!", 500, null);
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

                var weight = order.OrderDetail.Sum(p => p.Product.Weight);

                var ghnRequest = new CreateOrderGHNRequestDTO
                {
                    PaymentTypeID = order.Amount > 1000000m ? 1 : 2, // 1: nguoi ban tra phi van chuyen, 2: nguoi nhan tra phi
                    ShopId = 2510403, // ma shop
                    RequiredNote = "KHONGCHOXEMHANG",

                    //FromName = "Nguyễn Thành Tuấn", // cmt lay ten mac dinh tren he thong
                    FromPhone = admin.PhoneNumber ?? string.Empty,
                    FromAddressUser = $"{adminAddress.Street}\n{adminAddress.Ward}\n{adminAddress.Province}",
                    FromWardName = getWardNameAdmin,
                    FromProvinceName = adminAddress.Province,
                    //FromDistrictName = ,

                    ToName = $"{order.Users?.LastName} {order.Users?.FirstName}",
                    ToPhone = order.Users?.PhoneNumber ?? string.Empty,
                    ToAddress = $"{order.Address?.Street}, {order.Address?.Ward}, {order.Address?.Province}",
                    ToWardName = getWardNameUser,
                    ToProvinceName = userAddress.Province,
                    //ToDistrictName = "2152",

                    Weight = weight > 50000 ? null : weight, // Can nang tong cac san pham trong don hang
                    CodAmount = 0, // chua xu ly tien thu ho
                    CodFailedAmount = 0,
                    ServiceID = 53320, // Mã dịch vụ giao hàng
                    ServiceTypeID = 1, // 1: Giao hang tieu chuan, 2: giao hang nhanh
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
                var response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}/v2/shipping-order/create", content);

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

        // Lay phuong,xa tu data json
        private string? GetWardJsonData(string governmentCode)
        {
            List<DistrictGHN> districts = LoadDistrictsFromJson();
            return districts.SelectMany(p => p.Wards).FirstOrDefault(p => p.Code == governmentCode)?.FullName;
        }

        // Lay json data tinh thanh sau sap nhap
        private List<DistrictGHN> LoadDistrictsFromJson()
        {
            var path = Path.Combine(_env.WebRootPath, "full_json_generated_data_vn_units.json");

            var json = File.ReadAllText(path);

            return JsonSerializer.Deserialize<List<DistrictGHN>>(json) ?? new List<DistrictGHN>();
        }

        // Lay Tinh tu api GHN
        private async Task<ProvinceGHN?> GetProvinceGHNApiAsync(string provinceName)
        {
            var response = await _httpClient.GetAsync($"master-data/province");
            response.EnsureSuccessStatusCode();

            var resProvince = await response.Content.ReadFromJsonAsync<GHNApiResponse<List<ProvinceGHN>>>();
            if (resProvince == null || resProvince.Code != 200)
            {
                return null;
            }

            return resProvince.Data.FirstOrDefault(p => p.ProvinceName.Equals(provinceName, StringComparison.OrdinalIgnoreCase) ||
                (p.NameExtension != null && p.NameExtension.Any(x => x.Equals(provinceName, StringComparison.OrdinalIgnoreCase))));
        }

        // Lay Huyen tu api GHN
        private async Task<List<DistrictGHN>?> GetDistrictNameByProvinceAsync(int provinceId)
        {
            var response = await _httpClient.GetAsync($"master-data/district?province_id={provinceId}");
            response.EnsureSuccessStatusCode();

            var resDistrict = await response.Content.ReadFromJsonAsync<GHNApiResponse<List<DistrictGHN>>>();
            if (resDistrict == null || resDistrict.Code != 200)
            {
                return null;
            }

            //return resDistrict.Data.FirstOrDefault(p => p.ProvinceID == provinceId)?.DistrictName;
            return resDistrict.Data ?? null;
        }
    }
}
