using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.Models.Product;
using SixLabors.ImageSharp;
using System.Data;
using System.Collections.Immutable;
using CRUD_asp.netMVC.Models.ViewModels.Product;
using System.Globalization;
using System.Text;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using Microsoft.AspNetCore.Authorization;
using NuGet.Protocol;


namespace CRUD_asp.netMVC.Controllers
{
    [Authorize(Roles = "Admin")]
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
        [HttpGet]
        [Route("Admin"), Route("Admin/Index"), Route("admin/{page:int}")]
        public async Task<IActionResult> Index(int page = 1)
        {
            var product = _context.Products.AsNoTracking()
                       .Include(p => p.Brands)
                       .Include(p => p.Cate).OrderByDescending(p => p.ID);

            var paginationProduct = await PaginatedList<Products>.CreatePagAsync(product, page, 5);
            return View(paginationProduct);
        }

        // Find Porduct by keyword
        [HttpPost, ValidateAntiForgeryToken]
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
        [HttpGet]
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

        // GET: GetProducts/Create  -> data d/s duoc cap nhat qua phuong thuc ReloadViewModel
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return await ReloadViewModel(new ProductCreateViewModel());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateViewModel viewModel)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(ms => ms.Value.Errors.Count > 0)
                        .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );
                    return Json(new
                    {
                        success = false,
                        message = "Nhập thông tin sản phẩm của bạn !!!",
                        errors = errors
                    });
                }

                Products products = new Products()
                {
                    Name = viewModel.Name,
                    NormalizedName = RemoveDiacritics(viewModel.Name),
                    Description = viewModel.Description,
                    NormalizedDescription = RemoveDiacritics(viewModel.Description),
                    NewPrice = viewModel.NewPrice,
                    OldPrice = viewModel.OldPrice,
                    Quantity = viewModel.Quantity,
                    GenderID = viewModel.GenderID,
                    BrandID = viewModel.BrandID,
                    CateID = viewModel.CateID,
                    FeaturedID = viewModel.FeaturedID
                };

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
                        var fileUpLoadPath = Path.Combine(environment.WebRootPath, "images", "Products", nameFile).Replace("\\", "/");

                        using (var fileStream = new FileStream(fileUpLoadPath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        var imagePath = Path.Combine(nameFile).ToLower().Replace("\\", "/");

                        _context.Products.Add(products);
                        await _context.SaveChangesAsync(); // Them du lieu productID truoc khi them cac entity khac

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
                //return RedirectToAction(nameof(Index));
                return Json(new { success = true, message = "Thêm sản vào thành công. " });

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Loi: ", $"{ex.Message}");
                if (ex.InnerException != null)
                {
                    ModelState.AddModelError("Loi: ", $"{ex.InnerException.Message}");
                }
                await ReloadViewModel(viewModel);
                return Json(new { success = false, message = "Thêm sản vào thất bại. " });
            }
        }

        // GET: GetProducts/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

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

            // Gan du lieu vao edit thong qua truy van qua id product
            var viewModel = new ProductEditViewModel()
            {
                ID = Product.ID,
                Name = Product.Name,
                Description = Product.Description,
                PicturePath = Product.PicturePath,
                NewPrice = Product.NewPrice,
                OldPrice = Product.OldPrice,
                Quantity = Product.Quantity,
                GenderID = Product.GenderID,
                BrandID = Product.BrandID,
                CateID = Product.CateID,
                FeaturedID = Product.FeaturedID,

                ImagePaths = Product.ProductImages?.Select(p => p.PathNameImage).ToList() ?? new List<string>(),
                SelectedMaterialID = Product.ProductMaterial?.Select(p => p.MaterialID).ToArray() ?? Array.Empty<int>(),
                SelectedColorID = Product.ProductColor?.Select(p => p.ColorID).ToArray() ?? Array.Empty<int>(),
                SelectedStyleID = Product.ProductStyles?.Select(p => p.StyleID).ToArray() ?? Array.Empty<int>(),
                SelectedSeasonID = Product.ProductSeasons?.Select(p => p.SeasonID).ToArray() ?? Array.Empty<int>(),
                SelectedTagID = Product.ProductTags?.Select(p => p.TagID).ToArray() ?? Array.Empty<int>(),
                SelectedSizeID = Product.ProductSize?.Select(p => p.SizeID).ToArray() ?? Array.Empty<int>(),
            };

            return await ReloadViewModel(viewModel);
        }

        // Note: get all properties GetProducts class
        // POST: GetProducts/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
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
                    // Da reload data o method ReloadViewModel => ko can include dbset<entity>
                    var Product = await _context.Products
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

                    if (Product == null) return NotFound();
                    else
                    {
                        Product.Name = viewModel.Name;
                        Product.NormalizedName = RemoveDiacritics(viewModel.Name);
                        Product.Description = viewModel.Description;
                        Product.NormalizedDescription = RemoveDiacritics(viewModel.Description);
                        Product.NewPrice = viewModel.NewPrice;
                        Product.OldPrice = viewModel.OldPrice;
                        Product.Quantity = viewModel.Quantity;
                        Product.GenderID = viewModel.GenderID;
                        Product.BrandID = viewModel.BrandID;
                        Product.CateID = viewModel.CateID;
                        Product.FeaturedID = viewModel.FeaturedID;
                    }

                    if (viewModel.Picture != null && viewModel.Picture.Length > 0)
                    {
                        var oldImage = await _context.ProductImages.Where(p => p.ProductID == id).ToListAsync();
                        var deleteTask = oldImage.Select(async item =>
                        {
                            var oldPathPicture = Path.Combine(environment.WebRootPath, item.PathNameImage).Replace("\\", "/");
                            if (System.IO.File.Exists(oldPathPicture))
                            {
                                try
                                {
                                    await Task.Run(() => System.IO.File.Delete(oldPathPicture));
                                }
                                catch (IOException ex)
                                {
                                    ModelState.AddModelError("Picture", $"Lỗi khi xóa hình ảnh: {ex.Message}");
                                }
                            }

                        });
                        await Task.WhenAll(deleteTask); // dung lai den khi xoa het anh theo id product

                        _context.ProductImages.RemoveRange(oldImage);

                        var imageTasks = viewModel.Picture.Select(async (item, index) =>
                        {
                            var fileExtension = Path.GetExtension(item.FileName).ToLower();
                            var arrValidExtensions = new[] { ".jpg", ".png", ".webp", ".jpeg" };

                            if (!arrValidExtensions.Contains(fileExtension))
                            {
                                ModelState.AddModelError("Picture", "File không hợp lệ, vui lòng chọn file có đuôi jpg, png, jpeg, webp");
                                return null;
                            }

                            // add new image
                            var nameFile = Guid.NewGuid().ToString() + fileExtension;
                            var fileUploadPathImage = Path.Combine(environment.WebRootPath, "images", "Products", nameFile).Replace("\\", "/");

                            using (var image = await Image.LoadAsync(item.OpenReadStream()))
                            {
                                image.Mutate(x => x.Resize(new ResizeOptions
                                {
                                    Size = new SixLabors.ImageSharp.Size(800, 800),
                                    Mode = ResizeMode.Max
                                }));
                                await image.SaveAsync(fileUploadPathImage, new JpegEncoder { Quality = 80 });
                            }

                            using (var FileStream = new FileStream(fileUploadPathImage, FileMode.Create))
                            {
                                await item.CopyToAsync(FileStream);
                            }

                            var fileUploadImage = Path.Combine("", "", nameFile).ToLower().Replace("\\", "/");

                            return new ProductImages()
                            {
                                ProductID = Product.ID,
                                PathNameImage = fileUploadImage
                            };

                        }).ToList();


                        var newImageList = (await Task.WhenAll(imageTasks)).Where(img => img != null).ToList();
                        if (ModelState.IsValid)
                        {
                            _context.ProductImages.AddRange(newImageList);
                            viewModel.PicturePath = newImageList.FirstOrDefault()?.PathNameImage;
                        }
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

                    var ProductColorList = Product.ProductColor.Select(p => p.ColorID).ToList();
                    var selectColors = viewModel.SelectedColorID ?? Array.Empty<int>();
                    var addColor = selectColors.Except(ProductColorList).ToList();
                    var removeColor = ProductColorList.Except(selectColors).ToList();

                    _context.ProductColor.RemoveRange(Product.ProductColor.Where(p => removeColor.Contains(p.ColorID)));
                    _context.ProductColor.AddRange(
                        addColor.Select(idColor => new ProductColors()
                        {
                            ProductID = Product.ID,
                            ColorID = idColor
                        }));

                    var ProductSizeList = Product.ProductSize.Select(p => p.SizeID).ToList();
                    var selectSizes = viewModel.SelectedSizeID ?? Array.Empty<int>();
                    var addSize = selectSizes.Except(ProductSizeList).ToList();
                    var removeSize = ProductSizeList.Except(selectSizes).ToList();

                    _context.ProductSize.RemoveRange(Product.ProductSize.Where(p => removeSize.Contains(p.SizeID)));
                    _context.ProductSize.AddRange(
                        addSize.Select(idSize => new ProductSize()
                        {
                            ProductID = Product.ID,
                            SizeID = idSize
                        }));

                    var ProductStyleList = Product.ProductStyles.Select(p => p.StyleID).ToList();
                    var selectStyles = viewModel.SelectedStyleID ?? Array.Empty<int>();
                    var addStyle = selectStyles.Except(ProductStyleList).ToList();
                    var removeStyle = ProductStyleList.Except(selectStyles).ToList();

                    _context.ProductStyle.RemoveRange(Product.ProductStyles.Where(p => removeStyle.Contains(p.StyleID)));
                    _context.ProductStyle.AddRange(
                        addStyle.Select(idStyle => new ProductStyle()
                        {
                            ProductID = Product.ID,
                            StyleID = idStyle
                        }));

                    var ProductTagList = Product.ProductTags.Select(p => p.TagID).ToList();
                    var selectTags = viewModel.SelectedTagID ?? Array.Empty<int>();
                    var addCTag = selectTags.Except(ProductTagList).ToList();
                    var removeTag = ProductTagList.Except(selectTags).ToList();

                    _context.ProductTag.RemoveRange(Product.ProductTags.Where(p => removeTag.Contains(p.TagID)));
                    _context.ProductTag.AddRange(
                        addCTag.Select(idTag => new ProductTag()
                        {
                            ProductID = Product.ID,
                            TagID = idTag
                        }));

                    var ProductSeasonList = Product.ProductSeasons.Select(p => p.SeasonID).ToList();
                    var selectSeason = viewModel.SelectedSeasonID ?? Array.Empty<int>();
                    var addSeason = selectSeason.Except(ProductSeasonList).ToList();
                    var removeSeason = ProductSeasonList.Except(selectSeason).ToList();

                    _context.ProductSeason.RemoveRange(Product.ProductSeasons.Where(p => removeSeason.Contains(p.SeasonID)));
                    _context.ProductSeason.AddRange(
                        addSeason.Select(idSeason => new ProductSeason()
                        {
                            ProductID = Product.ID,
                            SeasonID = idSeason
                        }));

                    var ProductMaterialList = Product.ProductMaterial.Select(p => p.MaterialID).ToList();
                    var selectMaterials = viewModel.SelectedMaterialID ?? Array.Empty<int>();
                    var addMaterial = selectMaterials.Except(ProductMaterialList).ToList();
                    var removeMaterial = ProductMaterialList.Except(selectMaterials).ToList();

                    _context.ProductMaterial.RemoveRange(Product.ProductMaterial.Where(p => removeMaterial.Contains(p.MaterialID)));
                    _context.ProductMaterial.AddRange(
                        addMaterial.Select(idMaterial => new ProductMaterial()
                        {
                            ProductID = Product.ID,
                            MaterialID = idMaterial
                        }));


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
                    else throw;
                }
            }

            return await ReloadViewModel(viewModel);
        }

        public async Task<IActionResult> Brand()
        {
            var product = await _context.Brand.AsNoTracking().ToListAsync();
            return View(product);
        }

        // cap nhat du lieu trong database len cac select or option trong create, edit
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
