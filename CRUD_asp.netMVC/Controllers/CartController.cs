using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.Models.Cart;
using CRUD_asp.netMVC.Models.ViewModels.Cart;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> AddToCart(int productID, string pageCurrent)
        {
            try
            {
                var productExists = await context.Products.FindAsync(productID);
                if (productExists == null)
                {
                    ModelState.AddModelError("Cart", "Sản phẩm không tồn tại");
                    return NotFound();
                }

                var userID = User.Identity.IsAuthenticated ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value) : 0;

                if (userID > 0)
                {
                    var cartItem = context.Carts.FirstOrDefault(p => p.UserID == userID && p.ProductID == productID);

                    if (cartItem != null) // San pham ton tai, +1 qty 
                    {
                        cartItem.Quantity += 1;
                        context.Carts.Update(cartItem);
                    }
                    else // Sp ko ton tai, them moi
                    {
                        cartItem = new AddToCart()
                        {
                            UserID = userID,
                            ProductID = productID,
                            Quantity = 1,
                        };

                        context.Carts.Add(cartItem);
                    }

                    await context.SaveChangesAsync();

                    //var path = HttpContext.Request.Path.Value.Split('/', StringSplitOptions.RemoveEmptyEntries); //StringSplitOptions.RemoveEmptyEntries: bo qua chuoi rong
                    //var actionNameUrl = path.Length >= 2 ? path[1] : string.Empty;

                    if (pageCurrent == null)
                    {
                        return RedirectToAction("ProductDetail", "Product", new { id = productID });
                    }

                    return RedirectToAction(pageCurrent, "Product", new { id = productID });
                }
                else
                {
                    return RedirectToAction("LoginByProductID", "Account", new { ProductID = productID });
                }
            }
            catch (Exception ex)
            {

                return Json(new
                {
                    status = false,
                    message = "Thêm sản phẩm vào giỏ hàng không thành công." + ex.Message,
                });
            }
        }
    }
}
