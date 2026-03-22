using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.DTO.Generic;
using CRUD_asp.netMVC.DTO.Order.GHN;
using CRUD_asp.netMVC.Models.Addresses;
using CRUD_asp.netMVC.Service.GHN.Api;
using CRUD_asp.netMVC.Service.GHN.Fil;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UsersAuth = CRUD_asp.netMVC.Models.Auth.Users;

namespace CRUD_asp.netMVC.Service.GHN
{
    public class GhnService : IGhnService
    {
        private readonly ICallAPI _api;
        private readonly IFilterData _filter;
        private readonly AppDBContext _dbContext;
        private readonly ILogger<GhnService> _logger;
        private readonly UserManager<UsersAuth> _userManager;

        public GhnService(ICallAPI api, IFilterData filter, AppDBContext dbContext, ILogger<GhnService> logger, UserManager<UsersAuth> userManager)
        {
            _api = api;
            _filter = filter;
            _dbContext = dbContext;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<Result<string>> CreateOrderGHNRequestAsync(string orderId, int userId)
        {
            try
            {
                int shopId = 197537;
                Address? userAddress = null;
                Address? adminAddress = null;

                var address = _dbContext.Addresses;

                var getDataAdmin = await _userManager.GetUsersInRoleAsync("Admin");
                var admin = getDataAdmin.FirstOrDefault(p => p.Email.StartsWith("23211TT1243") && p.RoleId == 1);

                if (admin == null)
                {
                    return Result<string>.Fail("Không tìm thấy chủ cửa hàng!", 400, null);
                }

                adminAddress = await address.FirstOrDefaultAsync(p => p.UserID == admin.Id && p.IsDefault);
                userAddress = await address.FirstOrDefaultAsync(p => p.UserID == userId && p.IsDefault);

                if (adminAddress == null || userAddress == null)
                {
                    return Result<string>.Fail("Chủ cửa hàng hoặc người dùng chưa có địa chỉ!", 400, null);
                }

                var districtNameAdmin = _filter.GetDistrictNameByDistrictGHN(adminAddress.GovernmentCode);
                var districtNameUser = _filter.GetDistrictNameByDistrictGHN(userAddress.GovernmentCode);

                if (districtNameAdmin == null || districtNameUser == null)
                {
                    return Result<string>.Fail("Không tim thấy tên huyện của người dùng!", 400, null);
                }

                var districtIdAdmin = _filter.GetDistrictIdByDistrictGHN(adminAddress.GovernmentCode);
                var districtIdUser = _filter.GetDistrictIdByDistrictGHN(userAddress.GovernmentCode);

                if (districtIdAdmin == null || districtIdUser == null)
                {
                    return Result<string>.Fail("Không tim thấy huyện của người dùng!", 400, null);
                }

                var wardCodeAdmin = _filter.GetWardGHNByGovernmentPara(adminAddress.GovernmentCode);
                var wardCodeUser = _filter.GetWardGHNByGovernmentPara(userAddress.GovernmentCode);

                if (string.IsNullOrEmpty(wardCodeUser.WardCode) || string.IsNullOrEmpty(wardCodeAdmin.WardCode))
                {
                    return Result<string>.Fail("Không tim thấy xã/phường của người dùng!", 400, null);
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
                if (weight > 50000) // gram
                {   
                    return Result<string>.Fail("Đơn hàng vượt quá 50kg!", 400, null);
                }

                var service = await _api.GetServiceGHNAsync(shopId, weight, districtIdAdmin, districtIdUser);
                var serviceData = service.Data;

                if (serviceData == null)
                {
                    return Result<string>.Fail("Lỗi dịch vụ GHN!", 500, null);
                }

                var ghnRequest = new CreateOrderGHNRequestDTO
                {
                    ShopId = shopId,
                    RequiredNote = "CHOXEMHANGKHONGTHU",
                    PaymentTypeID = order.Amount > 1000000m ? 1 : 2,
                    // 1: nguoi ban tra phi van chuyen, 2: nguoi nhan tra phi

                    //FromName = "Nguyễn Thành Tuấn", // cmt lay ten mac dinh tren he thong
                    FromName = "LiFashion",
                    FromPhone = admin.PhoneNumber ?? string.Empty,
                    FromAddressUser = $"{adminAddress.Street}\n{adminAddress.Ward}\n{adminAddress.Province}",
                    FromWardName = adminAddress.Ward,
                    FromProvinceName = adminAddress.Province,
                    FromWardCode = wardCodeAdmin.WardCode,
                    FromDistrictName = districtNameAdmin,
                    FromDistrictID = districtIdAdmin,

                    ToName = $"{order.Users?.LastName} {order.Users?.FirstName}",
                    ToPhone = order.Users?.PhoneNumber ?? string.Empty,
                    ToAddress = $"{order.Address?.Street}, {order.Address?.Ward}, {order.Address?.Province}",
                    ToWardName = userAddress.Ward,
                    ToProvinceName = userAddress.Province,
                    ToWardCode = wardCodeUser.WardCode,
                    ToDistrictName = districtNameUser,
                    ToDistrictID = districtIdUser,

                    CodAmount = 0, // chua xu ly tien thu ho
                    CodFailedAmount = 0,
                    Weight = weight > 50000 ? 0 : weight, // Can nang tong cac san pham trong don hang
                    ServiceID = serviceData.Id, // Mã dịch vụ giao hàng
                    ServiceTypeID = serviceData.TypeId, // 1: Giao hang tieu chuan, 2: giao hang nhanh

                    Items = order.OrderDetail.Select(p => new ProductItem
                    {
                        Name = p.Product.Name ?? string.Empty,
                        Code = p.Product.ID.ToString(),
                        Quantity = p.Quantity,
                        Price = Convert.ToInt32(p.Product.NewPrice),
                        Length = 50,
                        Width = 10,
                        Height = 10,
                        Weight = Math.Clamp(p.Product.Weight ?? 100, 1, 5000)
                    }).ToList(),
                };

                var createShippingOrder = await _api.CreateShippingOrderAsync(order.ID, ghnRequest);

                if (createShippingOrder == null)
                {
                    return Result<string>.Fail("Gửi yêu cầu cho đơn vị vận chuyển không thành công!", 502, null);
                }

                return Result<string>.OK("Gửi yêu cầu cho đơn vị vận chuyển thành công!", 201, createShippingOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi hệ thống khi tạo đơn hàng GHN");
                return Result<string>.Fail("Lỗi hệ thống khi tạo đơn GHN", 500, ex.Message);
            }
        }
    }
}
