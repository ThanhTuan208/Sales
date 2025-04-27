using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CRUD_asp.netMVC.Models;
using CRUD_asp.netMVC.Data;
using Microsoft.EntityFrameworkCore;
using CRUD_asp.netMVC.Models.ViewModels.Home;
using CRUD_asp.netMVC.Models.Product;

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

    public async Task<IActionResult> Index()
    {
        var product = await context.Products.AsNoTracking()
            .Include(p => p.Brand)
            .Include(p => p.Cate)
            .Include(p => p.Gender)
            .Take(6).OrderByDescending(p => p.ID).ToListAsync();

        var brand = await context.Brand.AsNoTracking().ToListAsync();
        var categories = await context.Category.AsNoTracking().ToListAsync();

        HomeViewModel ViewModel = new HomeViewModel()
        {
            Products = product,
            Brands = brand,
            Categories = categories
        };

        return View(ViewModel);
    }

    public IActionResult About() => View();

    public IActionResult Contact() => View();

}
