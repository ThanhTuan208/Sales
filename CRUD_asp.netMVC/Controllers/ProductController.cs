using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.Models.Product;
using CRUD_asp.netMVC.Models.ViewModels.Home;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using NuGet.Common;
using System.Globalization;
using System.Text;

namespace CRUD_asp.netMVC.Controllers
{
    public class ProductController : Controller
    {
        public readonly AppDBContext context;

        public ProductController(AppDBContext _context)
        {
            context = _context;
        }

        // Ham chuyen doi co dau sang ko dau, chu hoa thanh chu thuong NormalizationFormD, FormC
        public string RemoveDiacritics(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            var textNormalFormD = text.Normalize(NormalizationForm.FormD);
            StringBuilder builderText = new StringBuilder();

            foreach (var item in textNormalFormD)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(item);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    builderText.Append(item);
                }
            }

            return builderText.ToString().Normalize(NormalizationForm.FormC);
        }

        // Hien thi danh sach san pham theo tu khoa tim kiem
        #region Cach 1: chi co the truy van dung gia tri keyword truyen vao ma ko the phan biet chu co dau va ko dau -> chuyen cach 2 
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Index(int productPage = 1, string keyword = "")
        //{

        //    var products = context.Products.AsNoTracking()
        //        .Include(p => p.Brands)
        //        .Include(p => p.Cate)

        //        .Where(p => p.Name.ToLower().Contains(keyword.ToLower().Trim()) || p.Description.ToLower().Contains(keyword.ToLower().Trim()));

        //    var pagProduct = await PaginatedList<Products>.CreatePagAsync(products, productPage, 16);

        //    var brands = context.Brand.AsNoTracking();
        //    var brandList = await context.Brand.AsNoTracking().ToListAsync();

        //    var cates = context.Category.AsNoTracking();
        //    var cateList = await context.Category.AsNoTracking().ToListAsync();

        //    BrandShowProductViewModel ViewModel = new()
        //    {
        //        Products = pagProduct,
        //        Brands = await PaginatedList<Brand>.CreatePagAsync(brands, 1, brandList.Count),
        //        Categories = await PaginatedList<Category>.CreatePagAsync(cates, 1, cateList.Count),
        //    };

        //    return View(ViewModel);

        //}
        #endregion

        // Tu tao pagination rieng vi vấn đề gốc rễ là sử dụng AsEnumerable() để gọi RemoveDiacritics, EF Core không thể dịch RemoveDiacritics sang SQL.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(int productPage = 1, string keyword = "")
        {
            var keywordDiacritics = RemoveDiacritics(keyword.ToLower());

            // Lấy danh sách sản phẩm từ DB
            var products = await context.Products
                .AsNoTracking()
                .Include(p => p.Brands)
                .Include(p => p.Cate).ToListAsync();

            var filteredProducts = products
                .Where(p => RemoveDiacritics(p.Name.ToLower()).Contains(keywordDiacritics) ||
                            RemoveDiacritics(p.Description.ToLower()).Contains(keywordDiacritics))
                .ToList();

            var productCount = products
               .Where(p => RemoveDiacritics(p.Name.ToLower()).Contains(keywordDiacritics) ||
                           RemoveDiacritics(p.Description.ToLower()).Contains(keywordDiacritics))
               .Count();

            var pagProduct = new PaginatedList<Products>(filteredProducts, productCount, productPage, 16);
            if (!pagProduct.Any())
            {
                pagProduct = new PaginatedList<Products>(products, productCount, productPage, 16);
                //ViewData["Info"] = "Không tìm thấy sản phẩm nào với từ khóa này";
            }

            var brands = context.Brand.AsNoTracking();
            var brandList = await context.Brand.AsNoTracking().ToListAsync();

            var cates = context.Category.AsNoTracking();
            var cateList = await context.Category.AsNoTracking().ToListAsync();

            BrandShowProductViewModel ViewModel = new()
            {
                Products = pagProduct,
                Brands = await PaginatedList<Brand>.CreatePagAsync(brands, 1, brandList.Count),
                Categories = await PaginatedList<Category>.CreatePagAsync(cates, 1, cateList.Count),
            };

            return View(ViewModel);
        }

        // Hien thi danh sach phan trang san pham va phan trang thuong hieu
        //[Route("Product/{productPage:int?}")]
        [HttpGet]
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

            BrandShowProductViewModel ViewModel = new()
            {
                Products = pagProduct,
                Brands = await PaginatedList<Brand>.CreatePagAsync(brands, 1, brandList.Count),
                Categories = await PaginatedList<Category>.CreatePagAsync(cates, 1, cateList.Count),
            };

            return View(ViewModel);
        }

        // Hien thi san pham cua brand qua id brand
        [HttpGet]
        public async Task<IActionResult> getProductByBrand(int brandID, int productPage = 1)
        {
            var brands = await context.Brand.AsNoTracking().FirstOrDefaultAsync(p => p.ID == brandID);

            ViewData["brandID"] = brandID;
            ViewData["image"] = brands.PicturePath;

            if (brands == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var getPagProductByBrand = context.Products.AsNoTracking().Where(p => p.Brands == brands).Include(p => p.Brands);
            var getPagProductListByBrand = await getPagProductByBrand.ToListAsync();

            BrandShowProductViewModel ViewModel = new()
            {
                Products = await PaginatedList<Products>.CreatePagAsync(getPagProductByBrand, productPage, 6),
                Brands = await PaginatedList<Brand>.CreatePagAsync(context.Brand.AsNoTracking(), 1, context.Brand.Count()),
                Categories = await PaginatedList<Category>.CreatePagAsync(context.Category.AsNoTracking(), 1, context.Category.Count())
            };

            return View(ViewModel);
        }

        // Hien thi san pham cua Category qua id danh muc CateID
        [HttpGet]
        public async Task<IActionResult> getProductByCate(int CateID = 1, int productPage = 1)
        {
            var cateID = await context.Category.AsNoTracking().FirstOrDefaultAsync(p => p.ID == CateID);

            ViewData["cateID"] = CateID;

            if (cateID == null)
            {
                return RedirectToAction(nameof(Index));
            }


            var getPagProductByCate = context.Products.AsNoTracking().Where(p => p.CateID == CateID).Include(p => p.Cate);
            var getPagProductListByCate = await getPagProductByCate.ToListAsync();

            BrandShowProductViewModel ViewModel = new()
            {
                Products = await PaginatedList<Products>.CreatePagAsync(getPagProductByCate, productPage, 6),
                Brands = await PaginatedList<Brand>.CreatePagAsync(context.Brand.AsNoTracking(), 1, context.Brand.Count()),
                Categories = await PaginatedList<Category>.CreatePagAsync(context.Category.AsNoTracking(), 1, context.Category.Count())
            };

            return View(ViewModel);
        }

        // hien thi san pham cua brand duoc phan loai boi category 
        [HttpGet]
        public async Task<IActionResult> getProductByCate_Brand(int brandID = 1, int CateID = 1, int productPage = 1)
        {
            var cates = await context.Category.AsNoTracking().FirstOrDefaultAsync(p => p.ID == CateID);
            var brands = await context.Brand.AsNoTracking().FirstOrDefaultAsync(p => p.ID == brandID);

            ViewData["cateID"] = CateID;
            ViewData["brandID"] = brandID;
            ViewData["image"] = brands.PicturePath;

            if (cates == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var getPagProductByCate_Brand = context.Products.AsNoTracking().Where(p => p.CateID == CateID && p.BrandID == brandID).Include(p => p.Cate).Include(p => p.Brands);
            var getPagProductListByCate = await getPagProductByCate_Brand.ToListAsync();

            BrandShowProductViewModel ViewModel = new()
            {
                Products = await PaginatedList<Products>.CreatePagAsync(getPagProductByCate_Brand, productPage, 6),
                Brands = await PaginatedList<Brand>.CreatePagAsync(context.Brand.AsNoTracking(), 1, context.Brand.Count()),
                Categories = await PaginatedList<Category>.CreatePagAsync(context.Category.AsNoTracking(), 1, context.Category.Count())
            };

            return View(ViewModel);
        }

        // Hien thi chi tiet cua san pham theo id
        public async Task<IActionResult> ProductDetail(int id)
        {
            var product = await context.Products.AsNoTracking()
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
                .FirstOrDefaultAsync(p => p.ID == id);

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
