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
        var product = await context.Products
            .Include(p => p.Brand)
            .Include(p => p.Cate)
            .Include(p => p.Gender)
            .Take(6).ToListAsync();

        return View(product);
    }

    public IActionResult About() => View();

    public IActionResult Contact() => View();

}
