using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.DTO.Address;
using CRUD_asp.netMVC.Models.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.View;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Tls;
using System.Security.Claims;

namespace CRUD_asp.netMVC.Controllers
{
    public class AddressController : Controller
    {
        public readonly AppDBContext _dbContext;

        public AddressController(AppDBContext dbContext)
        {
            _dbContext = dbContext;
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

                var userID = User.Identity.IsAuthenticated ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value) : 0;
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
                        //PostalCode = addressDTO.PostalCode ?? string.Empty,
                        PostalCode = "",
                        IsDefault = addressDTO.IsDefault
                    };

                    var addreesDefaultUser = await _dbContext.Addresses.Where(p => p.UserID == userID && p.ID != addressDTO.ID).ToListAsync();

                    List<Address> userDefault = new List<Address>();
                    foreach (var item in addreesDefaultUser)
                    {
                        item.IsDefault = false;
                        userDefault = new List<Address>() { item };
                    }

                    _dbContext.Addresses.UpdateRange(userDefault);
                    await _dbContext.Addresses.AddAsync(address);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    Json(new
                    {
                        success = false,
                        message = "Vui lòng đăng nhập để thêm địa chỉ !!!",
                    });
                }

                return Json(new
                {
                    success = true,
                    message = "Thêm địa chỉ thành công !!!",
                    //data = address
                });

            }
            catch (Exception)
            {
                return Json(new
                {
                    success = false,
                    message = "Lỗi hệ thống, vui lòng thử lại sau !!!",

                });
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
                        message = "Cập nhật thông tin địa chỉ của bạn !!!",
                        errors = errors
                    });
                }

                var userID = User.Identity.IsAuthenticated ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value) : 0;
                if (userID > 0)
                {
                    var AddressExists = await _dbContext.Addresses.FindAsync(addressDTO.ID);

                    if (AddressExists == null)
                    {
                        Json(new
                        {
                            success = false,
                            message = "Không tìm thấy địa chỉ của bạn !!!",
                        });
                    }

                    AddressExists.RecipientName = addressDTO.RecipientName;
                    AddressExists.PhoneNumber = addressDTO.PhoneNumber;
                    AddressExists.Street = addressDTO.Street;
                    AddressExists.Province = addressDTO.Province;
                    AddressExists.Ward = addressDTO.Ward;
                    //PostalCode = addressDTO.PostalCode ?? string.Empty,
                    AddressExists.PostalCode = "";
                    AddressExists.IsDefault = addressDTO.IsDefault;

                    var addreesDefaultUser = await _dbContext.Addresses.Where(p => p.UserID == userID && p.ID != addressDTO.ID).ToListAsync();

                    List<Address> userDefault = new List<Address>();
                    foreach (var item in addreesDefaultUser)
                    {
                        item.IsDefault = false;
                        userDefault = new List<Address>() { item };
                    }

                    _dbContext.Addresses.UpdateRange(userDefault);

                    _dbContext.Addresses.Update(AddressExists);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    Json(new
                    {
                        success = false,
                        message = "Vui lòng đăng nhập để thêm địa chỉ !!!",
                    });
                }

                return Json(new
                {
                    success = true,
                    message = "Cập nhật địa chỉ thành công !!!",

                });

            }
            catch (Exception)
            {
                return Json(new
                {
                    success = false,
                    message = "Lỗi hệ thống, vui lòng thử lại !!!",

                });
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAddress(int addressId)
        {
            try
            {
                if (addressId < 1)
                {
                    return Json(new
                    {
                        success = false,
                        message = $"Không tìm thấy id = {addressId} !!!",

                    });
                }

                var address = await _dbContext.Addresses.FindAsync(addressId);

                if (address == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = $"Không tìm thấy id = {addressId} trong dữ liệu !!!",

                    });
                }

                _dbContext.Addresses.Remove(address);
                await _dbContext.SaveChangesAsync();

                return Json(new
                {
                    success = true,
                    message = $"Xóa địa chỉ thành công. ",

                });
            }
            catch (Exception ex)
            {

                return Json(new
                {
                    success = false,
                    message = "Server: " + ex.Message,
                });
            }
        }
    }
}
