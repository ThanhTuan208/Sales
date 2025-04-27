using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.Models.Product;
using CRUD_asp.netMVC.Models.ViewModels.Home;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace CRUD_asp.netMVC.Controllers
{
    public class ProductController : Controller
    {
        public readonly AppDBContext context;

        public ProductController(AppDBContext _context)
        {
            context = _context;
        }

        // Hien thi danh sach phan trang san pham va phan trang thuong hieu
        public async Task<IActionResult> Index(int productPage = 1)
        {
            IQueryable<Products> products = context.Products.AsNoTracking()
                .Include(p => p.Brands)
                .Include(p => p.Cate)
                .Include(p => p.Gender)
                .Include(p => p.ProductImages).OrderByDescending(p => p.ID);

            var pagProduct = await PaginatedList<Products>.CreatePagAsync(products, productPage, 16);

            var brands = context.Brand.AsNoTracking();
            var brandList = await context.Brand.AsNoTracking().ToListAsync();

            var cates = context.Category.AsNoTracking();
            var cateList = await context.Category.AsNoTracking().ToListAsync();

            //var relatedPagProductByBrand = relatedPagProduct.GroupBy(p => p.BrandID).ToDictionary(p => p.Key, p => p.Take(3).ToList());
            BrandShowProductViewModel ViewModel = new()
            {
                Products = pagProduct,
                Brands = await PaginatedList<Brand>.CreatePagAsync(brands, 1, brandList.Count),
                Categories = await PaginatedList<Category>.CreatePagAsync(cates, 1, cateList.Count),
                //RelatedProductByBrands = null // relatedPagProductByBrand
            };

            return View(ViewModel);
        }

        // Hien thi san pham cua brand qua id brand
        public async Task<IActionResult> getProductByBrand(int brandID)
        {
            var brands = await context.Brand.AsNoTracking().FirstOrDefaultAsync(p => p.ID == brandID);

            if (brands == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var getPagProductByBrand = await context.Products.AsNoTracking().Where(p => p.Brands == brands).Include(p => p.Brands).ToListAsync();

            return View(getPagProductByBrand);
        }

        // Hien thi d/s chi tiet cua san pham
        public async Task<IActionResult> ProductDetail(int id)
        {
            var product = id > 0 && !string.IsNullOrWhiteSpace(id.ToString()) ?
                 await context.Products.AsNoTracking()
                .Include(p => p.Brands)
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

            var brandList = await context.Brand.AsNoTracking().ToListAsync();

            var cateList = await context.Category.AsNoTracking().ToListAsync();

            GeneralProduct_ListCateBrand ViewModel = new()
            {
                Product = product,
                Brands = brandList,
                Categories = cateList
            };

            if (ViewModel != null)
            {
                return View(ViewModel);
            }

            return NotFound();
        }
    }
}
