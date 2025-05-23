using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CRUD_asp.netMVC.Models;
using CRUD_asp.netMVC.Data;
using Microsoft.EntityFrameworkCore;
using CRUD_asp.netMVC.Models.ViewModels.Home;
using CRUD_asp.netMVC.Models.Product;
using System.CodeDom;
using CRUD_asp.netMVC.Models.Cart;
using System.Text.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace CRUD_asp.netMVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    public readonly AppDBContext context;


    public HomeController(ILogger<HomeController> logger, AppDBContext _context)
    {
        _logger = logger;
        context = _context;
    }

    // Hien thi san pham noi bat (Featured) tren giao dien chinh
    public async Task<IActionResult> Index()
    {
        var product = await context.Products.AsNoTracking()
            .Include(p => p.Brands)
            .Include(p => p.Cate)
            .Include(p => p.Gender)
            .Include(p => p.Carts)
            .Where(p => p.FeaturedID == 1)
            .Take(6).OrderByDescending(p => p.ID).ToListAsync();

        var userID = User.Identity.IsAuthenticated ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) : 0;

        var brand = await context.Brand.AsNoTracking().ToListAsync();
        var categories = await context.Category.AsNoTracking().ToListAsync();
        var carts = await context.Carts.Where(p => p.UserID == userID).ToListAsync();

        HomeViewModel ViewModel = new HomeViewModel()
        {
            Products = product,
            Brands = brand,
            Categories = categories,
            Carts = carts
        };

        if (ViewModel == null)
        {
            return NotFound();
        }

        ViewData["cart"] = carts.Count;
        return View(ViewModel);
    }

    public async Task<IActionResult> About()
    {
        var brandList = await context.Brand.AsNoTracking().ToListAsync();

        var cateList = await context.Category.AsNoTracking().ToListAsync();

        GeneralProduct_ListCateBrand ViewModel = new()
        {
            Product = await context.Products.FirstOrDefaultAsync(),
            Brands = brandList,
            Categories = cateList
        };

        return View(ViewModel);
    }

    public async Task<IActionResult> Contact()
    {
        var brandList = await context.Brand.AsNoTracking().ToListAsync();

        var cateList = await context.Category.AsNoTracking().ToListAsync();

        GeneralProduct_ListCateBrand ViewModel = new()
        {
            Product = await context.Products.FirstOrDefaultAsync(),
            Brands = brandList,
            Categories = cateList
        };

        return View(ViewModel);
    }

    // note: chua xu li xong
    public async Task<IActionResult> ProductFeatured()
    {
        var brandList = await context.Brand.AsNoTracking().ToListAsync();

        var cateList = await context.Category.AsNoTracking().ToListAsync();

        GeneralProduct_ListCateBrand ViewModel = new()
        {
            Product = await context.Products.FirstOrDefaultAsync(),
            Brands = brandList,
            Categories = cateList
        };

        return View(ViewModel);
    }

}
