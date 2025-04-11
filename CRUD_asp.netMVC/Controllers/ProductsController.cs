using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CRUD_asp.netMVC.Data;
using System.Configuration;
using AspNetCoreGeneratedDocument;
using NuGet.Versioning;
using System.Numerics;
using CRUD_asp.netMVC.Models.Product;
using CRUD_asp.netMVC.Models.Pagination;

namespace CRUD_asp.netMVC.Controllers
{
    public class ProductsController : Controller
    {
        public readonly AppDBContext _context;
        private readonly IWebHostEnvironment environment;

        public ProductsController(AppDBContext context, IWebHostEnvironment _environment)
        {
            _context = context;
            environment = _environment;
        }

        // GET: Products
        public async Task<IActionResult> Index(int page = 1)
        {
            var product = _context.Products
                       .Include(p => p.Manufactures)
                       .Include(p => p.Types).OrderByDescending(p=> p.ID);

            var paginationProduct = await PaginatedList<Products>.CreatePag(product, page, 5);
            return View(paginationProduct);
        }

        // Find Porduct by keyword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(int page = 1, string keyword = "")
        {
            var product = _context.Products
                       .Include(p => p.Manufactures)
                       .Include(p => p.Types).Where(p => p.Description.Contains(keyword) || p.Name.Contains(keyword));

            var paginationProduct = await PaginatedList<Products>.CreatePag(product, page, 5);
            return View(paginationProduct);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _context.Products
                        .Include(p => p.Manufactures)
                        .Include(p => p.Types)
                        .FirstOrDefault(p => p.ID == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            var products = _context.Products.Include(m => m.Manufactures).Include(t => t.Types).FirstOrDefault();

            ViewBag.Manufactures = new SelectList(_context.Manufactures, "ID", "Name", products.manuID);
            ViewBag.Prototypes = new SelectList(_context.Prototypes, "ID", "Name", products.typeID);

            return View(products);
        }

        // POST: Products/Create 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,picture,Description,Price,manuID,typeID")] Products products)
        {
            if (ModelState.IsValid)
            {
                if (products.picture != null && products.picture.Length > 0)
                {
                    var pathName = Path.GetFileName(products.picture.FileName);
                    if (pathName != null)
                    {
                        var fileUpLoad = Path.Combine(environment.WebRootPath, "image", pathName);
                        using (var fileStream = new FileStream(fileUpLoad, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            await products.picture.CopyToAsync(fileStream);
                        }
                    }

                    products.PicturePath = "/image/" + pathName;
                }

                _context.Add(products);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Manufactures = new SelectList(_context.Manufactures, "ID", "Name", products.manuID);
            ViewBag.Prototypes = new SelectList(_context.Prototypes, "ID", "Name", products.typeID);

            return View(products);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (_context.Products != null)
            {
                var product = _context.Products
                    .Include(p => p.Manufactures)
                    .Include(p => p.Types)
                    .FirstOrDefault(p => p.ID == id);

                if (product != null)
                {
                    ViewBag.Manufactures = new SelectList(_context.Manufactures, "ID", "Name", product.manuID);
                    ViewBag.ProtoType = new SelectList(_context.Prototypes, "ID", "Name", product.typeID);
                    return View(product);
                }
                else return NotFound();

            }
            else return NotFound();

        }

        // Tuan note: get all properties Products class
        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,picture,PicturePath,Description,Created,Price,manuID,typeID")] Products products)
        {
            if (id != products.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var file = products.picture;
                    if (file != null && !string.IsNullOrWhiteSpace(file.FileName))
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var fileUpLoad = Path.Combine(environment.WebRootPath, "image", fileName);
                        using (var FileStream = new FileStream(fileUpLoad, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            await file.CopyToAsync(FileStream);
                        }

                        products.PicturePath = "/image/" + fileName;
                    }
                    else
                    {
                        var oldPicture = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.ID == id);
                        if (oldPicture != null)
                        {
                            products.PicturePath = oldPicture.PicturePath;
                        }
                    }

                    _context.Update(products);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsExists(products.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

            }
            return View(products);


            //return 
        }


        // GET: Products/Delete/5
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

        // POST: Products/Delete/5
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
