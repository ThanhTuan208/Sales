using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.DTO.Address;
using CRUD_asp.netMVC.Models.Auth;
using EFCoreSecondLevelCacheInterceptor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Org.BouncyCastle.Bcpg;
using System.Runtime.Loader;
using System.Security.Claims;

namespace CRUD_asp.netMVC.Controllers
{
    public class AddressController : Controller
    {
        private readonly AppDBContext _dbContext;
        private readonly IMemoryCache _cache;

        public AddressController(AppDBContext dbContext, IMemoryCache cache)
        {
            _dbContext = dbContext;
            _cache = cache;
        }

        [HttpPost, ValidateAntiForgeryToken] // Them dia chi moi
        public async Task<IActionResult> AddAddress(AddressDTO addressDTO)
        {
            try
            {
                if (addressDTO == null) return View();

                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                           .Where(ms => ms.Value.Errors.Count > 0)
                           .ToDictionary(
                           kvp => kvp.Key,
                           kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                       );

                    return Json(new
                    {
                        success = false,
                        message = "Nhập thông tin địa chỉ của bạn !!!",
                        errors = errors
                    });
                }

                var userID = User.Identity.IsAuthenticated ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0") : 0;
                if (userID > 0)
                {
                    var address = new Address()
                    {
                        UserID = userID,
                        RecipientName = addressDTO.RecipientName,
                        PhoneNumber = addressDTO.PhoneNumber,
                        Street = addressDTO.Street,
                        Province = addressDTO.Province,
                        Ward = addressDTO.Ward,
                        PostalCode = null,
                        IsDefault = addressDTO.IsDefault,
                        IsDelete = false,
                        //PostalCode = addressDTO.PostalCode ?? string.Empty,
                    };

                    if (addressDTO.IsDefault)
                    {
                        var addreesDefaultUser = await _dbContext.Addresses.Where(p => p.UserID == userID).ToListAsync();

                        List<Address> userDefault = new List<Address>();
                        foreach (var item in addreesDefaultUser)
                        {
                            item.IsDefault = false;
                            userDefault = new List<Address>() { item };
                        }

                        address.IsDefault = addressDTO.IsDefault;
                        _dbContext.Addresses.UpdateRange(userDefault);
                    }

                    _dbContext.Addresses.Update(address);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    Json(new { success = false, message = "Vui lòng đăng nhập để thêm địa chỉ !", });
                }

                var UniqueAddressbyUser = _dbContext.Addresses.Where(p => p.UserID == userID);

                if (UniqueAddressbyUser != null && UniqueAddressbyUser.ToList().Count == 1)
                {
                    return Json(new { success = true, message = "Thêm địa chỉ thành công.", status = "unique" });
                }
                else if (UniqueAddressbyUser != null && UniqueAddressbyUser.ToList().Count > 1)
                {
                    return Json(new { success = true, message = "Thêm địa chỉ thành công.", isDefault = true });
                }

                return Json(new { success = true, message = "Thêm địa chỉ thành công." });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Lỗi thêm địa chỉ !" });
            }
        }

        [HttpPost, ValidateAntiForgeryToken] // Them dia chi moi
        public async Task<IActionResult> UpdateAddress(AddressDTO addressDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                           .Where(ms => ms.Value.Errors.Count > 0)
                           .ToDictionary(
                           kvp => kvp.Key,
                           kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                       );

                    return Json(new
                    {
                        success = false,
                        message = "Cập nhật thông tin địa chỉ của bạn !",
                        errors = errors
                    });
                }

