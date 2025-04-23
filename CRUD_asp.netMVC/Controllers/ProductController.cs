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

        public async Task<IActionResult> Index()
        {
            var products = await context.Products.AsNoTracking()
                .Include(p => p.Brand)
                .Include(p => p.Cate)
                .Include(p => p.Gender)
                .Include(p => p.ProductImages).OrderByDescending(p => p.ID).ToListAsync();

            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }
        public async Task<IActionResult> ProductDetail(int id)
        {
            var product = id > 0 && !string.IsNullOrWhiteSpace(id.ToString()) ?
                 await context.Products.AsNoTracking()
                .Include(p => p.Brand)
                .Include(p => p.Cate)
                .Include(p => p.Gender)
                .Include(p => p.ProductSize).ThenInclude(p => p.size)
                .Include(p => p.ProductColor).ThenInclude(p => p.Color)
                .Include(p => p.ProductMaterial).ThenInclude(p => p.Material)
                .Include(p => p.ProductStyles).ThenInclude(p => p.Style)
                .Include(p => p.ProductSeasons).ThenInclude(p => p.Season)
                .Include(p => p.ProductTags).ThenInclude(p => p.Tag)
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.ID == id) : null;

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    }
}
