using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.Models.Product;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using EFCoreSecondLevelCacheInterceptor;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using System.Diagnostics;
using System.Data;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Immutable;
using CRUD_asp.netMVC.Models.ViewModels.Product;
using System.Net.Quic;


namespace CRUD_asp.netMVC.Controllers
{
    public class AdminController : Controller
    {
        public readonly AppDBContext _context;
        private readonly IWebHostEnvironment environment;

        public AdminController(AppDBContext context, IWebHostEnvironment _environment)
        {
            _context = context;
            environment = _environment;
        }

        // GET: GetProducts
        public async Task<IActionResult> Index(int page = 1)
        {
            var product = _context.Products.AsNoTracking()
                       .Include(p => p.Brands)
                       .Include(p => p.Cate).OrderByDescending(p => p.ID);

            var paginationProduct = await PaginatedList<Products>.CreatePagAsync(product, page, 5);
            return View(paginationProduct);
        }

        // Find Porduct by keyword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(int page = 1, string keyword = "")
        {
            var product = _context.Products.AsNoTracking()
                .Include(p => p.Brands)
                .Include(p => p.Cate)
                .Where(p => p.Description.Contains(keyword) || p.Name.Contains(keyword));


            var paginationProduct = await PaginatedList<Products>.CreatePagAsync(product, page, 5);
            return View(paginationProduct);
        }

        // GET: GetProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Product = await _context.Products.AsNoTracking()
                .Include(p => p.Brands)
                .Include(p => p.Cate)
                .Include(p => p.Gender)
                .Include(p => p.Featured)
                .Include(p => p.ProductMaterial)
                .Include(p => p.ProductTags)
                .Include(p => p.ProductStyles)
                .Include(p => p.ProductColor)
                .Include(p => p.ProductSeasons)
                .Include(p => p.ProductSize)
                .FirstOrDefaultAsync(p => p.ID == id);

            if (Product == null)
            {
                return NotFound();
            }

            return View(Product);
        }

        // GET: GetProducts/Create  -> lay d/s co trong cac mqh 1-n n-n
        public async Task<IActionResult> Create()
        {
            var viewModel = new ProductCreateViewModel
            {
                BrandList = new SelectList(await _context.Brand.AsNoTracking().ToListAsync(), "ID", "Name"),
                CategoryList = new SelectList(await _context.Category.AsNoTracking().ToListAsync(), "ID", "Name"),
                GenderList = new SelectList(await _context.Gender.AsNoTracking().ToListAsync(), "ID", "Name"),
                MaterialList = new SelectList(await _context.Material.AsNoTracking().ToListAsync(), "ID", "Name"),
                ColorList = new SelectList(await _context.Color.AsNoTracking().ToListAsync(), "ID", "Name"),
                SizeList = new SelectList(await _context.Size.AsNoTracking().ToListAsync(), "ID", "Name"),
                TagList = new SelectList(await _context.Tag.AsNoTracking().ToListAsync(), "ID", "Name"),
                SeasonList = new SelectList(await _context.Season.AsNoTracking().ToListAsync(), "ID", "Name"),
                StyleList = new SelectList(await _context.Style.AsNoTracking().ToListAsync(), "ID", "Name")
            };

            return await ReloadViewModel(viewModel);
        }

        // POST: GetProducts/Create 
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Products products = new Products()
                    {
                        Name = viewModel.Name,
                        Description = viewModel.Description,
                        Price = viewModel.Price,
                        Quantity = viewModel.Quantity,
                        GenderID = viewModel.GenderID,
                        BrandID = viewModel.BrandID,
                        CateID = viewModel.CateID,
                        FeaturedID = viewModel.FeaturedID
                    };

                    _context.Add(products);
                    await _context.SaveChangesAsync();

