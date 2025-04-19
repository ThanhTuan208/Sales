using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CRUD_asp.netMVC.Models;
using CRUD_asp.netMVC.Data;
using Microsoft.EntityFrameworkCore;


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
        var product = context.Products
            .Include(p => p.Brand)
            .Include(p => p.Cate).Take(6);

        return View(product);
    }

    public IActionResult Product() => View();

    public IActionResult ProductDetail() => View();

    public IActionResult About() => View();
}
