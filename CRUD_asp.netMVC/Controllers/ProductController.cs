using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.Models.Cart;
using CRUD_asp.netMVC.Models.Product;
using CRUD_asp.netMVC.ViewModels.Home;
using CRUD_asp.netMVC.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Tls;
using System.Globalization;
using System.Security.Claims;
using System.Text;

namespace CRUD_asp.netMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDBContext _dbContext;
        //private readonly IDbContextFactory<AppDBContext> dbContextFactory;

        public ProductController(AppDBContext _context)
        {
            _dbContext = _context;
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

                IQueryable<Products> products = _dbContext.Products.AsNoTracking()
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
            IQueryable<Products> products = _dbContext.Products.AsNoTracking()
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
            var brands = await _dbContext.Brand.AsNoTracking().FirstOrDefaultAsync(p => p.ID == brandID);

            ViewData["brandID"] = brandID;
            ViewData["image"] = brands.PicturePath;

            if (brands == null)
            {
                return RedirectToAction(nameof(Index));
            }

            IQueryable<Products> getPagProductByBrand = _dbContext.Products.AsNoTracking()
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
        public async Task<IActionResult> getProductByCate(int CateID = 1, int productPage = 1, bool featured = false)
        {
            var cateID = await _dbContext.Category.AsNoTracking().FirstOrDefaultAsync(p => p.ID == CateID);

            ViewData["cateID"] = CateID;

            if (cateID == null)
            {
                return RedirectToAction(nameof(Index));
            }

            bool isFeatured = true;
            IQueryable<Products> getPagProductByCate;
            if (!featured)
            {
                getPagProductByCate = _dbContext.Products.AsNoTracking()
                   .Include(p => p.Cate)
                   .Where(p => p.CateID == CateID);

                isFeatured = false;
            }
            else
            {
                getPagProductByCate = _dbContext.Products.AsNoTracking()
                   .Include(p => p.Cate)
                   .Where(p => p.CateID == CateID && p.FeaturedID == 1);
            }

            var productCount = await getPagProductByCate.CountAsync();
            ViewBag.ProductCount = productCount;

            // Pagination truyen tham so cho product, brand, category, cart
            var ViewModel = await CreatePaginationGeneral(getPagProductByCate, productPage, 6);
            ViewModel.IsFeatured = isFeatured;

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
            var cates = await _dbContext.Category.AsNoTracking().FirstOrDefaultAsync(p => p.ID == CateID);
            var brands = await _dbContext.Brand.AsNoTracking().FirstOrDefaultAsync(p => p.ID == brandID);
            var cart = await _dbContext.Carts.AsNoTracking().ToListAsync();

            ViewData["cateID"] = CateID;
            ViewData["brandID"] = brandID;
            ViewData["image"] = brands.PicturePath;

            if (cates == null)
            {
                return RedirectToAction(nameof(Index));
            }

            IQueryable<Products> getPagProductByCate_Brand = _dbContext.Products.AsNoTracking()
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
            var product = await _dbContext.Products.AsNoTracking()
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

            var brandList = await _dbContext.Brand.AsNoTracking().ToListAsync();

            var cateList = await _dbContext.Category.AsNoTracking().ToListAsync();

            var orderPaid = await _dbContext.OrderDetail.CountAsync(p => p.ProductID == id);

            var userID = User.Identity.IsAuthenticated ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0") : 0;
            var carts = await _dbContext.Carts.Where(p => p.UserID == userID).ToListAsync();

            ViewData["cart"] = carts.Count;

            GeneralProduct_ListCateBrand ViewModel = new()
            {
                Product = product,
                Brands = brandList,
                Categories = cateList,
                Carts = carts,
                Sold = orderPaid
            };

            return ViewModel != null ? View(ViewModel) : NotFound();
        }

        [HttpGet] // Hien thi danh sach san pham noi bat
        public async Task<IActionResult> ProductFeatured(int productPage = 1)
        {
            IQueryable<Products> products = _dbContext.Products.AsNoTracking()
                  .Include(p => p.Brands)
                  .Include(p => p.Cate)
                  .Include(p => p.Gender)
                  .Include(p => p.ProductImages)
                  .Where(p => p.FeaturedID == 1);

            var productCount = await products.CountAsync();
            ViewBag.ProductCount = productCount;

            var pagProduct = await PaginatedList<Products>.CreatePagAsync(products, productPage, 16);

            // Pagination truyen tham so cho product, brand, category, cart
            var ViewModel = await CreatePaginationGeneral(products, productPage, 12);

            return View("ProductFeatureList", ViewModel);
        }

        /// <summary>
        /// Phan trang san pham 
        /// </summary>
        /// <param name="Iqueryable"></param>
        /// <param name="pageCurrent"></param>
        /// <param name="pageCount"></param> 
        /// <param name="carts"></param>
        /// <returns></returns>
        public async Task<getPaginationByProductViewModel> CreatePaginationGeneral(IQueryable<Products> Products, int pageCurrent, int pageCount)
        {
            var brands = _dbContext.Brand.AsNoTracking();
            var cates = _dbContext.Category.AsNoTracking();

            var productPag = await PaginatedList<Products>.CreatePagAsync(Products, pageCurrent, pageCount);
            var brandPag = await PaginatedList<Brand>.CreatePagAsync(brands, 1, brands.Count());
            var catePag = await PaginatedList<Category>.CreatePagAsync(cates, 1, cates.Count());

            var carts = new List<AddToCart>();
            var userID = User.Identity.IsAuthenticated ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0") : 0;
            if (userID > 0)
            {
                carts = await _dbContext.Carts.Where(p => p.UserID == userID).ToListAsync();
            }

            ViewData["cart"] = carts.Count;

            return new getPaginationByProductViewModel
            {
                Products = productPag,
                Brands = brandPag,
                Categories = catePag,
                Carts = carts
            };
        }
    }
}
