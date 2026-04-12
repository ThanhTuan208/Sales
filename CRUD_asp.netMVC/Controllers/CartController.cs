using AspNetCoreGeneratedDocument;
using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.DTO.Cart;
using CRUD_asp.netMVC.Extensions.Payments;
using CRUD_asp.netMVC.Extensions.RenderViewGeneral;
using CRUD_asp.netMVC.Models.Addresses;
using CRUD_asp.netMVC.Models.Cart;
using CRUD_asp.netMVC.Models.Order;
using CRUD_asp.netMVC.Models.Product;
using CRUD_asp.netMVC.ViewModels.Cart;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit.Text;
using MimeKit.Tnef;
using NetTopologySuite.Geometries.Prepared;
using Org.BouncyCastle.Bcpg;
using System.Security.Claims;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace CRUD_asp.netMVC.Controllers
{
    [Authorize(Policy = "CustomerOrGuest")]
    public class CartController : Controller
    {
        private readonly AppDBContext _dbContext;
        private readonly QrCodeService _qrCode;

        public CartController(AppDBContext _context, QrCodeService qrCode)
        {
            _dbContext = _context;
            _qrCode = qrCode;
        }

        [HttpGet] // Cap nhat cart isDelete = false
        public async Task<IActionResult> UdpateIsDeleteCart(string[]? arrID, bool IsBuyNow = false)
        {
            try
            {
                var userID = User.Identity.IsAuthenticated ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0") : 0;
                if (userID > 0)
                {
                    var cart = _dbContext.Carts.Where(p => p.UserID == userID);
                    if (await cart.AllAsync(p => !p.IsDelete))
                    {
                        return Json(new { success = false });
                    }

                    await cart.ExecuteUpdateAsync(e => e.SetProperty(s => s.IsDelete, false));
                }

                return Json(new { success = true });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> CheckAddressData(string[]? arrID, bool IsBuyNow = false)
        {
            try
            {
                var viewModel = await GeneralIndex(arrID);

                var userID = User.Identity.IsAuthenticated ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0") : 0;
                if (userID > 0)
                {
                    var address = await _dbContext.Addresses.FirstOrDefaultAsync(p => p.UserID == userID && p.IsDefault);

                    if (address == null)
                    {
                        return PartialView("_ModalPaymentPartialRight", viewModel);
                    }
                }

                return Json(new { success = true, message = $"Có dữ liệu. " });
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = $"Server: " + ex.Message });
            }
        }

        // Su dung method chung cho Index (get, post)
        public async Task<CartViewModel> GeneralIndex(
            string[]? arrID,
            int? productId = null,
            string? color = null,
            string? size = null,
            int qty = 1,
            double? price = null)
        {
            try
            {
                var cartItems = new List<AddToCart>();
                var cartItemByIDs = new List<AddToCart>();
                var addressUser = new List<Address>();
                var validBuyNow = new CartValidationResult();

                var userID = User.Identity.IsAuthenticated ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0") : 0;
                if (userID > 0)
                {

                    cartItems = await _dbContext.Carts
                        .AsNoTracking()
                        .Include(c => c.Product)
                        .Include(c => c.Product).ThenInclude(p => p.ProductColor)
                        .Include(c => c.Product).ThenInclude(p => p.ProductSize)
                        .Include(c => c.Product).ThenInclude(p => p.ProductImages)
                        .Include(c => c.Product).ThenInclude(p => p.Cate)
                        .Include(c => c.Users).ThenInclude(p => p.Addresses)
                        .Where(c => c.UserID == userID)
                        .ToListAsync();

                    var arrIDSet = arrID?.Select(int.Parse).ToHashSet() ?? new HashSet<int>();

                    cartItemByIDs = cartItems.Where(p => arrIDSet.Contains(p.ID)).ToList() ?? [];

                    addressUser = await _dbContext.Addresses.Where(p => p.UserID == userID).ToListAsync() ?? [];

                    validBuyNow = new()
                    {
                        ProductId = productId,
                        Size = size,
                        Color = color,
                        Quantity = qty,
                        Price = price,
                        Product = await _dbContext.Products.AsNoTracking()
                                                            .Include(p => p.ProductImages)
                                                            .FirstOrDefaultAsync(p => p.ID == productId)
                    };
                }
                else
                {
                    var cart = HttpContext.Session.GetString("Cart");
                    cartItems = string.IsNullOrEmpty(cart) ? new List<AddToCart>() : JsonSerializer.Deserialize<List<AddToCart>>(cart);

                    if (cartItems != null && cartItems.Any())
                    {
                        var getIDProductByCart = cartItems.Select(p => p.ProductID);
                        var products = await _dbContext.Products.Where(p => getIDProductByCart.Contains(p.ID)).ToListAsync();

                        if (products.Any())
                        {
                            foreach (var item in cartItems)
                            {
                                item.Product = products.FirstOrDefault(p => p.ID == item.ProductID);
                            }
                        }
                        else cartItems.Clear();
                    }
                }

                CartViewModel viewModel = new()
                {
                    CartItems = cartItems,
                    AddressUser = addressUser,
                    CartItemByIDs = cartItemByIDs,
                    ValidBuyNow = validBuyNow,
                    TotalPrice = cartItems.Sum(p => p.Product != null ? p.Product.NewPrice * p.Quantity : 0)
                };

                return viewModel;
            }
            catch (Exception)
            {
                return new CartViewModel();
            }
        }

        [HttpGet, Route("Cart")] // Hien thi gio hang
        public async Task<IActionResult> Index(
            string[]? arrID,
            int? productId,
            string color,
            string size,
            int qty,
            double price,
            bool IsBuyNow = false,
            bool IsAddress = false,
            bool UpdateAddress = false)
        {
            try
            {
                var viewModel = await GeneralIndex(arrID, productId, color, size, qty, price);

                if (viewModel.CartItemByIDs.Count > 0 || IsBuyNow)
                {
                    if (IsAddress)
                    {
                        if (UpdateAddress)
                        {
                            return PartialView("_EditAddressPartial", viewModel);
                        }
                        else return PartialView("_ListAddressPartial", viewModel);
                    }
                    else return PartialView("_ModalPaymentPartial", viewModel);
                }
                else return View(viewModel);
            }
            catch (Exception)
            {
                return View(new CartViewModel());
            }
        }

        [HttpGet] // Hien thi QR Modal gio hang
        public async Task<IActionResult> ShowQrModalCart(
            string[]? arrID,
            int? productId,
            string color,
            string size,
            int qty,
            double price,
            bool resetQR,
            bool IsBuyNow = false,
            string PaymentMethod = "transfer")
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var selectList = new List<AddToCart>();
                var partial = await GeneralIndex(arrID, productId, color, size, qty, price);

                if (PaymentMethod == null) return PartialView("_ModalPaymentPartialLeft", partial);
                var userID = User.Identity.IsAuthenticated ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0") : 0;

                if (partial.CartItemByIDs.Count > 0 || productId.HasValue || IsBuyNow)
                {
                    var convertArrIDToInt = arrID.Select(int.Parse).ToHashSet();
                    if (!productId.HasValue)
                    {
                        selectList = partial.CartItemByIDs.Where(p => convertArrIDToInt.Contains(p.ID)).ToList();
                    }
                    else
                    {
                        Products? product = await _dbContext.Products.FirstOrDefaultAsync(p => p.ID == productId);
                        if (product == null)
                        {
                            await transaction.RollbackAsync();
                            return Json(new { success = false, message = "Sản phẩm không tồn tại" });
                        }

                        selectList = new()
                        {
                            new AddToCart()
                            {
                                UserID = userID,
                                ProductID = productId.Value,
                                SelectColor = color,
                                SelectSize = size,
                                Quantity = qty,
                                created_at = DateTime.UtcNow,
                                Product = product

                            }
                        };
                    }
                    var totalList = (decimal?)selectList.Sum(p => p.Product != null ? p.Product.NewPrice * p.Quantity : 0);

                    if (userID > 0)
                    {
                        var address = await _dbContext.Addresses.FirstOrDefaultAsync(p => p.UserID == userID && p.IsDefault);

                        string timeID = Regex.Replace(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss:fff"), @"[/:\s]", "") + "VN_NTT";

                        var order = new Orders
                        {
                            ID = timeID,
                            UserID = userID,
                            //AddressID = address.ID,
                            Amount = totalList > 1000000 ? totalList : totalList + 30000,
                            Status = "Pending",
                            PaymentMethod = PaymentMethod ?? string.Empty,
                            OrderDate = DateTime.UtcNow,
                            TrackingNumber = "Đang tạo mã vận đơn",
                            TransactionId = Guid.NewGuid().ToString("N").Substring(0, 12).ToUpper()
                        };

                        foreach (var p in selectList)
                        {
                            var orderDetail = new OrderDetail()
                            {
                                OrderID = order.ID,
                                ProductID = p.ProductID,
                                Quantity = p.Quantity,
                                Price = p.Product.NewPrice
                            };

                            await _dbContext.OrderDetail.AddAsync(orderDetail);
                        }

                        if (!productId.HasValue)
                        {
                            await _dbContext.Carts.Where(p => p.UserID == userID && convertArrIDToInt.Contains(p.ID))
                                                  .ExecuteUpdateAsync(s => s.SetProperty(sp => sp.IsDelete, true));
                        }

                        await _dbContext.Orders.AddAsync(order);
                        await _dbContext.SaveChangesAsync();
                        // Xu ly xoa don hang khi nguoi dung khong thanh toan sau 10p (Service/OrderCleanupService.cs)

                        // Tao ma QR
                        string bankAcc = "0001335756540";
                        string qrUrl = _qrCode.GenerateBankQrCode(
                            bankAcc,
                            order.Amount,
                            $"Vui lòng không thay đổi ORD{order.TransactionId}",
                            "NGUYEN THANH TUAN");

                        var qrPaymentModel = new QrPaymentViewModel
                        {
                            OrderId = order.ID,
                            Amount = order.Amount,
                            QrCodeUrl = qrUrl,
                            BankAccount = bankAcc,
                        };

                        partial.QrPayment = qrPaymentModel;

                        if (resetQR)
                        {
                            return PartialView("_ModalPaymentPartialRight", partial);
                        }
                    }
                }

                await transaction.CommitAsync();

                if (productId.HasValue)
                {
                    var html = await this.RenderViewAsync("_ModalPaymentPartial", partial, true);

                    return Json(new
                    {
                        html = html,
                        success = true
                    });
                }
                else if (IsBuyNow)
                {
                    return PartialView("_ModalPaymentPartialRight", partial);
                }
                else return PartialView("_ModalPaymentPartial", partial);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet] // Mua san pham ngay trong san pham chi tiet
        public async Task<IActionResult> BuyNow(int productID, string color, string size, int qty, double price)
        {
            var validation = await CheckDataBeforeAddOrBuy(productID, qty, color, size);

            if (validation != null) return validation;
            else
            {
                return RedirectToAction("", "Cart", new
                {
                    productId = productID,
                    color = color,
                    size = size,
                    qty = qty,
                    price = price,
                });
            }
        }

        [HttpPost] // Them san pham vao gio hang
        public async Task<IActionResult> AddToCart(int productID, int qty, string color, string size)
        {
            try
            {
                var validation = await CheckDataBeforeAddOrBuy(productID, qty, color, size);

                if (validation != null) return validation;
                else
                {
                    var userID = User.Identity.IsAuthenticated ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0") : 0;

                    if (userID > 0)
                    {
                        var cartItem = _dbContext.Carts.FirstOrDefault(p => p.UserID == userID && p.ProductID == productID && p.SelectColor == color && p.SelectSize == size);

                        if (cartItem != null) // San pham ton tai, + so so luong san pham
                        {
                            cartItem.Quantity += qty;
                            _dbContext.Carts.Update(cartItem);
                        }
                        else // Sp ko ton tai, them moi
                        {
                            cartItem = new AddToCart()
                            {
                                UserID = userID,
                                ProductID = productID,
                                Quantity = qty,
                                SelectColor = color,
                                SelectSize = size,
                                created_at = DateTime.Now
                            };

                            _dbContext.Carts.Add(cartItem);
                        }

                        await _dbContext.SaveChangesAsync();

                        var countCart = await _dbContext.Carts.Where(p => p.UserID == userID).ToListAsync();

                        return Json(new
                        {
                            success = true,
                            authenticated = 1,
                            qtyNewCart = countCart.Count, // Cap nhat so luong gio hang
                            message = "Thêm sản phẩm vào giỏ hàng thành công"
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            success = false,
                            authenticated = 0,
                            message = "Cần đăng nhập tài khoản trước khi thêm sản phẩm !!!",
                            productID = productID,
                        });
                    }
                }
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Thêm sản phẩm vào giỏ hàng không thành công. " });
            }
        }

        private async Task<IActionResult?> CheckDataBeforeAddOrBuy(int productID, int qty, string color, string size)
        {
            var productExists = await _dbContext.Products.FindAsync(productID);

            if (productExists == null)
                return Json(new { success = false, message = "Sản phẩm không tồn tại" });

            if (string.IsNullOrEmpty(color) && string.IsNullOrEmpty(size))
                return Json(new { success = false, authenticated = 1, message = "Vui lòng chọn phân loại sản phẩm", productID = productID });

            if (string.IsNullOrEmpty(color))
                return Json(new { success = false, authenticated = 1, message = "Bạn cần chọn màu của sản phẩm", productID = productID });

            if (string.IsNullOrEmpty(size))
                return Json(new { success = false, authenticated = 1, message = "Bạn cần chọn kích thước của sản phẩm", productID = productID });

            return null;
        }

        [HttpPost] // Cap nhat so luong gio hang
        public async Task<IActionResult> UpdateToCart(int id, int qty, string opera, string accept)
        {
            try
            {
                var productExists = await _dbContext.Carts.FindAsync(id);
                if (productExists == null)
                {
                    return Json(new { success = false, message = "Sản phẩm trong giỏ hàng không tồn tại" });
                }

                var userID = User.Identity.IsAuthenticated ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0") : 0;

                if (userID > 0)
                {
                    var cartItem = _dbContext.Carts.Include(p => p.Product).FirstOrDefault(p => p.UserID == userID && p.ID == id);

                    if (cartItem != null)
                    {
                        if (opera == "+")
                        {
                            cartItem.Quantity = qty > 0 ? qty : cartItem.Quantity + 1;
                        }
                        else if (opera == "-")
                        {
                            if (cartItem.Quantity > 1)
                            {
                                cartItem.Quantity = qty > 0 ? qty : cartItem.Quantity - 1;
                            }
                            else return Json(new { success = false, message = "Số lượng không thể nhỏ hơn 1" });
                        }
                        else return Json(new { success = false, message = "Thao tác không hợp lệ" });

                        _dbContext.Carts.Update(cartItem);
                        await _dbContext.SaveChangesAsync();

                        return Json(new
                        {
                            success = true,
                            NewQty = cartItem.Quantity,
                            NewTotalPrice = cartItem.Product.NewPrice * cartItem.Quantity
                        });

                    }

                    return Json(new { success = false, message = "Không tìm thấy sản phẩm trong giỏ hàng" });
                }
                else
                {
                    return Json(new { success = false, message = "Vui lòng đăng nhập để cập nhật giỏ hàng" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message }); // Trả về JSON thay vì redirect
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteToCart(int id)
        {
            try
            {
                var productExists = await _dbContext.Carts.FindAsync(id);
                if (productExists == null)
                {
                    ModelState.AddModelError("Cart", "Sản phẩm trong giỏ hàng không tồn tại");
                    return NotFound();
                }

                var userID = User.Identity.IsAuthenticated ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0") : 0;

                if (userID > 0)
                {
                    var cartItem = _dbContext.Carts.FirstOrDefault(p => p.UserID == userID && p.ID == id);

                    if (cartItem != null) // xoa san pham ton tai
                    {
                        _dbContext.Carts.Remove(cartItem);
                    }

                    await _dbContext.SaveChangesAsync();

                    return Json(new { success = true, message = "Xóa sản phẩm thành công." });
                }
                else
                {
                    return RedirectToAction("LoginByProductID", "Auth");
                }

            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập để cập nhật giỏ hàng" });
            }
        }


    }
}