                    if (viewModel.Picture != null && viewModel.Picture.Any())
                    {
                        bool IsFirstImage = true;
                        foreach (var file in viewModel.Picture)
                        {
                            var getPathExtentions = Path.GetExtension(file.FileName).ToLower();
                            var fileExtentions = new[] { ".jpg", ".png", ".jpeg", ".webp" };

                            if (!fileExtentions.Contains(getPathExtentions))
                            {
                                ModelState.AddModelError("Picture", "Không thể tải lên file này, vui lòng chọn file có đuôi jpg, png, jpeg, webp");
                                return await ReloadViewModel(viewModel);
                            }

                            var nameFile = Guid.NewGuid().ToString() + getPathExtentions;
                            var fileUpLoadPath = Path.Combine(environment.WebRootPath, "images", "GetProducts", nameFile).Replace("\\", "/");

                            using (var fileStream = new FileStream(fileUpLoadPath, FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                            }

                            var imagePath = Path.Combine("images", "GetProducts", nameFile).ToLower().Replace("\\", "/");

                            _context.ProductImages.Add(new ProductImages()
                            {
                                ProductID = products.ID,
                                PathNameImage = imagePath
                            });

                            if (IsFirstImage)
                            {
                                products.PicturePath = imagePath;
                                IsFirstImage = false;
                            }

                        }
                    }
                    if (viewModel.SelectedColorID != null && viewModel.SelectedColorID.Any())
                    {
                        foreach (var ColorID in viewModel.SelectedColorID)
                        {
                            _context.ProductColor.Add(new ProductColors()
                            {
                                ProductID = products.ID,
                                ColorID = ColorID
                            });
                        }
                    }

                    if (viewModel.SelectedSizeID != null && viewModel.SelectedSizeID.Any())
                    {
                        foreach (var SizeID in viewModel.SelectedSizeID)
                        {
                            _context.ProductSize.Add(new ProductSize()
                            {
                                ProductID = products.ID,
                                SizeID = SizeID
                            });
                        }
                    }

                    if (viewModel.SelectedMaterialID != null && viewModel.SelectedMaterialID.Any())
                    {
                        foreach (var MateID in viewModel.SelectedMaterialID)
                        {
                            _context.ProductMaterial.Add(new ProductMaterial()
                            {
                                ProductID = products.ID,
                                MaterialID = MateID
                            });
                        }
                    }

                    if (viewModel.SelectedSeasonID != null && viewModel.SelectedSeasonID.Any())
                    {
                        foreach (var SeasonID in viewModel.SelectedSeasonID)
                        {
                            _context.ProductSeason.Add(new ProductSeason()
                            {
                                ProductID = products.ID,
                                SeasonID = SeasonID
                            });
                        }
                    }

                    if (viewModel.SelectedStyleID != null && viewModel.SelectedStyleID.Any())
                    {
                        foreach (var StyleID in viewModel.SelectedStyleID)
                        {
                            _context.ProductStyle.Add(new ProductStyle()
                            {
                                ProductID = products.ID,
                                StyleID = StyleID
                            });
                        }
                    }


                    if (viewModel.SelectedTagID != null && viewModel.SelectedTagID.Any())
                    {
                        foreach (var TagID in viewModel.SelectedTagID)
                        {
                            _context.ProductTag.Add(new ProductTag()
                            {
                                ProductID = products.ID,
                                TagID = TagID
                            });
                        }
                    }


                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Loi: ", $"{ex.InnerException.Message}");
                    return await ReloadViewModel(viewModel);
                }

            }

            return await ReloadViewModel(viewModel);
        }

        // GET: GetProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var Product = await _context.Products.AsNoTracking()
                .Include(p => p.ProductMaterial)
                .Include(p => p.ProductTags)
                .Include(p => p.ProductStyles)
                .Include(p => p.ProductColor)
                .Include(p => p.ProductSeasons)
                .Include(p => p.ProductSize)
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.ID == id);

            if (Product == null)
            {
                return NotFound();
            }

            var viewModel = new ProductEditViewModel()
            {
                ID = Product.ID,
                Name = Product.Name,
                Description = Product.Description,
                PicturePath = Product.PicturePath,
                Price = Product.Price,
                Quantity = Product.Quantity,
                GenderID = Product.GenderID,
                BrandID = Product.BrandID,
                CateID = Product.CateID,
                FeaturedID = Product.FeaturedID,

                ImagePaths = await _context.ProductImages?.Select(p => p.PathNameImage).ToListAsync() ?? new List<string>(),

                // Lay cac gia tri phan tu co trong mang hien thi vao the <select>
                SelectedMaterialID = _context.Material?.Select(p => p.ID).ToArray() ?? Array.Empty<int>(),
                SelectedColorID = _context.Color?.Select(p => p.ID).ToArray() ?? Array.Empty<int>(),
                SelectedStyleID = _context.Style?.Select(p => p.ID).ToArray() ?? Array.Empty<int>(),
                SelectedSeasonID = _context.Season?.Select(p => p.ID).ToArray() ?? Array.Empty<int>(),
                SelectedTagID = _context.Tag?.Select(p => p.ID).ToArray() ?? Array.Empty<int>(),
                SelectedSizeID = _context.Size?.Select(p => p.ID).ToArray() ?? Array.Empty<int>(),

                // mqh 1-n
                BrandList = new SelectList(await _context.Brand.AsNoTracking().ToListAsync(), "ID", "Name", Product.BrandID),
                CategoryList = new SelectList(await _context.Category.AsNoTracking().ToListAsync(), "ID", "Name", Product.CateID),
                GenderList = new SelectList(await _context.Gender.AsNoTracking().ToListAsync(), "ID", "Name", Product.GenderID),

                // mqh n-n
                MaterialList = new SelectList(await _context.Material.AsNoTracking().ToListAsync(), "ID", "Name"),
                ColorList = new SelectList(await _context.Brand.AsNoTracking().ToListAsync(), "ID", "Name"),
                SizeList = new SelectList(await _context.Size.AsNoTracking().ToListAsync(), "ID", "Name"),
                StyleList = new SelectList(await _context.Style.AsNoTracking().ToListAsync(), "ID", "Name"),
                TagList = new SelectList(await _context.Tag.AsNoTracking().ToListAsync(), "ID", "Name"),
                SeasonList = new SelectList(await _context.Season.AsNoTracking().ToListAsync(), "ID", "Name"),
            };

            return await ReloadViewModel(viewModel);
        }

        // Tuan note: get all properties GetProducts class
        // POST: GetProducts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductEditViewModel viewModel)
        {
            if (id != viewModel.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var Product = await _context.Products
                        .Include(p => p.ProductMaterial)
                        .Include(p => p.ProductTags)
                        .Include(p => p.ProductStyles)
                        .Include(p => p.ProductColor)
                        .Include(p => p.ProductSeasons)
                        .Include(p => p.ProductSize)
                        .Include(p => p.ProductImages)
                        .FirstOrDefaultAsync(p => p.ID == id);

                    if (Product == null) return NotFound();
                    else
                    {
                        Product.Name = viewModel.Name;
                        Product.Description = viewModel.Description;
                        Product.Price = viewModel.Price;
                        Product.Quantity = viewModel.Quantity;
                        Product.GenderID = viewModel.GenderID;
                        Product.BrandID = viewModel.BrandID;
                        Product.CateID = viewModel.CateID;
                        Product.FeaturedID = viewModel.FeaturedID;
                    }


                    if (viewModel.Picture != null && viewModel.Picture.Length > 0)
                    {
                        // kiem tra anh ton tai chua
                        var oldPicture = Product.ProductImages.ToList();
                        foreach (var image in oldPicture)
                        {
                            var oldFilePicture = Path.Combine(environment.WebRootPath, image.PathNameImage);

                            if (System.IO.File.Exists(oldFilePicture))
                            {
                                try
                                {
                                    System.IO.File.Delete(oldFilePicture);
                                }
                                catch (IOException)
                                {
                                    ModelState.AddModelError("Picture", "Lỗi xóa hình ảnh (line 338): " + oldFilePicture);
                                }
                            }

                        }
                        _context.ProductImages.RemoveRange(oldPicture);

                        bool isFirstImage = true;
                        // kiem tra duoi anh
                        foreach (var file in viewModel.Picture)
                        {
                            if (file != null)
                            {
                                var fileExtention = Path.GetExtension(file.FileName).ToLower();
                                var ValidExtentions = new[] { ".jpg", ".png", ".jpeg", ".webp" };

                                if (!ValidExtentions.Contains(fileExtention))
                                {
                                    ModelState.AddModelError("picture", "File không hợp lệ, vui lòng chọn file có đuôi jpg, png, jpeg, webp");
                                    return await ReloadViewModel(viewModel);
                                }

                                #region Picture
                                // tao ten anh
                                var nameFile = Guid.NewGuid().ToString() + fileExtention;
                                var fileUploadPath = Path.Combine(environment.WebRootPath, "images", "Products", nameFile).Replace("\\", "/");

                                // ghi tep moi
                                using (var fileStream = new FileStream(fileUploadPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
                                {
                                    await file.CopyToAsync(fileStream);
                                }

                                var fileImage = Path.Combine("images", "Products", nameFile).Replace("\\", "/");

                                _context.ProductImages.Add(new ProductImages()
                                {
                                    ProductID = Product.ID,
                                    PathNameImage = fileImage

                                });

                                if (isFirstImage)
                                {
                                    viewModel.PicturePath = fileImage;
                                    isFirstImage = false;
                                }
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        var oldPicture = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.ID == id);

                        if (oldPicture != null)
                        {
                            viewModel.PicturePath = oldPicture.PicturePath;
                        }
                    }

                    Product.PicturePath = viewModel.PicturePath;

                    _context.ProductMaterial.RemoveRange(Product.ProductMaterial);
                    _context.ProductColor.RemoveRange(Product.ProductColor);
                    _context.ProductSeason.RemoveRange(Product.ProductSeasons);
                    _context.ProductSize.RemoveRange(Product.ProductSize);
                    _context.ProductStyle.RemoveRange(Product.ProductStyles);
                    _context.ProductTag.RemoveRange(Product.ProductTags);

                    if (viewModel.SelectedColorID != null && viewModel.SelectedColorID.Any())
                    {
                        foreach (var MateID in viewModel.SelectedColorID)
                        {
                            _context.ProductColor.Add(new ProductColors()
                            {
                                ProductID = Product.ID,
                                ColorID = MateID
                            });
                        }
                    }

                    if (viewModel.SelectedSizeID != null && viewModel.SelectedSizeID.Any())
                    {
                        foreach (var SizeID in viewModel.SelectedSizeID)
                        {
                            _context.ProductSize.Add(new ProductSize()
                            {
                                ProductID = Product.ID,
                                SizeID = SizeID
                            });
                        }
                    }

                    if (viewModel.SelectedMaterialID != null && viewModel.SelectedMaterialID.Any())
                    {
                        foreach (var MateID in viewModel.SelectedMaterialID)
                        {
                            _context.ProductMaterial.Add(new ProductMaterial()
                            {
                                ProductID = Product.ID,
                                MaterialID = MateID
                            });
                        }
                    }

                    if (viewModel.SelectedSeasonID != null && viewModel.SelectedSeasonID.Any())
                    {
                        foreach (var SeasonID in viewModel.SelectedSeasonID)
                        {
                            _context.ProductSeason.Add(new ProductSeason()
                            {
                                ProductID = Product.ID,
                                SeasonID = SeasonID
                            });
                        }
                    }

                    if (viewModel.SelectedStyleID != null && viewModel.SelectedStyleID.Any())
                    {
                        foreach (var StyleID in viewModel.SelectedStyleID)
                        {
                            _context.ProductStyle.Add(new ProductStyle()
                            {
                                ProductID = Product.ID,
                                StyleID = StyleID
                            });
                        }
                    }


                    if (viewModel.SelectedTagID != null && viewModel.SelectedTagID.Any())
                    {
                        foreach (var TagID in viewModel.SelectedTagID)
                        {
                            _context.ProductTag.Add(new ProductTag()
                            {
                                ProductID = Product.ID,
                                TagID = TagID
                            });
                        }
                    }

                    _context.Update(Product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsExists(viewModel.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return await ReloadViewModel(viewModel);
        }

        public async Task<IActionResult> Brand()
        {
            var product = await _context.Brand.AsNoTracking().ToListAsync();
            return View(product);
        }

        public async Task<IActionResult> ReloadViewModel(IProductGeneralViewModel IviewModel)
        {
            // mqh 1-n
            IviewModel.BrandList = new SelectList(await _context.Brand.AsNoTracking().ToListAsync(), "ID", "Name", IviewModel.BrandID);
            IviewModel.CategoryList = new SelectList(await _context.Category.AsNoTracking().ToListAsync(), "ID", "Name", IviewModel.CateID);
            IviewModel.GenderList = new SelectList(await _context.Gender.AsNoTracking().ToListAsync(), "ID", "Name", IviewModel.GenderID);
            IviewModel.FeaturedList = new SelectList(await _context.Featured.AsNoTracking().ToListAsync(), "ID", "Name", IviewModel.FeaturedID);

            // mqh n-n
            IviewModel.MaterialList = new SelectList(await _context.Material.AsNoTracking().ToListAsync(), "ID", "Name");
            IviewModel.SeasonList = new SelectList(await _context.Season.AsNoTracking().ToListAsync(), "ID", "Name");
            IviewModel.TagList = new SelectList(await _context.Tag.AsNoTracking().ToListAsync(), "ID", "Name");
            IviewModel.StyleList = new SelectList(await _context.Style.AsNoTracking().ToListAsync(), "ID", "Name");
            IviewModel.SizeList = new SelectList(await _context.Size.AsNoTracking().ToListAsync(), "ID", "Name");
            IviewModel.ColorList = new SelectList(await _context.Color.AsNoTracking().ToListAsync(), "ID", "Name");

            return View(IviewModel);
        }

        // GET: GetProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .FirstOrDefaultAsync(m => m.ID == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // POST: GetProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var products = await _context.Products.FindAsync(id);
            if (products != null)
            {
                _context.Products.Remove(products);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductsExists(int id)
        {
            return _context.Products.Any(e => e.ID == id);
        }
    }
}
