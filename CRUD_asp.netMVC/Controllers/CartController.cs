using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.Models.Cart;
using CRUD_asp.netMVC.Models.ViewModels.Cart;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Identity.Client;
using NuGet.Protocol.Plugins;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text.Json;

namespace CRUD_asp.netMVC.Controllers
{
    public class CartController : Controller
    {
        public readonly AppDBContext context;

        public CartController(AppDBContext _context)
        {
            context = _context;
        }

        // GET: Cart
        [HttpGet, Route("Cart")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var cartItems = new List<AddToCart>();
                var userID = User.Identity.IsAuthenticated ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value) : 0;

                if (userID > 0)
                {
                    cartItems = await context.Carts
                        .AsNoTracking()
                        .Include(c => c.Product)
                        .Include(c => c.Product).ThenInclude(p => p.ProductColor)
                        .Include(c => c.Product).ThenInclude(p => p.ProductSize)
                        .Include(c => c.Users)
                        .Where(c => c.UserID == userID)
                        .ToListAsync();
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
                    TotalPrice = cartItems.Sum(p => p.Product != null ? p.Product.NewPrice * p.Quantity : 0)
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Cart", "Lỗi hiển thị giỏ hàng: " + ex.Message);
                return View(new CartViewModel());
            }
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int productID, int qty, string color, string size)
        {
            try
            {
                var productExists = await context.Products.FindAsync(productID);
                if (productExists == null)
                {
                    ModelState.AddModelError("Cart", "Sản phẩm không tồn tại");
                    return NotFound();
                }

                if (color == null)
                {
                    TempData["ErrorMessage"] = "Bạn cần chọn màu của sản phẩm";
                }

                if (size == null)
                {
                    TempData["ErrorMessage"] = "Bạn cần chọn kích thước của sản phẩm";
                }

                if (color != null && size != null)
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
                            };

                            context.Carts.Add(cartItem);
                        }

                        await context.SaveChangesAsync();
                        //var path = HttpContext.Request.Path.Value.Split('/', StringSplitOptions.RemoveEmptyEntries); //StringSplitOptions.RemoveEmptyEntries: bo qua chuoi rong
                        //var actionNameUrl = path.Length >= 2 ? path[1] : string.Empty;
                        TempData["SuccessMessage"] = "Thêm sản phẩm vào giỏ hàng thành công";
                        return RedirectToAction("ProductDetail", "Product", new { id = productID });
                    }
                    else
                    {
                        return RedirectToAction("LoginByProductID", "Account", new { ProductID = productID });
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Vui lòng chọn phân loại sản phẩm";
                    return RedirectToAction("ProductDetail", "Product", new { id = productID });
                }
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Thêm sản phẩm vào giỏ hàng không thành công";
                return RedirectToAction("ProductDetail", "Product", new { id = productID });
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

                    return RedirectToAction("Index", "Cart");
                }
                else
                {
                    return RedirectToAction("LoginByProductID", "Account");
                }

            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Thêm sản phẩm vào giỏ hàng không thành công";
                return RedirectToAction("ProductDetail", "Product");
            }
        }
    }
}