                var userID = User.Identity.IsAuthenticated ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0") : 0;
                if (userID > 0)
                {
                    var AddressExists = await _dbContext.Addresses.FindAsync(addressDTO.ID);

                    if (AddressExists == null)
                    {
                        Json(new { success = false, message = "Không tìm thấy địa chỉ của bạn !" });
                    }

                    AddressExists.RecipientName = addressDTO.RecipientName;
                    AddressExists.PhoneNumber = addressDTO.PhoneNumber;
                    AddressExists.Street = addressDTO.Street;
                    AddressExists.Province = addressDTO.Province;
                    AddressExists.Ward = addressDTO.Ward;
                    AddressExists.PostalCode = null;
                    //PostalCode = addressDTO.PostalCode ?? string.Empty,


                    if (addressDTO.IsDefault)
                    {
                        var addreesDefaultUser = await _dbContext.Addresses.Where(p => p.UserID == userID).ToListAsync();

                        List<Address> userDefault = new List<Address>();
                        foreach (var item in addreesDefaultUser)
                        {
                            item.IsDefault = false;
                            userDefault = new List<Address>() { item };
                        }

                        AddressExists.IsDefault = addressDTO.IsDefault;
                        _dbContext.Addresses.UpdateRange(userDefault);
                    }

                    _dbContext.Addresses.Update(AddressExists);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    Json(new { success = false, message = "Vui lòng đăng nhập để thêm địa chỉ !" });
                }

                var UniqueAddressbyUser = _dbContext.Addresses.Where(p => p.UserID == userID);
                var UniqueAddressbyUserIsNull = UniqueAddressbyUser.FirstOrDefault(p => p.IsDefault);
                //updateQR = UniqueAddressbyUser.ToList().Count > 1 && UniqueAddressbyUserIsNull != null;

                if (!_cache.TryGetValue("countRequest", out int count))
                {
                    count = 0;
                }

                var updateQR = false;
                count = _cache.Get<int>("countRequest");
                if (UniqueAddressbyUserIsNull != null)
                {
                    count++;
                }

                if (UniqueAddressbyUserIsNull != null && count == 0)
                {
                    updateQR = true;
                    count++;
                }
                else
                {
                    updateQR = false;
                    count--;
                }
                _cache.Set("countRequest", count);

                if (UniqueAddressbyUser != null)
                {
                    if (UniqueAddressbyUser.ToList().Count == 1 && UniqueAddressbyUserIsNull == null)
                    {
                        return Json(new { success = true, message = "Cập nhật địa chỉ thành công.", status = "unique" });
                    }
                    else if (UniqueAddressbyUser.ToList().Count > 1 && UniqueAddressbyUserIsNull != null)
                    {
                        return Json(new { success = true, message = "Cập nhật địa chỉ thành công.", isDefault = updateQR });
                    }
                    else if (UniqueAddressbyUser.ToList().Count > 1 && UniqueAddressbyUserIsNull == null)
                    {
                        return Json(new { success = true, message = "Cập nhật địa chỉ thành công.", isDefault = true });
                    }
                    else if (UniqueAddressbyUser.ToList().Count == 1 && UniqueAddressbyUserIsNull != null)
                    {
                        return Json(new { success = true, message = "Cập nhật địa chỉ thành công.", isDefault = true });
                    }
                }

                return Json(new { success = true, message = "Cập nhật địa chỉ thành công." });

            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Lỗi hệ thống, vui lòng thử lại !" });
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAddress(int addressId)
        {
            try
            {
                if (addressId < 1)
                {
                    return Json(new { success = false, message = $"Không tìm thấy id = {addressId} !" });
                }


                var userID = User.Identity.IsAuthenticated ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0") : 0;
                if (userID > 0)
                {
                    var address = await _dbContext.Addresses.FirstOrDefaultAsync(p => p.UserID == userID && p.ID == addressId);

                    var IsAddressExistsOrder = await _dbContext.Orders.FirstOrDefaultAsync(p => p.UserID == userID && p.AddressID == addressId);
                    if (IsAddressExistsOrder != null)
                    {
                        IsAddressExistsOrder.AddressID = null;

                        _dbContext.Orders.Update(IsAddressExistsOrder);
                    }

                    _dbContext.Addresses.Remove(address);
                    await _dbContext.SaveChangesAsync();

                }

                return Json(new { success = true, message = $"Xóa địa chỉ thành công. " });
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = "Server: " + ex.Message });
            }
        }
    }
}
