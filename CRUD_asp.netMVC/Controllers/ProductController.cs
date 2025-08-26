using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.Models.Product;
using CRUD_asp.netMVC.Models.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Claims;
using System.Text;

namespace CRUD_asp.netMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDBContext context;
        //private readonly IDbContextFactory<AppDBContext> dbContextFactory;

        public ProductController(AppDBContext _context)
        {
            context = _context;
            //dbContextFactory = _dbContextFactory;
        }

        /// Ham chuyen doi co dau sang ko dau, chu hoa thanh chu thuong NormalizationFormD, FormC
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

        /// <summary>
        /// Search Products
        /// </summary>
        /// <param name="productPage"></param>
        /// <param name="keyword"></param>
        /// <param name="cateID"></param>
        /// <param name="brandID"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> SearchProduct(int productPage = 1, string keyword = "", int? cateID = null, int? brandID = null)
        {
            try
            {
                var keywordDiacritics = !string.IsNullOrWhiteSpace(keyword)
                    ? RemoveDiacritics(keyword.ToLower().Trim())
                    : string.Empty;

                if (keywordDiacritics == string.Empty)
                {
                    return RedirectToAction("Index", "Product");
                }

                IQueryable<Products> products = context.Products.AsNoTracking()
                  .Include(p => p.Brands)
                  .Include(p => p.Cate)
                  .Where(p => p.NormalizedName.ToLower().Contains(keywordDiacritics) ||
                               p.NormalizedDescription.ToLower().Contains(keywordDiacritics));

                if (cateID > 0 && cateID.HasValue)
                {
                    products = products.Where(p => p.CateID == cateID);
                }

                if (brandID > 0 && brandID.HasValue)
                {
                    products = products.Where(p => p.BrandID == brandID);
                }

                var productCount = await products.CountAsync();
                //var path = HttpContext.Request.Path.Value.Split('/', StringSplitOptions.RemoveEmptyEntries); //StringSplitOptions.RemoveEmptyEntries: bo qua chuoi rong
                //var actionNameUrl = path.Length >= 2 ? path[1] : string.Empty;

                ViewData["cateID"] = cateID;
                ViewData["brandID"] = brandID;

                ViewBag.ProductCount = productCount;
                ViewBag.Keyword = keyword;

                var pagProduct = await PaginatedList<Products>.CreatePagAsync(products, productPage, 12);
                if (!pagProduct.Any())
                {
                    ViewData["Info"] = $"Không tìm thấy sản phẩm nào với từ khóa '{keyword}' ";
                }

                // Pagination truyen tham so cho product, brand, category, cart
                var ViewModel = await CreatePaginationGeneral(products, productPage, 12);

                return View(ViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Search", "Lỗi tìm kiếm hiển thị sản phẩm: " + ex.Message);
                throw;
            }

        }

        [HttpGet]
        /// Hien thi danh sach phan trang san pham va phan trang thuong hieu
        public async Task<IActionResult> Index(int productPage = 1)
        {
            IQueryable<Products> products = context.Products.AsNoTracking()
                .Include(p => p.Brands)
                .Include(p => p.Cate)
                .Include(p => p.Gender)
                .Include(p => p.ProductImages).OrderByDescending(p => p.ID);

            var productCount = await products.CountAsync();
            ViewBag.ProductCount = productCount;

            var pagProduct = await PaginatedList<Products>.CreatePagAsync(products, productPage, 16);

            // Pagination truyen tham so cho product, brand, category, cart
            var ViewModel = await CreatePaginationGeneral(products, productPage, 12);

            return View(ViewModel);
        }

        /// <summary>
        /// </summary>
        /// <param name="brandID"></param>
        /// <param name="productPage"></param>
        /// <returns></returns>
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

            IQueryable<Products> getPagProductByBrand = context.Products.AsNoTracking()
                .Where(p => p.Brands == brands)
                .Include(p => p.Brands);

            var productCount = await getPagProductByBrand.CountAsync();
            ViewBag.ProductCount = productCount;

            // Pagination truyen tham so cho product, brand, category, cart
            var ViewModel = await CreatePaginationGeneral(getPagProductByBrand, productPage, 6);

            //var path = HttpContext.Request.Path.Value.Split('/', StringSplitOptions.RemoveEmptyEntries); //StringSplitOptions.RemoveEmptyEntries: bo qua chuoi rong
            //var actionNameUrl = path.Length >= 2 ? path[1] : string.Empty;
            //ViewBag.ActionNameUrl = actionNameUrl;

            return View(ViewModel);
        }

        /// <summary>
        /// Hien thi san pham cua Category qua id danh muc CateID
        /// </summary>
        /// <param name="CateID"></param>
        /// <param name="productPage"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> getProductByCate(int CateID = 1, int productPage = 1)
        {
            var cateID = await context.Category.AsNoTracking().FirstOrDefaultAsync(p => p.ID == CateID);

            ViewData["cateID"] = CateID;

            if (cateID == null)
            {
                return RedirectToAction(nameof(Index));
            }

            IQueryable<Products> getPagProductByCate = context.Products.AsNoTracking()
                .Where(p => p.CateID == CateID)
                .Include(p => p.Cate);

            var productCount = await getPagProductByCate.CountAsync();
            ViewBag.ProductCount = productCount;

            // Pagination truyen tham so cho product, brand, category, cart
            var ViewModel = await CreatePaginationGeneral(getPagProductByCate, productPage, 6);

            return View(ViewModel);
        }

        /// <summary>
        /// hien thi san pham cua brand duoc phan loai boi category 
        /// </summary>
        /// <param name="brandID"></param>
        /// <param name="CateID"></param>
        /// <param name="productPage"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> getProductByCate_Brand(int brandID = 1, int CateID = 1, int productPage = 1)
        {
            var cates = await context.Category.AsNoTracking().FirstOrDefaultAsync(p => p.ID == CateID);
            var brands = await context.Brand.AsNoTracking().FirstOrDefaultAsync(p => p.ID == brandID);
            var cart = await context.Carts.AsNoTracking().ToListAsync();

            ViewData["cateID"] = CateID;
            ViewData["brandID"] = brandID;
            ViewData["image"] = brands.PicturePath;

            if (cates == null)
            {
                return RedirectToAction(nameof(Index));
            }

            IQueryable<Products> getPagProductByCate_Brand = context.Products.AsNoTracking()
                .Where(p => p.CateID == CateID && p.BrandID == brandID)
                .Include(p => p.Cate)
                .Include(p => p.Brands);

            var productCount = await getPagProductByCate_Brand.CountAsync();
            ViewBag.ProductCount = productCount;

            // Pagination truyen tham so cho product, brand, category, cart
            var ViewModel = await CreatePaginationGeneral(getPagProductByCate_Brand, productPage, 6);

            return View(ViewModel);
        }

        /// <summary>
        /// Hien thi chi tiet cua san pham theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ProductDetail(int id)
        {
            var product = await context.Products.AsNoTracking()
                .Include(p => p.Brands)
                .Include(p => p.Cate)
                .Include(p => p.Gender)
                .Include(p => p.ProductImages)
                .Include(p => p.ProductSize).ThenInclude(p => p.size)
                .Include(p => p.ProductColor).ThenInclude(p => p.Color)
                .Include(p => p.ProductMaterial).ThenInclude(p => p.Material)
                .Include(p => p.ProductStyles).ThenInclude(p => p.Style)
                .Include(p => p.ProductSeasons).ThenInclude(p => p.Season)
                .Include(p => p.ProductTags).ThenInclude(p => p.Tag)
                .FirstOrDefaultAsync(p => p.ID == id);

            var brandList = await context.Brand.AsNoTracking().ToListAsync();

            var cateList = await context.Category.AsNoTracking().ToListAsync();

            var userID = User.Identity.IsAuthenticated ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) : 0;
            var carts = await context.Carts.Where(p => p.UserID == userID).ToListAsync();

            ViewData["cart"] = carts.Count;

            GeneralProduct_ListCateBrand ViewModel = new()
            {
                Product = product,
                Brands = brandList,
                Categories = cateList,
                Carts = carts
            };

            return ViewModel != null ? View(ViewModel) : NotFound();
        }

        /// <summary>
        /// </summary>
        /// <param name="Iqueryable"></param>
        /// <param name="pageCurrent"></param>
        /// <param name="pageCount"></param>
        /// <param name="carts"></param>
        /// <returns></returns>
        public async Task<getPaginationByProductViewModel> CreatePaginationGeneral(IQueryable<Products> Iqueryable, int pageCurrent, int pageCount)
        {
            var product = await PaginatedList<Products>.CreatePagAsync(Iqueryable, pageCurrent, pageCount);
            var brands = await PaginatedList<Brand>.CreatePagAsync(context.Brand.AsNoTracking(), 1, context.Brand.AsNoTracking().Count());
            var cates = await PaginatedList<Category>.CreatePagAsync(context.Category.AsNoTracking(), 1, context.Category.AsNoTracking().Count());

            var userID = User.Identity.IsAuthenticated ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) : 0;
            var carts = await context.Carts.Where(p => p.UserID == userID).ToListAsync();
            ViewData["cart"] = carts.Count;

            return new getPaginationByProductViewModel
            {
                Products = product,
                Brands = brands,
                Categories = cates,
                Carts = carts
            };
        }
    }
}
