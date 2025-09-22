using AspNetCoreGeneratedDocument;
using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.Models.Auth;
using CRUD_asp.netMVC.Models.Cart;
using CRUD_asp.netMVC.Models.Order;
using CRUD_asp.netMVC.Service.Payment;
using CRUD_asp.netMVC.ViewModels.Cart;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using NuGet.Protocol.Resources;
using Org.BouncyCastle.Crypto;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace CRUD_asp.netMVC.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDBContext context;
        private readonly QrCodeService _qrCode;

        public CartController(AppDBContext _context, QrCodeService qrCode)
        {
            context = _context;
            _qrCode = qrCode;
        }

        // Su dung method chung cho Index (get, post)
        public async Task<CartViewModel> GeneralIndex(string[]? arrID)
        {
            try
            {
                var cartItems = new List<AddToCart>();
                var cartItemByIDs = new List<AddToCart>();
                var addressUser = new List<Address>();

                var userID = User.Identity.IsAuthenticated ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0") : 0;

                if (userID > 0)
                {
                    cartItems = await context.Carts
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
                    cartItemByIDs = cartItems.Where(p => arrIDSet.Contains(p.ID)).ToList();

                    addressUser = await context.Addresses.ToListAsync();
                }
                else
                {
                    var cart = HttpContext.Session.GetString("Cart");
                    cartItems = string.IsNullOrEmpty(cart) ? new List<AddToCart>() : JsonSerializer.Deserialize<List<AddToCart>>(cart);

                    if (cartItems.Any())
                    {
                        var getIDProductByCart = cartItems.Select(p => p.ProductID);
                        var products = await context.Products.Where(p => getIDProductByCart.Contains(p.ID)).ToListAsync();

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
                    QrPayment = { },
                    CartItemByIDs = cartItemByIDs,
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
        public async Task<IActionResult> Index(string[]? arrID, bool IsAddress = false, bool UpdateAddress = false)
        {
            try
            {
                var viewModel = await GeneralIndex(arrID);

                if (viewModel.CartItemByIDs.Count > 0)
                {
                    if (IsAddress)
                    {
                        if (UpdateAddress)
                        {
                            return PartialView("_EditAddressPartial", viewModel);

                        }
                        else return PartialView("_ListAddressPartial", viewModel);

                    }
                    else
                    {
                        // Tra ve partial dia chi form modal
                        return PartialView("_ModalPaymentPartial", viewModel);
                    }
                }
                else
                {
                    return View(viewModel);
                }
            }
            catch (Exception)
            {
                return View(new CartViewModel());
            }
        }

        [HttpGet] // Hien thi QR Modal gio hang
        public async Task<IActionResult> ShowQrModalCart(string[]? arrID, bool resetQR, string PaymentMethod)
        {
            try
            {
                var viewModel = await GeneralIndex(arrID);

                if (PaymentMethod == null) return PartialView("_ModalPaymentPartialLeft", viewModel);

                if (viewModel.CartItemByIDs.Count > 0)
                {
                    var selectList = viewModel.CartItemByIDs.Where(p => arrID.Contains(p.ID.ToString())).ToList();
                    var totalList = selectList.Sum(p => p.Product != null ? p.Product.NewPrice * p.Quantity : 0);

                    var userID = User.Identity.IsAuthenticated ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) : 0;
                    if (userID > 0)
                    {
                        var address = await context.Addresses.FirstOrDefaultAsync(p => p.UserID == userID && p.IsDefault);

                        string timeID = Regex.Replace(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), @"[/:\s]", "") + "VN_NTT";

                        var order = new Orders
                        {
                            ID = timeID,
                            UserID = userID,
                            AddressID = address.ID,
                            Amount = totalList > 1000000 ? totalList : totalList + 30000,
                            Status = "Pending",
                            PaymentMethod = PaymentMethod ?? string.Empty,
                            OrderDate = DateTime.Now,
                            TransactionId = Guid.NewGuid().ToString("N").Substring(0, 12).ToUpper()
                        };

                        selectList.ForEach(async p =>
                        {
                            var orderDetail = new OrderDetail()
                            {
                                OrderID = order.ID,
                                ProductID = p.ProductID,
                                Quantity = p.Quantity,
                                Price = p.Product.NewPrice
                            };

                            await context.OrderDetail.AddAsync(orderDetail);
                        });

                        await context.Orders.AddAsync(order);
                        await context.SaveChangesAsync();
                        // Xu ly xoa don hang khi nguoi dung khong thanh toan sau 10p (Service/OrderCleanupService.cs)

                        // Tao ma QR
                        string bankAcc = "0001335756540";
                        string qrUrl = _qrCode.GenerateBankQrCode(
                            bankAcc,
                            order.Amount,
                            $"Vui lòng không thay đổi: ORD{order.TransactionId}",
                            "NGUYEN THANH TUAN");

                        var qrPaymentModel = new QrPaymentViewModel
                        {
                            OrderId = order.ID,
                            Amount = order.Amount,
                            QrCodeUrl = qrUrl,
                            BankAccount = bankAcc,
                        };

                        viewModel.QrPayment = qrPaymentModel;

                        if (resetQR)
                        {
                            return PartialView("_ModalPaymentPartialRight", viewModel);
                        }
                        else
                        {
                            return PartialView("_ModalPaymentPartial", viewModel);
                        }
                    }

                    return PartialView("_ModalPaymentPartial", viewModel);
                }
                else
                {
                    return PartialView("_ModalPaymentPartial", viewModel);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpPost, ValidateAntiForgeryToken] // Them san pham vao gio hang
        public async Task<IActionResult> AddToCart(int productID, int qty, string color, string size)
        {
            try
            {
                var productExists = await context.Products.FindAsync(productID);

                if (productExists == null)
                {
                    return Json(new { success = false, message = "Sản phẩm không tồn tại" });
                }

                if (color == null && size == null)
                {
                    return Json(new { success = false, authenticated = 1, message = "Vui lòng chọn phân loại sản phẩm", productID = productID });
                }
                else if (color == null)
                {
                    return Json(new { success = false, authenticated = 1, message = "Bạn cần chọn màu của sản phẩm", productID = productID });
                }
                else if (size == null)
                {
                    return Json(new { success = false, authenticated = 1, message = "Bạn cần chọn kích thước của sản phẩm", productID = productID });
                }
                else
                {
                    var userID = User.Identity.IsAuthenticated ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value) : 0;

                    if (userID > 0)
                    {
                        var cartItem = context.Carts.FirstOrDefault(p => p.UserID == userID && p.ProductID == productID && p.SelectColor == color && p.SelectSize == size);

                        if (cartItem != null) // San pham ton tai, + so so luong san pham
                        {
                            cartItem.Quantity += qty;
                            context.Carts.Update(cartItem);
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

                            context.Carts.Add(cartItem);
                        }

                        await context.SaveChangesAsync();

                        var countCart = await context.Carts.Where(p => p.UserID == userID).ToListAsync();

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

        [HttpPost, ValidateAntiForgeryToken] // Cap nhat so luong gio hang
        public async Task<IActionResult> UpdateToCart(int id, int qty, string opera, string accept)
        {
            try
            {
                var productExists = await context.Carts.FindAsync(id);
                if (productExists == null)
                {
                    return Json(new { success = false, message = "Sản phẩm trong giỏ hàng không tồn tại" });
                }

                var userID = User.Identity.IsAuthenticated ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value) : 0;

                if (userID > 0)
                {
                    var cartItem = context.Carts.Include(p => p.Product).FirstOrDefault(p => p.UserID == userID && p.ID == id);

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
                            else
                            {
                                return Json(new { success = false, message = "Số lượng không thể nhỏ hơn 1" });
                            }
                        }
                        else
                        {
                            return Json(new { success = false, message = "Thao tác không hợp lệ" });
                        }

                        context.Carts.Update(cartItem);
                        await context.SaveChangesAsync();

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

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteToCart(int id)
        {
            try
            {
                var productExists = await context.Carts.FindAsync(id);
                if (productExists == null)
                {
                    ModelState.AddModelError("Cart", "Sản phẩm trong giỏ hàng không tồn tại");
                    return NotFound();
                }

                var userID = User.Identity.IsAuthenticated ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value) : 0;

                if (userID > 0)
                {
                    var cartItem = context.Carts.FirstOrDefault(p => p.UserID == userID && p.ID == id);

                    if (cartItem != null) // xoa san pham ton tai
                    {
                        context.Carts.Remove(cartItem);
                    }

                    await context.SaveChangesAsync();

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
