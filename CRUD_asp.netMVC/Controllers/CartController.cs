using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.Models.Cart;
using CRUD_asp.netMVC.Models.Order;
using CRUD_asp.netMVC.Models.ViewModels.Cart;
using CRUD_asp.netMVC.Service.Payment;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using NuGet.Protocol.Resources;
using System.Security.Claims;
using System.Text.Json;

namespace CRUD_asp.netMVC.Controllers
{
    public class CartController : Controller
    {
        public readonly AppDBContext context;
        public readonly QrCodeService _qrCode;

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

                var userID = User.Identity.IsAuthenticated ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value) : 0;

                if (userID > 0)
                {
                    cartItems = await context.Carts
                        .AsNoTracking()
                        .Include(c => c.Product)
                        .Include(c => c.Product).ThenInclude(p => p.ProductColor)
                        .Include(c => c.Product).ThenInclude(p => p.ProductSize)
                        .Include(c => c.Product).ThenInclude(p => p.ProductImages)
                        .Include(c => c.Users).ThenInclude(p => p.Addresses)
                        .Where(c => c.UserID == userID)
                        .ToListAsync();

                    var arrIDSet = arrID?.Select(int.Parse).ToHashSet() ?? new HashSet<int>();

                    cartItemByIDs = cartItems.Where(p => arrIDSet.Contains(p.ID)).ToList();
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

        [HttpGet, Route("Cart")] // Show Cart
        public async Task<IActionResult> Index(string[]? arrID, bool IsAddress = false)
        {
            try
            {
                ViewBag.IsAddress = IsAddress;

                var viewModel = await GeneralIndex(arrID);

                if (viewModel.CartItemByIDs.Count > 0)
                {
                    if (IsAddress)
                    {
                        // Tra ve partial dia chi form modal
                        return PartialView("_EditAddressPartial", viewModel);
                    }
                    else
                    {
                        // Tra ve partial thanh toan qr
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
                return View();
            }
        }

        [HttpPost, ValidateAntiForgeryToken] // Hien thi QR Modal gio hang
        public async Task<IActionResult> ShowQrModalCart(string[]? arrID)
        {
            try
            {
                var viewModel = await GeneralIndex(arrID);

                if (viewModel.CartItemByIDs.Count > 0)
                {
                    if (decimal.TryParse(viewModel.TotalPrice.ToString(), out decimal amount))
                    {
                        var order = new Orders()
                        {
                            ID = Guid.NewGuid().ToString(),
                            Amount = amount,
                            Status = "Pending",
                            OrderDate = DateTime.Now
                        };

                        // Tao ma QR
                        string bankAcc = "0001335756540";
                        var qrPaymentService = _qrCode.GenerateBankQrCode(order.ID, order.Amount, bankAcc);

                        var qrPaymentModel = new QrPaymentViewModel
                        {
                            OrderId = order.ID,
                            Amount = order.Amount,
                            QrCodeUrl = qrPaymentService,
                            BankAccount = bankAcc,
                            PollingUrl = Url.Action("CheckPaymentStatus", "Payment", new { orderId = order.ID }, Request.Scheme)
                        };

                        viewModel.QrPayment = qrPaymentModel;

                        // can tao partial rieng de hien thi danh sach ben trong modal, co the thu xoa partial de kiem tra
                        return PartialView("_ModalPaymentPartial", viewModel);
                    }
                    else
                    {
                        return View(nameof(Index), viewModel);
                    }
                }
                else
                {
                    return View(viewModel);
                }
            }
            catch (Exception)
            {
                return PartialView("_ModalPaymentPartial", new CartViewModel());
            }
        }

        //public async Task<IActionResult> 

        [HttpPost, ValidateAntiForgeryToken]
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
                    return Json(new { success = false, authenticated = 1, message = "Vui lòng chọn phân loại sản phẩm", productID = productID, });
                }
                else if (color == null)
                {
                    return Json(new { success = false, authenticated = 1, message = "Bạn cần chọn màu của sản phẩm", productID = productID, });
                }
                else if (size == null)
                {
                    return Json(new { success = false, authenticated = 1, message = "Bạn cần chọn kích thước của sản phẩm", productID = productID, });
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
                //TempData["ErrorMessage"] = "Thêm sản phẩm vào giỏ hàng không thành công";
                return Json(new { success = false, message = "Thêm sản phẩm vào giỏ hàng không thành công. " });
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
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
