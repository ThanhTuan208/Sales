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
        [HttpGet]
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
                    TotalPrice = cartItems.Sum(p => p.Product != null ? p.Product.Price * p.Quantity : 0)
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Cart", "Lỗi hiển thị giỏ hàng: " + ex.Message);
                return View(new CartViewModel());
            }
        }
    }
}
