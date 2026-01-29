using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.Extensions.RenderViewGeneral;
using CRUD_asp.netMVC.Models.Cart;
using CRUD_asp.netMVC.Models.Product;
using CRUD_asp.netMVC.ViewModels.Home;
using CRUD_asp.netMVC.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.Globalization;
using System.Security.Claims;
using System.Text;

namespace CRUD_asp.netMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDBContext _dbContext;
        //private readonly IDbContextFactory<AppDBContext> _dbFactory;

        public ProductController(AppDBContext _context)
        {
            _dbContext = _context;
            //_dbFactory = dbFactory;
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

        // Hien thi danh sach phan trang san pham va phan trang thuong hieu
        [Route("Product"), Route("Product/Index"), HttpGet]
        public async Task<IActionResult> Index(int productPage = 1)
        {
            IQueryable<Products> products = _dbContext.Products.AsNoTracking()
                .Include(p => p.Brands)
                .Include(p => p.Cate)
                .Include(p => p.Gender)
                .Include(p => p.ProductImages).OrderByDescending(p => p.ID);

            var productCount = await products.CountAsync();
            ViewBag.ProductCount = productCount;

            // Pagination truyen tham so cho product, brand, category, cart
            var ViewModel = await CreatePaginationGeneral(products, productPage, 12).ConfigureAwait(false);

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
                                                                            .Include(p => p.Brands)
                                                                            .Where(p => p.Brands == brands);

            var productCount = await getPagProductByBrand.CountAsync();
            ViewBag.ProductCount = productCount;

            // Pagination truyen tham so cho product, brand, category, cart
            var ViewModel = await CreatePaginationGeneral(getPagProductByBrand, productPage, 6);

            return View(ViewModel);
        }

        /// <summary>
        /// Hien thi san pham cua Category qua id danh muc CateID
        /// </summary>
        /// <param name="CateID"></param>
        /// <param name="productPage"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> getProductByCate(int CateID = 1, int productPage = 1, bool featured = false, bool sales = false)
        {
            var cateID = await _dbContext.Category.AsNoTracking().FirstOrDefaultAsync(p => p.ID == CateID);

            ViewData["cateID"] = CateID;

            if (cateID == null) return RedirectToAction(nameof(Index));

            bool isSales = true;
            bool isFeatured = true;
            IQueryable<Products> getPagProductByCate;

            if (featured && !sales)
            {
                if (!featured)
                {
                    getPagProductByCate = _dbContext.Products.AsNoTracking()
                       .Include(p => p.Cate)
                       .Where(p => p.CateID == CateID);

                    isFeatured = false;
                    ViewBag.NameAction = "Featured";
                }
                else
                {
                    getPagProductByCate = _dbContext.Products.AsNoTracking()
                                                      .Include(p => p.Brands)
                                                      .Include(p => p.Cate)
                                                      .Include(p => p.Gender)
                                                      .Include(p => p.ProductImages)
                                                      .Where(p => p.CateID == CateID && p.FeaturedID == 1);
                    ViewBag.NameAction = "Featured";
                }
            }
            else if (!featured && sales)
            {
                if (!sales)
                {
                    getPagProductByCate = _dbContext.Products.AsNoTracking()
                       .Include(p => p.Cate)
                       .Where(p => p.CateID == CateID);

                    isSales = false;
                    ViewBag.NameAction = "Sales";
                }
                else
                {
                    getPagProductByCate = _dbContext.Products.AsNoTracking()
                                                     .Include(p => p.Brands)
                                                     .Include(p => p.Cate)
                                                     .Include(p => p.Gender)
                                                     .Include(p => p.ProductImages)
                                                     .Where(p => p.CateID == CateID && p.OldPrice > 0);
                    ViewBag.NameAction = "Sales";
                }
            }
            else getPagProductByCate = _dbContext.Products.AsNoTracking()
                                                       .Include(p => p.Cate)
                                                       .Where(p => p.CateID == CateID);

            var productCount = await getPagProductByCate.CountAsync();
            ViewBag.ProductCount = productCount;

            // Pagination truyen tham so cho product, brand, category, cart
            var ViewModel = await CreatePaginationGeneral(getPagProductByCate, productPage, 8);

            if (featured && !sales)
            {
                ViewModel.IsFeatured = isFeatured;
            }
            else if (!featured && sales) ViewModel.IsSales = isSales;

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

        [HttpGet] // Loc danh sach san pham nguoi dung
        public async Task<IActionResult> FilterProduct(string actionName, int cateID, string filter, int productPage = 1)
        {
            try
            {
                IQueryable<Products> productList;
                var viewModel = new getPaginationByProductViewModel();
                if (cateID < 1)
                {
                    productList = _dbContext.Products.AsNoTracking();
                }
                else productList = _dbContext.Products.Where(p => p.CateID == cateID).AsNoTracking();

                if (actionName == "Sales")
                {
                    productList = productList.Include(p => p.Brands)
                                                     .Include(p => p.Cate)
                                                     .Include(p => p.Gender)
                                                     .Include(p => p.ProductImages)
                                                     .Where(p => p.OldPrice > 0);
                }
                else if (actionName == "Featured")
                {
                    productList = productList.Include(p => p.Brands)
                                                      .Include(p => p.Cate)
                                                      .Include(p => p.Gender)
                                                      .Include(p => p.ProductImages)
                                                      .Where(p => p.FeaturedID == 1);
                }

                var orderPaid = await _dbContext.Orders.Where(p => p.Status.Equals("Paid")).ToListAsync().ConfigureAwait(false);
                var orderListPaid = await _dbContext.OrderDetail.Where(p => orderPaid.Select(s => s.ID).Contains(p.OrderID)).ToListAsync().ConfigureAwait(false);

                if (filter.Equals("sortNew", StringComparison.OrdinalIgnoreCase))
                {
                    productList = productList.OrderBy(p => p.Created);
                }
                else if (filter.Equals("sortBest", StringComparison.OrdinalIgnoreCase))
                {
                    productList = productList.Where(p => orderListPaid.Select(s => s.ProductID).Contains(p.ID));
                }
                else if (filter.Equals("sortHighToLow", StringComparison.OrdinalIgnoreCase))
                {
                    productList = productList.OrderByDescending(p => p.NewPrice);
                }
                else productList = productList.OrderBy(p => p.NewPrice);

                viewModel = await CreatePaginationGeneral(productList, productPage, 12);
                var countProduct = productList.Count();

                string html = await this.RenderViewAsync("_ProductFiltersPartial", viewModel, true);
                return Json(new { html, countProduct });
            }
            catch (Exception ex)
            {
                return Json(new { err = ex.Message });
            }
        }

        /// <summary>
        /// Hien thi chi tiet cua san pham theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ProductDetail(int id)
        {
            try
            {
                var product = await _dbContext.Products.AsNoTracking()
               .Include(p => p.Brands)
               .Include(p => p.Cate)
               .Include(p => p.Gender)
               .Include(p => p.ProductImages)
               .Include(p => p.ProductQty).ThenInclude(p => p.Color)
               .Include(p => p.ProductQty).ThenInclude(p => p.Size)
               .Include(p => p.ProductSize).ThenInclude(p => p.Size)
               .Include(p => p.ProductColor).ThenInclude(p => p.Color)
               .Include(p => p.ProductMaterial).ThenInclude(p => p.Material)
               .Include(p => p.ProductStyles).ThenInclude(p => p.Style)
               .Include(p => p.ProductSeasons).ThenInclude(p => p.Season)
               .Include(p => p.ProductTags).ThenInclude(p => p.Tag)
               .FirstOrDefaultAsync(p => p.ID == id);

                if (product == null) return NotFound();

                Random rand = new Random();

                var userID = User.Identity.IsAuthenticated ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0") : 0;

                var brandList = await _dbContext.Brand.AsNoTracking().ToListAsync().ConfigureAwait(false);
                var cateList = await _dbContext.Category.AsNoTracking().ToListAsync().ConfigureAwait(false);
                var orderPaid = await _dbContext.OrderDetail.Where(p => p.ProductID == id).SumAsync(p => p.Quantity).ConfigureAwait(false);
                var carts = await _dbContext.Carts.Where(p => p.UserID == userID).ToListAsync().ConfigureAwait(false);
                var relaterdProducts = await _dbContext.Products.Where(p => p.ID != id && p.CateID == product.CateID).ToListAsync().ConfigureAwait(false);

                if (relaterdProducts.Count > 0)
                {
                    relaterdProducts = relaterdProducts.OrderBy(p => rand.Next()).Take(4).ToList();
                }
                else relaterdProducts = new List<Products>();

                GeneralProduct_ListCateBrand ViewModel = new()
                {
                    Product = product,
                    Products = relaterdProducts,
                    Brands = brandList,
                    Categories = cateList,
                    Carts = carts,
                    Sold = orderPaid,
                };

                ViewData["cart"] = carts.Count;
                return View(ViewModel);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet] // Hien thi danh sach san pham noi bat
        public async Task<IActionResult> Sales(int? cateID, int productPage = 1, bool sales = false)
        {
            IQueryable<Products> products;
            if (cateID.HasValue || sales)
            {
                products = _dbContext.Products.AsNoTracking()
                  .Include(p => p.Brands)
                  .Include(p => p.Cate)
                  .Include(p => p.Gender)
                  .Include(p => p.ProductImages)
                  .Where(p => p.CateID == cateID && p.OldPrice > 0);

                ViewBag.IsSales = true;
                ViewBag.CateID = cateID ?? 0;
            }
            else
            {
                products = _dbContext.Products.AsNoTracking()
                      .Include(p => p.Brands)
                      .Include(p => p.Cate)
                      .Include(p => p.Gender)
                      .Include(p => p.ProductImages)
                      .Where(p => p.OldPrice > 0);

                ViewBag.IsSales = false;
            }

            var productCount = await products.CountAsync();
            ViewBag.ProductCount = productCount;
            ViewBag.NameAction = "Sales";
            //var pagProduct = await PaginatedList<Products>.CreatePagAsync(products, productPage, 16);

            // Pagination truyen tham so cho product, brand, category, cart
            var ViewModel = await CreatePaginationGeneral(products, productPage, 12);

            return View("ProductSaleProducts", ViewModel);
        }

        [HttpGet] // Hien thi danh sach san pham noi bat
        public async Task<IActionResult> ProductFeatured(int? cateID, int productPage = 1, bool featured = false)
        {
            IQueryable<Products> products;
            if (cateID.HasValue || featured)
            {
                products = _dbContext.Products.AsNoTracking()
                  .Include(p => p.Brands)
                  .Include(p => p.Cate)
                  .Include(p => p.Gender)
                  .Include(p => p.ProductImages)
                  .Where(p => p.CateID == cateID && p.FeaturedID == 1);

                ViewBag.IsFeatured = true;
                ViewBag.CateID = cateID ?? 0;
            }
            else
            {
                products = _dbContext.Products.AsNoTracking()
                  .Include(p => p.Brands)
                  .Include(p => p.Cate)
                  .Include(p => p.Gender)
                  .Include(p => p.ProductImages)
                  .Where(p => p.FeaturedID == 1);

                ViewBag.IsFeatured = false;
            }

            var productCount = await products.CountAsync();
            ViewBag.ProductCount = productCount;
            ViewBag.NameAction = "Featured";

            var pagProduct = await PaginatedList<Products>.CreatePagAsync(products, productPage, 16);

            // Pagination truyen tham so cho product, brand, category, cart
            var ViewModel = await CreatePaginationGeneral(products, productPage, 12);

            return View("ProductFeatureList", ViewModel);
        }

        // Phuong thuc phan trang san pham chung
        public async Task<getPaginationByProductViewModel> CreatePaginationGeneral(IQueryable<Products> Products, int pageCurrent, int pageCount)
        {
            var brands = _dbContext.Brand.AsNoTracking();
            var cates = _dbContext.Category.AsNoTracking();

            var brandPag = await PaginatedList<Brand>.CreatePagAsync(brands, 1, brands.Count());
            var catePag = await PaginatedList<Category>.CreatePagAsync(cates, 1, cates.Count());
            var productPag = await PaginatedList<Products>.CreatePagAsync(Products, pageCurrent, pageCount);

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
