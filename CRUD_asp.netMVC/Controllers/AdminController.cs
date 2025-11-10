using CRUD_asp.netMVC.Controllers.Extentions;
using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.Models.Product;
using CRUD_asp.netMVC.ViewModels.Admin;
using CRUD_asp.netMVC.ViewModels.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Elfie.Model.Tree;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Org.BouncyCastle.Tls;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Color = CRUD_asp.netMVC.Models.Product.Color;
using Size = CRUD_asp.netMVC.Models.Product.Size;


namespace CRUD_asp.netMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDBContext _dbContext;
        private readonly IWebHostEnvironment _environment;
        private readonly IDbContextFactory<AppDBContext> _dbFactory;

        public AdminController(AppDBContext context, IWebHostEnvironment environment, IDbContextFactory<AppDBContext> dbFactory)
        {
            _dbContext = context;
            _environment = environment;
            _dbFactory = dbFactory;
        }

        [HttpGet]
        [Route("Admin"), Route("Admin/Index"), Route("Admin/{page:int}")]
        public async Task<IActionResult> Index(int page = 1)
        {
            var product = _dbContext.Products.AsNoTracking()
                       .Include(p => p.Brands)
                       .Include(p => p.Cate).OrderByDescending(p => p.ID);

            var paginationProduct = await PaginatedList<Products>.CreatePagAsync(product, page, 5);
            return View(paginationProduct);
        }

        // Find Porduct by keyword
        [HttpPost]
        public async Task<IActionResult> Index(int page = 1, string keyword = "")
        {
            var product = _dbContext.Products.AsNoTracking()
                .Include(p => p.Brands)
                .Include(p => p.Cate)
                .Where(p => p.Description.Contains(keyword) || p.Name.Contains(keyword));


            var paginationProduct = await PaginatedList<Products>.CreatePagAsync(product, page, 5);
            return View(paginationProduct);
        }

        [HttpGet]// GET: GetProducts/DetailProduct/5
        public async Task<IActionResult> DetailProduct(int? id)
        {
            if (id == null) return NotFound();

            var Product = await _dbContext.Products.AsNoTracking()
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
                .Include(p => p.ProductQty)
                .FirstOrDefaultAsync(p => p.ID == id);

            if (Product == null) return NotFound();

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

        [HttpGet] // data d/s duoc cap nhat qua phuong thuc ReloadViewModel
        public async Task<IActionResult> CreateProduct()
        {
            return await ReloadViewModel(new ProductGeneralViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductGeneralViewModel viewModel)
        {
            try
            {
                var getTempProduct = HttpContext.Session.GetString("TempProductQty");
                var productQtyList = string.IsNullOrEmpty(getTempProduct) ? new List<TempProductQty>() : JsonSerializer.Deserialize<List<TempProductQty>>(getTempProduct);

                viewModel.TempProductQty = productQtyList ?? new List<TempProductQty>();

                if (viewModel.Picture == null)
                {
                    ModelState.AddModelError(nameof(viewModel.Picture), "Cần thêm ít nhất 1 hình ảnh sản phẩm. ");
                }
                else ModelState.Remove(nameof(viewModel.Picture));

                if (viewModel.TempProductQty.Count == 0)
                {
                    ModelState.AddModelError(nameof(viewModel.TempProductQty), "Cần cập nhật số lượng sản phẩm ít nhất 1 mục.");
                }
                else ModelState.Remove(nameof(viewModel.TempProductQty));

                if (!ModelState.IsValid)
                {
                    var allErrors = ModelState
                                    .Where(e => e.Value.Errors.Count > 0)
                                    .Select(e => new { Field = e.Key, Errors = e.Value.Errors.Select(er => er.ErrorMessage) })
                                    .ToList();

                    return Json(new { success = false, message = "Loi valid", errors = allErrors });
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
                    string nameFile = "";
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

                        nameFile = Guid.NewGuid().ToString() + getPathExtentions;
                        var fileUpLoadPath = Path.Combine(_environment.WebRootPath, "images", "Products", nameFile).Replace("\\", "/");

                        using (var image = await Image.LoadAsync(file.OpenReadStream()))
                        {
                            image.Mutate(x => x.Resize(new ResizeOptions
                            {
                                Size = new SixLabors.ImageSharp.Size(800, 800),
                                Mode = ResizeMode.Crop
                            }));
                            await image.SaveAsync(fileUpLoadPath, new WebpEncoder { Quality = 80 });
                        }

                        using (var fileStream = new FileStream(fileUpLoadPath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        var imagePath = Path.Combine(nameFile).ToLower().Replace("\\", "/");

                        // Them du lieu productID truoc khi them cac entity khac
                        if (products.ID == 0)
                        {
                            _dbContext.Products.Add(products);
                            await _dbContext.SaveChangesAsync();
                        }

                        _dbContext.ProductImages.Add(new ProductImages()
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


                    if (productQtyList.Count != 0)
                    {
                        foreach (var proQty in productQtyList)
                        {
                            await _dbContext.ProductQty.AddRangeAsync(new ProductQuantity()
                            {
                                ProductID = products.ID,
                                ColorID = proQty.ColorID,
                                SizeID = proQty.SizeID,
                                Quantity = proQty.Quantity
                            });
                        }

                        HttpContext.Session.Remove("TempProductQty");
                    }
                }

                if (viewModel.SelectedMaterialID != null && viewModel.SelectedMaterialID.Any())
                {
                    foreach (var MateID in viewModel.SelectedMaterialID)
                    {
                        _dbContext.ProductMaterial.Add(new ProductMaterial()
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
                        _dbContext.ProductSeason.Add(new ProductSeason()
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
                        _dbContext.ProductStyle.Add(new ProductStyle()
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
                        _dbContext.ProductTag.Add(new ProductTag()
                        {
                            ProductID = products.ID,
                            TagID = TagID
                        });
                    }
                }


                await _dbContext.SaveChangesAsync();
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
                return await ReloadViewModel(viewModel);
                //return Json(new { success = false, message = "Thêm sản vào thất bại. " });
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditProduct(int? id)
        {
            if (id == null) return NotFound();

            var Product = await _dbContext.Products.AsNoTracking()
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
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.ID == id);

            // Gan du lieu vao edit thong qua truy van qua id product
            var viewModel = new ProductGeneralViewModel()
            {
                ID = Product.ID,
                Name = Product.Name,
                Description = Product.Description,
                PicturePath = Product.PicturePath,
                NewPrice = Product.NewPrice,
                OldPrice = Product.OldPrice,
                Quantity = await _dbContext.ProductQty.Where(p => p.ProductID == id).SumAsync(p => p.Quantity),
                GenderID = Product.GenderID,
                BrandID = Product.BrandID,
                CateID = Product.CateID,
                FeaturedID = Product.FeaturedID,

                //Array.Empty<int>() -> [] (.net 8)
                SelectedMaterialID = Product.ProductMaterial?.Select(p => p.MaterialID).ToArray() ?? [],
                SelectedColorID = Product.ProductColor?.Select(p => p.ColorID).ToArray() ?? [],
                SelectedStyleID = Product.ProductStyles?.Select(p => p.StyleID).ToArray() ?? [],
                SelectedSeasonID = Product.ProductSeasons?.Select(p => p.SeasonID).ToArray() ?? [],
                SelectedTagID = Product.ProductTags?.Select(p => p.TagID).ToArray() ?? [],
                SelectedSizeID = Product.ProductSize?.Select(p => p.SizeID).ToArray() ?? [],
            };

            return await ReloadViewModel(viewModel);
        }

        // Note: get all properties GetProducts class
        // POST: GetProducts/EditProduct/5
        [HttpPost]
        public async Task<IActionResult> EditProduct(int id, ProductGeneralViewModel viewModel)
        {
            if (id != viewModel.ID)
            {
                return NotFound();
            }

            var productQtyList = _dbContext.ProductQty.Where(p => p.ProductID == id).ToList();
            viewModel.ProductQty = productQtyList ?? new List<ProductQuantity>();

            if (viewModel.ProductQty.Count == 0)
            {
                ModelState.AddModelError(nameof(viewModel.ProductQty), "Cần cập nhật số lượng sản phẩm ít nhất 1 mục.");
            }
            else ModelState.Remove(nameof(viewModel.ProductQty));

            if (!ModelState.IsValid)
            {
                var allErrors = ModelState
                                .Where(e => e.Value.Errors.Count > 0)
                                .Select(e => new { Field = e.Key, Errors = e.Value.Errors.Select(er => er.ErrorMessage) })
                                .ToList();

                return Json(new { success = false, message = "Loi valid", errors = allErrors });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var Product = await _dbContext.Products
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
                        var oldImage = await _dbContext.ProductImages.Where(p => p.ProductID == id).ToListAsync();
                        var deleteTask = oldImage.Select(async item =>
                        {
                            var oldPathPicture = Path.Combine(_environment.WebRootPath, item.PathNameImage).Replace("\\", "/");
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

                        _dbContext.ProductImages.RemoveRange(oldImage);

                        var imageTasks = viewModel.Picture.Select(async (item, index) =>
                        {
                            var fileExtension = Path.GetExtension(item.FileName).ToLower();
                            var arrValidExtensions = new[] { ".jpg", ".png", ".webp", ".jpeg" };

                            if (!arrValidExtensions.Contains(fileExtension))
                            {
                                ModelState.AddModelError("Picture", "File không hợp lệ, vui lòng chọn file có đuôi jpg, png, jpeg, webp");
                                return null;
                            }

                            var nameFile = Guid.NewGuid().ToString() + fileExtension;
                            var fileUploadPathImage = Path.Combine(_environment.WebRootPath, "images", "Products", nameFile).Replace("\\", "/");

                            using (var image = await Image.LoadAsync(item.OpenReadStream()))
                            {
                                image.Mutate(x => x.Resize(new ResizeOptions
                                {
                                    Size = new SixLabors.ImageSharp.Size(800, 800),
                                    Mode = ResizeMode.Crop
                                }));
                                await image.SaveAsync(fileUploadPathImage, new WebpEncoder { Quality = 75 });
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
                            _dbContext.ProductImages.AddRange(newImageList);
                            viewModel.PicturePath = newImageList.FirstOrDefault()?.PathNameImage;
                        }
                    }
                    else
                    {
                        var oldPicture = await _dbContext.Products.AsNoTracking().FirstOrDefaultAsync(p => p.ID == id);

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

                    _dbContext.ProductColor.RemoveRange(Product.ProductColor.Where(p => removeColor.Contains(p.ColorID)));
                    _dbContext.ProductColor.AddRange(
                        addColor.Select(idColor => new ProductColors()
                        {
                            ProductID = Product.ID,
                            ColorID = idColor
                        }));

                    var ProductSizeList = Product.ProductSize.Select(p => p.SizeID).ToList();
                    var selectSizes = viewModel.SelectedSizeID ?? Array.Empty<int>();
                    var addSize = selectSizes.Except(ProductSizeList).ToList();
                    var removeSize = ProductSizeList.Except(selectSizes).ToList();

                    _dbContext.ProductSize.RemoveRange(Product.ProductSize.Where(p => removeSize.Contains(p.SizeID)));
                    _dbContext.ProductSize.AddRange(
                        addSize.Select(idSize => new ProductSize()
                        {
                            ProductID = Product.ID,
                            SizeID = idSize
                        }));

                    var ProductStyleList = Product.ProductStyles.Select(p => p.StyleID).ToList();
                    var selectStyles = viewModel.SelectedStyleID ?? Array.Empty<int>();
                    var addStyle = selectStyles.Except(ProductStyleList).ToList();
                    var removeStyle = ProductStyleList.Except(selectStyles).ToList();

                    _dbContext.ProductStyle.RemoveRange(Product.ProductStyles.Where(p => removeStyle.Contains(p.StyleID)));
                    _dbContext.ProductStyle.AddRange(
                        addStyle.Select(idStyle => new ProductStyle()
                        {
                            ProductID = Product.ID,
                            StyleID = idStyle
                        }));

                    var ProductTagList = Product.ProductTags.Select(p => p.TagID).ToList();
                    var selectTags = viewModel.SelectedTagID ?? Array.Empty<int>();
                    var addCTag = selectTags.Except(ProductTagList).ToList();
                    var removeTag = ProductTagList.Except(selectTags).ToList();

                    _dbContext.ProductTag.RemoveRange(Product.ProductTags.Where(p => removeTag.Contains(p.TagID)));
                    _dbContext.ProductTag.AddRange(
                        addCTag.Select(idTag => new ProductTag()
                        {
                            ProductID = Product.ID,
                            TagID = idTag
                        }));

                    var ProductSeasonList = Product.ProductSeasons.Select(p => p.SeasonID).ToList();
                    var selectSeason = viewModel.SelectedSeasonID ?? Array.Empty<int>();
                    var addSeason = selectSeason.Except(ProductSeasonList).ToList();
                    var removeSeason = ProductSeasonList.Except(selectSeason).ToList();

                    _dbContext.ProductSeason.RemoveRange(Product.ProductSeasons.Where(p => removeSeason.Contains(p.SeasonID)));
                    _dbContext.ProductSeason.AddRange(
                        addSeason.Select(idSeason => new ProductSeason()
                        {
                            ProductID = Product.ID,
                            SeasonID = idSeason
                        }));

                    var ProductMaterialList = Product.ProductMaterial.Select(p => p.MaterialID).ToList();
                    var selectMaterials = viewModel.SelectedMaterialID ?? Array.Empty<int>();
                    var addMaterial = selectMaterials.Except(ProductMaterialList).ToList();
                    var removeMaterial = ProductMaterialList.Except(selectMaterials).ToList();

                    _dbContext.ProductMaterial.RemoveRange(Product.ProductMaterial.Where(p => removeMaterial.Contains(p.MaterialID)));
                    _dbContext.ProductMaterial.AddRange(
                        addMaterial.Select(idMaterial => new ProductMaterial()
                        {
                            ProductID = Product.ID,
                            MaterialID = idMaterial
                        }));


                    _dbContext.Update(Product);
                    await _dbContext.SaveChangesAsync();
                    //return RedirectToAction(nameof(Index));
                    return Json(new { success = true, message = "Thêm sản vào thành công. " });
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

        // cap nhat du lieu trong database len cac select or option trong create, edit
        public async Task<IActionResult> ReloadViewModel(IProductGeneralViewModel IviewModel)
        {
            var getTempProduct = HttpContext.Session.GetString("TempProductQty");
            var productQtyList = string.IsNullOrEmpty(getTempProduct) ? new List<TempProductQty>() : JsonSerializer.Deserialize<List<TempProductQty>>(getTempProduct);

            IviewModel.TempProductQty = productQtyList;

            // mqh 1-n
            IviewModel.BrandList = new SelectList(await _dbContext.Brand.AsNoTracking().ToListAsync(), "ID", "Name");
            IviewModel.CategoryList = new SelectList(await _dbContext.Category.AsNoTracking().ToListAsync(), "ID", "Name");
            IviewModel.GenderList = new SelectList(await _dbContext.Gender.AsNoTracking().ToListAsync(), "ID", "Name");
            IviewModel.FeaturedList = new SelectList(await _dbContext.Featured.AsNoTracking().ToListAsync(), "ID", "Name");

            // mqh n-n
            IviewModel.MaterialList = new SelectList(await _dbContext.Material.AsNoTracking().ToListAsync(), "ID", "Name");
            IviewModel.SeasonList = new SelectList(await _dbContext.Season.AsNoTracking().ToListAsync(), "ID", "Name");
            IviewModel.TagList = new SelectList(await _dbContext.Tag.AsNoTracking().ToListAsync(), "ID", "Name");
            IviewModel.StyleList = new SelectList(await _dbContext.Style.AsNoTracking().ToListAsync(), "ID", "Name");
            IviewModel.SizeList = new SelectList(await _dbContext.Size.AsNoTracking().ToListAsync(), "ID", "Name");
            IviewModel.ColorList = new SelectList(await _dbContext.Color.AsNoTracking().ToListAsync(), "ID", "Name");

            return View(IviewModel);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteProduct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _dbContext.Products
                .Include(p => p.Brands)
                .Include(p => p.Cate)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        [HttpPost, ActionName("DeleteProduct")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (id < 1) return NotFound();

            var productQtyExist = await _dbContext.ProductQty.Where(p => p.ProductID == id).ToListAsync();
            if (productQtyExist.Count > 0 && productQtyExist != null)
            {
                _dbContext.ProductQty.RemoveRange(productQtyExist);

                var products = await _dbContext.Products.FindAsync(id);
                if (products != null)
                {
                    _dbContext.Products.Remove(products);
                }
            }

            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductsExists(int id)
        {
            return _dbContext.Products.Any(e => e.ID == id);
        }

        [HttpGet] // Danh sach thuong hieu
        public async Task<IActionResult> BrandList(int page = 1)
        {
            var brand = _dbContext.Brand.AsNoTracking().OrderByDescending(p => p.ID);

            var paginationBrand = await PaginatedList<Brand>.CreatePagAsync(brand, page, 5);
            return View(paginationBrand);
        }

        [HttpGet] // Hien thi giao dien create
        public IActionResult CreateBrand() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBrand(Brand brand)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(er => er.Value.Errors.Count > 0)
                        .ToDictionary(k => k.Key, v => v.Value.Errors.Select(p => p.ErrorMessage).ToArray());

                    return Json(new
                    {
                        success = false,
                        message = "Cần nhập dữ liệu các ô trống !!!",
                        errors = errors
                    });
                }

                if (brand.Picture != null)
                {
                    string nameFile = "";

                    var getPathExtentions = Path.GetExtension(brand.Picture.FileName).ToLower();
                    var fileExtentions = new[] { ".jpg", ".png", ".jpeg", ".webp" };

                    if (!fileExtentions.Contains(getPathExtentions))
                    {
                        return Json(new
                        {
                            success = false,
                            message = "Không thể tải lên file này, vui lòng chọn file có đuôi jpg, png, jpeg, webp",
                            errors = new { Avatarnput = new[] { "Không thể tải lên file này, vui lòng chọn file có đuôi jpg, png, jpeg, webp" } }
                        });
                    }

                    nameFile = Guid.NewGuid().ToString() + getPathExtentions;
                    var fileUpLoadPath = Path.Combine(_environment.WebRootPath, "images", "Logo", nameFile).Replace("\\", "/");

                    using (var fileStream = new FileStream(fileUpLoadPath, FileMode.Create))
                    {
                        await brand.Picture.CopyToAsync(fileStream);
                    }

                    brand.PicturePath = Path.Combine("images", "logo", nameFile).ToLower().Replace("\\", "/");

                    _dbContext.Brand.Add(brand);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = $"Bạn cần thêm ảnh trước khi thêm thương hiệu. ",
                        errors = new { Picture = new[] { "Bạn cần thêm ảnh trước khi thêm thương hiệu này. " } }
                    });
                }

                return Json(new
                {
                    success = true,
                    message = $"thêm thương hiệu {brand.Name} thành công. "
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = $"Thương hiệu thêm không thành công. {ex.Message}"
                });
            }

        }

        [HttpGet] // Chinh sua thuong hieu
        public async Task<IActionResult> EditBrand(int id)
        {
            var Brand = await _dbContext.Brand.FindAsync(id);
            if (Brand == null)
            {
                return View();
            }

            return View(Brand);
        }


        [HttpPost]
        public async Task<IActionResult> EditBrand(Brand brand)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(er => er.Value.Errors.Count > 0)
                        .ToDictionary(k => k.Key, v => v.Value.Errors.Select(p => p.ErrorMessage).ToArray());

                    return Json(new
                    {
                        success = false,
                        message = "Cần nhập dữ liệu các ô trống !!!",
                        errors = errors
                    });
                }

                if (brand.Picture != null)
                {
                    string nameFile = "";

                    var getPathExtentions = Path.GetExtension(brand.Picture.FileName).ToLower();
                    var fileExtentions = new[] { ".jpg", ".png", ".jpeg", ".webp" };

                    if (!fileExtentions.Contains(getPathExtentions))
                    {
                        return Json(new
                        {
                            success = false,
                            message = "Không thể tải lên file này, vui lòng chọn file có đuôi jpg, png, jpeg, webp",
                            errors = new { PicturePath = new[] { "Không thể tải lên file này, vui lòng chọn file có đuôi jpg, png, jpeg, webp" } }
                        });
                    }

                    nameFile = Guid.NewGuid().ToString() + getPathExtentions;
                    var fileUpLoadPath = Path.Combine(_environment.WebRootPath, "images", "Logo", nameFile).Replace("\\", "/");

                    using (var fileStream = new FileStream(fileUpLoadPath, FileMode.Create))
                    {
                        await brand.Picture.CopyToAsync(fileStream);
                    }

                    brand.PicturePath = Path.Combine("images", "logo", nameFile).ToLower().Replace("\\", "/");

                }

                _dbContext.Brand.Update(brand);
                await _dbContext.SaveChangesAsync();


                return Json(new
                {
                    success = true,
                    message = $"Cập nhật thương hiệu {brand.Name} thành công. "
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = $"Thương hiệu cập nhật không thành công. {ex.Message}"
                });
            }
        }

        [HttpGet] // HIen thi chi tiet thuong hieu
        public async Task<IActionResult> DetailBrand(int id)
        {
            var brand = await BrandByID(id);

            return brand != null ? View(brand) : View();
        }

        [HttpGet]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            var brand = await BrandByID(id);

            return brand != null ? View(brand) : View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBrandByID(int id)
        {
            var brand = await _dbContext.Brand.FindAsync(id);
            if (brand != null)
            {
                var product = await _dbContext.Products.Where(p => p.BrandID == brand.ID).ToListAsync();

                _dbContext.Brand.Remove(brand);
                _dbContext.Products.RemoveRange(product);

                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(BrandList));
            }

            return View();
        }


        public async Task<Brand?> BrandByID(int id)
        {
            var brand = await _dbContext.Brand.FindAsync(id);

            if (brand == null) return null;

            return brand;
        }

        [HttpPost] // Xoa value thuoc tinh san pham 
        public async Task<IActionResult> DeletePropTValueForProduct(int id, string value, string type)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                if (id < 1) return NotFound();

                var productQtyList = await _dbContext.ProductQty.ToListAsync();

                var data = await DataTables(type);

                var idExistProp = data.FirstOrDefault(p => p.ID == id);
                var idExistQty = productQtyList.FirstOrDefault(p => p.ColorID == id || p.SizeID == id);
                if (idExistQty != null || idExistProp != null)
                {
                    ModelState.AddModelError("Name", $"{type} {value.ToLower()} đang được sử dụng !");
                }
                else
                {
                    string query = $@"
                            DELETE FROM {type}
                            WHERE ID = @id;";

                    var result = await _dbContext.Database.ExecuteSqlRawAsync(query, new SqlParameter("@id", id));

                    await transaction.CommitAsync();
                }

                var newList = await DataTables(type);
                var partial = await LoadProductQty(null, null, null);
                partial.ValueType = type;
                partial.Items = newList;

                string html = await this.RenderViewAsync("_ProductItemPartial", partial, true);

                if (!ModelState.IsValid)
                {
                    await transaction.RollbackAsync();

                    var Errors = ModelState
                                        .Where(e => e.Value.Errors.Count > 0)
                                        .Select(e => new { Field = e.Key, Errors = e.Value.Errors.Select(er => er.ErrorMessage) })
                                        .ToList();

                    return Json(new { success = false, html = html, errors = Errors });
                }

                var propList = newList.Select(p => new { ID = p.ID, Name = p.Name }).ToList();

                return Json(new { success = true, html, typeVal = type, propList });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Content($"ID Error: {ex.Message}, error detail: {ex.InnerException}");
            }
        }

        [HttpPost] // Cap nhat value thuoc tinh san pham 
        public async Task<IActionResult> UpdatePropTValueForProduct(int id, string value, string typeVal)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var dataList = await DataTables(typeVal);
                if (dataList != null)
                {
                    var dataExist = dataList.FirstOrDefault(p => p.ID == id);
                    if (dataList != null)
                    {
                        if (dataExist.Name.Equals(value, StringComparison.OrdinalIgnoreCase))
                        {
                            ModelState.AddModelError("Name", "Dữ liệu giống nhau !");
                        }
                        else
                        {
                            string sqlUpdateQty = $@"
                                        UPDATE {typeVal}
                                        SET Name = @value
                                        WHERE ID = @id;";

                            int result = await _dbContext.Database.ExecuteSqlRawAsync(sqlUpdateQty,
                                new SqlParameter("@value", value),
                                new SqlParameter("@id", id)
                                );

                            await transaction.CommitAsync();
                        }

                        var newList = await DataTables(typeVal);
                        var partial = await LoadProductQty(null, null, null);
                        partial.ValueType = typeVal;
                        partial.Items = newList;

                        string html = await this.RenderViewAsync("_ProductItemPartial", partial, true);

                        if (!ModelState.IsValid)
                        {
                            await transaction.RollbackAsync();

                            var Errors = ModelState
                                                .Where(e => e.Value.Errors.Count > 0)
                                                .Select(e => new { Field = e.Key, Errors = e.Value.Errors.Select(er => er.ErrorMessage) })
                                                .ToList();

                            return Json(new { success = false, html = html, errors = Errors });
                        }

                        var propList = newList.Select(p => new { ID = p.ID, Name = p.Name }).ToList();

                        return Json(new { success = true, html, typeVal, newList, propList });
                    }
                }

                return Json(new { success = false });
            }
            catch
            {
                await transaction.RollbackAsync();
                return BadRequest();
            }
        }

        // Hien thi ds theo kieu thuoc tinh san pham
        private async Task<List<IProductItemGeneral>> DataTables(string type)
        {
            return type switch
            {
                "Material" => await _dbContext.Material.AsNoTracking().ToListAsync<IProductItemGeneral>(),
                "Color" => await _dbContext.Color.AsNoTracking().ToListAsync<IProductItemGeneral>(),
                "Size" => await _dbContext.Size.AsNoTracking().ToListAsync<IProductItemGeneral>(),
                "Season" => await _dbContext.Season.AsNoTracking().ToListAsync<IProductItemGeneral>(),
                "Style" => await _dbContext.Style.AsNoTracking().ToListAsync<IProductItemGeneral>(),
                "Tag" => await _dbContext.Tag.AsNoTracking().ToListAsync<IProductItemGeneral>(),
                _ => new List<IProductItemGeneral>()
            };
        }

        [HttpPost] // Them value cho thuoc tinh san pham
        public async Task<IActionResult> AddPropTValueForProduct(string value, string typeVal)
        {
            try
            {
                object addValue = new object();
                var typeValList = new List<IProductItemGeneral>();

                var nameType = typeVal switch
                {
                    "Material" => "Chất liệu",
                    "Color" => "Màu sắc",
                    "Size" => "Kích cỡ",
                    "Season" => "Mùa",
                    "Style" => "Phong cách",
                    "Tag" => "Thể loại",
                    _ => "Thuộc tính"
                };

                nameType = nameType.ToLower();
                switch (typeVal)
                {
                    case "Material":

                        var materialExist = await _dbContext.Material.FirstOrDefaultAsync(m => m.Name.ToLower() == value.ToLower());

                        if (materialExist == null)
                        {
                            ModelState.Remove(nameof(materialExist.Name));

                            addValue = new Material() { Name = value };
                            await _dbContext.Material.AddAsync((Material)addValue);
                        }
                        else
                        {
                            ModelState.AddModelError(nameof(materialExist.Name), $"{value} đã tồn tại, cần thêm 1 {nameType} mới. ");
                            break;
                        }

                        await _dbContext.SaveChangesAsync();
                        typeValList = _dbContext.Material.Cast<IProductItemGeneral>().ToList();

                        break;

                    case "Tag":

                        var tagExist = await _dbContext.Tag.FirstOrDefaultAsync(m => m.Name.ToLower() == value.ToLower());

                        if (tagExist == null)
                        {
                            ModelState.Remove(nameof(tagExist.Name));

                            addValue = new Tag() { Name = value, Description = null };
                            await _dbContext.Tag.AddAsync((Tag)addValue);
                        }
                        else
                        {
                            ModelState.AddModelError(nameof(tagExist.Name), $"{value} đã tồn tại, cần thêm 1 {nameType} mới. ");
                            break;
                        }
                        await _dbContext.SaveChangesAsync();
                        typeValList = _dbContext.Tag.Cast<IProductItemGeneral>().ToList();

                        break;

                    case "Season":

                        var seasonExist = await _dbContext.Season.FirstOrDefaultAsync(m => m.Name.ToLower() == value.ToLower());

                        if (seasonExist == null)
                        {
                            ModelState.Remove(nameof(seasonExist.Name));

                            addValue = new Season() { Name = value };
                            await _dbContext.Season.AddAsync((Season)addValue);
                        }
                        else
                        {
                            ModelState.AddModelError(nameof(seasonExist.Name), $"{value} đã tồn tại, cần thêm 1 {nameType} mới. ");
                            break;
                        }
                        await _dbContext.SaveChangesAsync();
                        typeValList = _dbContext.Season.Cast<IProductItemGeneral>().ToList();

                        break;

                    case "Color":

                        var colorExist = await _dbContext.Color.FirstOrDefaultAsync(m => m.Name.ToLower() == value.ToLower());

                        if (colorExist == null)
                        {
                            ModelState.Remove(nameof(colorExist.Name));

                            addValue = new Color() { Name = value };
                            await _dbContext.Color.AddAsync((Color)addValue);
                        }
                        else
                        {
                            ModelState.AddModelError(nameof(colorExist.Name), $"{value} đã tồn tại, cần thêm 1 {nameType} mới. ");
                            break;
                        }
                        await _dbContext.SaveChangesAsync();
                        typeValList = _dbContext.Color.Cast<IProductItemGeneral>().ToList();

                        break;

                    case "Size":

                        var sizeExist = await _dbContext.Size.FirstOrDefaultAsync(m => m.Name.ToLower() == value.ToLower());

                        if (sizeExist == null)
                        {
                            ModelState.Remove(nameof(sizeExist.Name));

                            addValue = new Size() { Name = value };
                            await _dbContext.Size.AddAsync((Size)addValue);
                        }
                        else
                        {
                            ModelState.AddModelError(nameof(sizeExist.Name), $"{value} đã tồn tại, cần thêm 1 {nameType} mới. ");
                            break;
                        }
                        await _dbContext.SaveChangesAsync();
                        typeValList = _dbContext.Size.Cast<IProductItemGeneral>().ToList();

                        break;

                    default:

                        var styleExist = await _dbContext.Style.FirstOrDefaultAsync(m => m.Name.ToLower() == value.ToLower());

                        if (styleExist == null)
                        {
                            ModelState.Remove(nameof(styleExist.Name));

                            addValue = new Style() { Name = value };
                            await _dbContext.Style.AddAsync((Style)addValue);
                        }
                        else
                        {
                            ModelState.AddModelError(nameof(styleExist.Name), $"{value} đã tồn tại, cần thêm 1 {nameType} mới. ");
                            break;
                        }

                        await _dbContext.SaveChangesAsync();
                        typeValList = _dbContext.Style.Cast<IProductItemGeneral>().ToList();

                        break;
                }

                var partial = await LoadProductQty(null, null, null);
                partial.ValueType = typeVal;
                partial.Items = typeValList;

                var listStyle = await _dbContext.Style.ToListAsync();

                string html = await this.RenderViewAsync("_ProductItemPartial", partial, true);

                if (!ModelState.IsValid)
                {
                    var Errors = ModelState
                                    .Where(e => e.Value.Errors.Count > 0)
                                    .Select(e => new { Field = e.Key, Errors = e.Value.Errors.Select(er => er.ErrorMessage) })
                                    .ToList();

                    return Json(new { success = false, html = html, errors = Errors });
                }

                var propList = typeValList.Select(p => new { ID = p.ID, Name = p.Name }).ToList();

                return Json(new { success = true, html = html, typeVal = typeVal, nameType, propList });
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost] // Them quantity, size, color cua san pham hien thi chi tiet cho nguoi dung
        public async Task<IActionResult> AddTempProductQty(int? id, int colorVal, int sizeVal, int qtyVal)
        {
            try
            {
                var productQtyList = await _dbContext.ProductQty.Where(p => p.ProductID == id).ToListAsync();
                var ProductQty = HttpContext.Session.GetString("TempProductQty");
                var tempDataList = string.IsNullOrEmpty(ProductQty) ? new List<TempProductQty>() : JsonSerializer.Deserialize<List<TempProductQty>>(ProductQty) ?? [];

                var dataExistList = id.HasValue ? productQtyList.Cast<IProductQty>().ToList() : tempDataList.Cast<IProductQty>().ToList();

                var dataExist = dataExistList.FirstOrDefault(p => p.ColorID == colorVal && p.SizeID == sizeVal);
                if (dataExist != null)
                {
                    dataExist.Quantity += qtyVal;

                    if (id.HasValue)
                    {
                        if (dataExist is ProductQuantity)
                        {
                            _dbContext.ProductQty.Update((ProductQuantity)dataExist);
                        }
                    }
                }
                else
                {
                    if (id.HasValue)
                    {
                        dataExist = new ProductQuantity
                        {
                            ProductID = Convert.ToInt32(id.ToString()),
                            ColorID = colorVal,
                            SizeID = sizeVal,
                            Quantity = qtyVal
                        };

                        if (dataExist is ProductQuantity)
                        {
                            await _dbContext.ProductQty.AddAsync((ProductQuantity)dataExist);
                        }
                    }
                    else
                    {
                        dataExistList.Add(new TempProductQty
                        {
                            ColorID = colorVal,
                            SizeID = sizeVal,
                            Quantity = qtyVal
                        });

                        HttpContext.Session.SetString("TempProductQty", JsonSerializer.Serialize(dataExistList));
                    }
                }

                await _dbContext.SaveChangesAsync();

                var partial = await LoadProductQty(null, null, null);
                partial.ValueType = "Update";

                if (!id.HasValue)
                {
                    partial.Qty = partial.TempProductQty.Sum(p => p.Quantity);
                    partial.TempProductQty = dataExistList.OfType<TempProductQty>().ToList();
                }
                else
                {
                    partial.Qty = _dbContext.ProductQty.Where(p => p.ProductID == id).Sum(p => p.Quantity);
                    partial.ProductQty = await _dbContext.ProductQty.Where(p => p.ProductID == id).ToListAsync();
                }

                string html = await this.RenderViewAsync("_ProductItemPartial", partial, true);

                return Json(new { success = true, html = html, qty = partial.Qty });
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost] // Cap nhat quantity, size, color cua san pham hien thi chi tiet cho nguoi dung
        public async Task<IActionResult> UpdateTempProductQty(int? id, int newColorVal, int newSizeVal, int newQtyVal, int oldColorVal, int oldSizeVal)
        {
            try
            {
                var productQtyList = await _dbContext.ProductQty.Where(p => p.ProductID == id).ToListAsync();

                var ProductQty = HttpContext.Session.GetString("TempProductQty");
                var tempDataList = string.IsNullOrEmpty(ProductQty) ? new List<TempProductQty>() : JsonSerializer.Deserialize<List<TempProductQty>>(ProductQty);

                var dataExistList = id.HasValue ? productQtyList.Cast<IProductQty>().ToList() : tempDataList.Cast<IProductQty>().ToList();

                var dataExist = dataExistList.FirstOrDefault(p => p.ColorID == newColorVal && p.SizeID == newSizeVal
                                                            && !(p.ColorID == oldColorVal && p.SizeID == oldSizeVal));

                var dataSelected = dataExistList.FirstOrDefault(p => p.ColorID == oldColorVal && p.SizeID == oldSizeVal);


                if (dataExist != null)
                {
                    dataExist.Quantity += newQtyVal;

                    if (dataSelected != null)
                    {
                        if (dataSelected is TempProductQty)
                        {
                            dataExistList.Remove((TempProductQty)dataSelected);
                        }
                        else _dbContext.ProductQty.Remove((ProductQuantity)dataSelected);
                    }
                }
                else
                {
                    dataSelected.ColorID = newColorVal;
                    dataSelected.SizeID = newSizeVal;
                    dataSelected.Quantity = newQtyVal;
                }

                if (id.HasValue)
                {
                    _dbContext.ProductQty.Update((ProductQuantity)dataSelected);
                    await _dbContext.SaveChangesAsync();
                }
                else HttpContext.Session.SetString("TempProductQty", JsonSerializer.Serialize(tempDataList));

                var partial = await LoadProductQty(id, null, null);
                partial.ValueType = "Update";

                if (!id.HasValue)
                {
                    partial.Qty = partial.TempProductQty.Sum(p => p.Quantity);
                    partial.TempProductQty = dataExistList.OfType<TempProductQty>().ToList();
                }
                else
                {
                    partial.Qty = _dbContext.ProductQty.Where(p => p.ProductID == id).Sum(p => p.Quantity);
                    partial.ProductQty = await _dbContext.ProductQty.Where(p => p.ProductID == id).ToListAsync();
                }

                string html = await this.RenderViewAsync("_ProductItemPartial", partial, true);

                return Json(new { success = true, html = html, qty = partial.Qty });
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost] // Xoa quantity, size, color cua san pham chi tiet cho nguoi dung
        public async Task<IActionResult> DeleteTempProductQty(int? id, int colorVal, int sizeVal, int qtyVal)
        {
            try
            {
                var productQtyList = await _dbContext.ProductQty.Where(p => p.ProductID == id).ToListAsync();
                var partial = await LoadProductQty(null, null, null);
                partial.ValueType = "Update";

                var ProductQty = HttpContext.Session.GetString("TempProductQty");
                var tempDataList = string.IsNullOrEmpty(ProductQty) ? new List<TempProductQty>() : JsonSerializer.Deserialize<List<TempProductQty>>(ProductQty);

                if (id.HasValue)
                {
                    var productQty = await _dbContext.ProductQty.FirstOrDefaultAsync(p => p.ProductID == id && p.SizeID == sizeVal && p.ColorID == colorVal && p.Quantity == qtyVal);
                    _dbContext.ProductQty.Remove(productQty);
                    await _dbContext.SaveChangesAsync();

                    partial.Qty = _dbContext.ProductQty.Where(p => p.ProductID == id).Sum(p => p.Quantity);
                    partial.ProductQty = await _dbContext.ProductQty.Where(p => p.ProductID == id).ToListAsync();
                }
                else
                {
                    tempDataList.RemoveAll(p => p.SizeID == sizeVal && p.ColorID == colorVal && p.Quantity == qtyVal);
                    HttpContext.Session.SetString("TempProductQty", JsonSerializer.Serialize(tempDataList));
                    partial.Qty = partial.TempProductQty.Sum(p => p.Quantity);
                    partial.TempProductQty = tempDataList;
                }

                string html = await this.RenderViewAsync("_ProductItemPartial", partial, true);

                return Json(new { success = true, html = html, qty = partial.Qty });
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet] // Load bang so luong san pham
        public async Task<IActionResult> DisplayProductQty(int? idExist, string[] arrColor, string[] arrSize)
        {
            try
            {
                var productQtyList = _dbContext.ProductQty.ToList();
                if (idExist.HasValue)
                {
                    arrColor = productQtyList.Where(p => p.ProductID == idExist).Select(p => p.ColorID.ToString()).ToArray();
                    arrSize = productQtyList.Where(p => p.ProductID == idExist).Select(p => p.SizeID.ToString()).ToArray();
                }
                else
                {
                    if (arrColor.Length == 0) arrColor = [];
                    if (arrSize.Length == 0) arrSize = [];
                }

                var partial = await LoadProductQty(idExist, arrColor, arrSize);
                partial.ValueType = "Update";

                partial.Qty = idExist.HasValue ? partial.ProductQty.Sum(p => p.Quantity) : partial.TempProductQty.Sum(p => p.Quantity);

                string html = await this.RenderViewAsync("_ProductItemPartial", partial, true);

                return Json(new { success = true, html = html, qty = partial.Qty });
            }
            catch
            {
                throw new Exception();
            }
        }

        // Ham load san pham chung
        public async Task<ProductItemGeneral> LoadProductQty(int? id, string[]? arrColor, string[]? arrSize)
        {
            var ProductQty = HttpContext.Session.GetString("TempProductQty");
            var tempDataList = string.IsNullOrEmpty(ProductQty) ? new List<TempProductQty>() : JsonSerializer.Deserialize<List<TempProductQty>>(ProductQty);

            ProductItemGeneral partial = new ProductItemGeneral()
            {
                ProductQty = new List<ProductQuantity>(),
                TempProductQty = tempDataList ?? new List<TempProductQty>()
            };

            var colorBySelectList = new List<Color>();
            var sizeBySelectList = new List<Size>();

            sizeBySelectList = await _dbContext.Size.ToListAsync();
            colorBySelectList = await _dbContext.Color.ToListAsync();

            partial.ProductQty = await _dbContext.ProductQty.Where(p => p.ProductID == id && arrColor.Contains(p.ColorID.ToString()) && arrSize.Contains(p.SizeID.ToString()))
                                                            .Include(p => p.Color)
                                                            .Include(p => p.Size)
                                                            .ToListAsync() ?? new List<ProductQuantity>();

            partial.SelectListSize = new SelectList(sizeBySelectList, "ID", "Name");
            partial.SelectListColor = new SelectList(colorBySelectList, "ID", "Name");

            var tasks = (
                tag: _dbFactory.CreateDbContext().Tag.ToListAsync(),
                style: _dbFactory.CreateDbContext().Style.ToListAsync(),
                season: _dbFactory.CreateDbContext().Season.ToListAsync(),
                material: _dbFactory.CreateDbContext().Material.ToListAsync()
            );

            await Task.WhenAll(tasks.tag, tasks.style, tasks.season, tasks.material);

            //partial.Tag = tasks.tag.Result;
            //partial.Style = tasks.style.Result;
            //partial.Season = tasks.season.Result;
            //partial.Material = tasks.material.Result;

            return partial;
        }

        [HttpGet] // lay thuoc tinh hien thi danh sach chi tiet san pham (admin)
        public async Task<IActionResult> ProductDetailListItem(string[]? opsValue, string nameValue)
        {
            try
            {
                ProductItemGeneral partial = new ProductItemGeneral()
                {
                    ValueType = nameValue,
                    Items = new List<IProductItemGeneral>()
                };

                if (opsValue == null)
                {
                    opsValue = Array.Empty<string>();
                }

                switch (nameValue)
                {
                    case "Material":
                        var materialList = await _dbContext.Material.Where(p => opsValue.Contains(p.ID.ToString())).ToListAsync();
                        partial.Items = materialList.Cast<IProductItemGeneral>().ToList();
                        partial.Material = materialList;
                        break;

                    case "Color":
                        var colorList = await _dbContext.Color.Where(p => opsValue.Contains(p.ID.ToString())).ToListAsync();
                        partial.Items = colorList.Cast<IProductItemGeneral>().ToList();
                        partial.Color = colorList;
                        break;

                    case "Size":
                        var sizeList = await _dbContext.Size.Where(p => opsValue.Contains(p.ID.ToString())).ToListAsync();
                        partial.Items = sizeList.Cast<IProductItemGeneral>().ToList();
                        partial.Size = sizeList;
                        break;

                    case "Season":
                        var seasonList = await _dbContext.Season.Where(p => opsValue.Contains(p.ID.ToString())).ToListAsync();
                        partial.Items = seasonList.Cast<IProductItemGeneral>().ToList();
                        partial.Season = seasonList;
                        break;

                    case "Style":
                        var styleList = await _dbContext.Style.Where(p => opsValue.Contains(p.ID.ToString())).ToListAsync();
                        partial.Items = styleList.Cast<IProductItemGeneral>().ToList();
                        partial.Style = styleList;
                        break;

                    case "Tag":
                        var tagList = await _dbContext.Tag.Where(p => opsValue.Contains(p.ID.ToString())).ToListAsync();
                        partial.Items = tagList.Cast<IProductItemGeneral>().ToList();
                        partial.Tag = tagList;
                        break;

                    default: return await ReloadViewModel(new ProductGeneralViewModel());
                }

                return PartialView("_ProductItemPartial", partial);
            }
            catch
            {
                throw new Exception();
            }
        }
    }
}
