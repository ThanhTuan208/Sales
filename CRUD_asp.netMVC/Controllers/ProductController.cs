using CRUD_asp.netMVC.Data;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD_asp.netMVC.Controllers
{
    public class ProductController : Controller
    {
        public readonly AppDBContext context;

        public ProductController(AppDBContext _context)
        {
            context = _context;
        }

        public IActionResult Index() => View();

        public async Task<IActionResult> ProductDetail(int id)
        {
            var product = id > 0 && !string.IsNullOrWhiteSpace(id.ToString()) ?
                 await context.Products
                .Include(p => p.Brand)
                .Include(p => p.Cate)
                .Include(p => p.Gender)
                .Include(p => p.ProductSize).ThenInclude(p => p.size)
                .Include(p => p.ProductColor).ThenInclude(p => p.Color)
                .Include(p => p.ProductMaterial).ThenInclude(p => p.Material)
                .FirstOrDefaultAsync(p => p.ID == id) : null;

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    }
}
