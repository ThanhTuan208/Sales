using CRUD_asp.netMVC.DTO.Auth;
using CRUD_asp.netMVC.Models.Auth;
using CRUD_asp.netMVC.Models.Cart;
using CRUD_asp.netMVC.Models.Product;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Text;

namespace CRUD_asp.netMVC.Data.Seed
{
    public static class ModelBuilderCreateFuncGetSeed
    {
        public static void RelationshipEntitys(this ModelBuilder modelBuilder)
        {
            // Mockup du lieu mqh 1 - n
            modelBuilder.Entity<Products>().HasOne(m => m.Brands).WithMany(p => p.products).HasForeignKey(mi => mi.BrandID).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Products>().HasOne(t => t.Cate).WithMany(p => p.products).HasForeignKey(ti => ti.CateID).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Products>().HasOne(p => p.Gender).WithMany(p => p.Products).HasForeignKey(p => p.GenderID).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Products>().HasOne(p => p.Featured).WithMany(p => p.products).HasForeignKey(p => p.FeaturedID).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AddToCart>().HasOne(p => p.Product).WithMany(c => c.Carts).HasForeignKey(pi => pi.ProductID).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<AddToCart>().HasOne(u => u.Users).WithMany(c => c.Carts).HasForeignKey(ui => ui.UserID).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderDetail>().HasOne(o => o.Orders).WithMany(od => od.OrderDetail).HasForeignKey(oi => oi.OrderID).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<OrderDetail>().HasOne(p => p.Product).WithMany(od => od.OrderDetails).HasForeignKey(pi => pi.ProductID).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Orders>().HasOne(u => u.Users).WithMany(o => o.Orders).HasForeignKey(pi => pi.UserID).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reviews>().HasOne(r => r.Users).WithMany(r => r.Reviews).HasForeignKey(u => u.UserID).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Reviews>().HasOne(r => r.Product).WithMany(r => r.Reviews).HasForeignKey(u => u.ProductID).OnDelete(DeleteBehavior.Cascade);

            // Mockup du lieu mqh 1 - 1
            modelBuilder.Entity<Payment>().HasOne(o => o.Order).WithOne(p => p.Payment).HasForeignKey<Payment>(o => o.OrderID).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Users>().HasOne(r => r.Manager).WithOne(u => u.Users).HasForeignKey<Manager>(mi => mi.UserID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Users>().HasOne(r => r.Staff).WithOne(u => u.Users).HasForeignKey<Staff>(mi => mi.UserID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Users>().HasOne(r => r.Customer).WithOne(u => u.Users).HasForeignKey<Customer>(mi => mi.UserID).OnDelete(DeleteBehavior.Restrict);

            // Mockup du lieu mqh n - n
            modelBuilder.Entity<ProductSeason>().HasKey(p => new { p.SeasonID, p.ProductID });
            modelBuilder.Entity<ProductSeason>().HasOne(p => p.Product).WithMany(p => p.ProductSeasons).HasForeignKey(p => p.ProductID);
            modelBuilder.Entity<ProductSeason>().HasOne(p => p.Season).WithMany(p => p.ProductSeason).HasForeignKey(p => p.SeasonID);

            modelBuilder.Entity<ProductTag>().HasKey(p => new { p.TagID, p.ProductID });
            modelBuilder.Entity<ProductTag>().HasOne(p => p.Product).WithMany(p => p.ProductTags).HasForeignKey(p => p.ProductID);
            modelBuilder.Entity<ProductTag>().HasOne(p => p.Tag).WithMany(p => p.ProductTag).HasForeignKey(p => p.TagID);

            modelBuilder.Entity<ProductStyle>().HasKey(p => new { p.StyleID, p.ProductID });
            modelBuilder.Entity<ProductStyle>().HasOne(p => p.Product).WithMany(p => p.ProductStyles).HasForeignKey(p => p.ProductID);
            modelBuilder.Entity<ProductStyle>().HasOne(p => p.Style).WithMany(p => p.ProductStyles).HasForeignKey(p => p.StyleID);

            modelBuilder.Entity<ProductSize>().HasKey(p => new { p.ProductID, p.SizeID });
            modelBuilder.Entity<ProductSize>().HasOne(p => p.products).WithMany(p => p.ProductSize).HasForeignKey(p => p.ProductID);
            modelBuilder.Entity<ProductSize>().HasOne(p => p.size).WithMany(p => p.ProductSize).HasForeignKey(p => p.SizeID);

            modelBuilder.Entity<ProductColors>().HasKey(p => new { p.ProductID, p.ColorID });
            modelBuilder.Entity<ProductColors>().HasOne(p => p.Product).WithMany(p => p.ProductColor).HasForeignKey(p => p.ProductID);
            modelBuilder.Entity<ProductColors>().HasOne(p => p.Color).WithMany(p => p.ProductColor).HasForeignKey(p => p.ColorID);

            modelBuilder.Entity<ProductMaterial>().HasKey(p => new { p.ProductID, p.MaterialID });
            modelBuilder.Entity<ProductMaterial>().HasOne(p => p.Product).WithMany(p => p.ProductMaterial).HasForeignKey(p => p.ProductID);
            modelBuilder.Entity<ProductMaterial>().HasOne(p => p.Material).WithMany(p => p.ProductMaterial).HasForeignKey(p => p.MaterialID);

            modelBuilder.Entity<ProductColors>().HasKey(p => new { p.ProductID, p.ColorID });
            modelBuilder.Entity<ProductColors>().HasOne(p => p.Product).WithMany(p => p.ProductColor).HasForeignKey(p => p.ProductID);
            modelBuilder.Entity<ProductColors>().HasOne(p => p.Color).WithMany(p => p.ProductColor).HasForeignKey(p => p.ColorID);

            modelBuilder.Entity<ProductImages>().HasOne(p => p.Products).WithMany(p => p.ProductImages).HasForeignKey(p => p.ProductID);
            //-------------------------------------------------------------------------
            modelBuilder.Entity<Login>().HasNoKey();
            modelBuilder.Entity<Register>().HasNoKey();

            var hashPass1 = new PasswordHasher<Users>().HashPassword(null, "admin123");
            var hashPass2 = new PasswordHasher<Users>().HashPassword(null, "123456");
        }


        public static void SeedFeatured(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Featured>().HasData(

                 new Featured { ID = 1, Name = "Nổi bật" },
                 new Featured { ID = 2, Name = "Không nổi bật" }
                 );

        }

        public static void SeedRoels(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Roles>().HasData(

                 new Roles { Id = 1, Name = "Admin", NormalizedName = "ADMIN" },
                 new Roles { Id = 2, Name = "Customer", NormalizedName = "CUSTOMER" }
                 );
        }

        public static void SeedGenders(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Gender>().HasData(
                new Gender { ID = 1, Name = "Nam" },
                new Gender { ID = 2, Name = "Nữ" },
                new Gender { ID = 3, Name = "Unisex" }
            );
        }

        public static void SeedMaterial(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Material>().HasData(
                new Material { ID = 1, Name = "Cotton" },
                new Material { ID = 2, Name = "Polyester" },
                new Material { ID = 3, Name = "Silk" },
                new Material { ID = 4, Name = "Leather" },
                new Material { ID = 5, Name = "Wool" }
            );
        }

        public static void SeedStyle(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Style>().HasData(
              new Style { ID = 1, Name = "Basic" },
              new Style { ID = 2, Name = "Sport" },
              new Style { ID = 3, Name = "Formal" },
              new Style { ID = 4, Name = "Streetwear" },
              new Style { ID = 5, Name = "Vintage" },
              new Style { ID = 6, Name = "Casual" }
          );

        }

        public static void SeedSeason(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Season>().HasData(
                new Season { ID = 1, Name = "Mùa xuân" },
                new Season { ID = 2, Name = "Mùa hạ" },
                new Season { ID = 3, Name = "Mùa thu" },
                new Season { ID = 4, Name = "Mùa đông" }
            );
        }

        public static void SeedColor(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Color>().HasData(
                new Color { ID = 1, Name = "Đen" },
                new Color { ID = 2, Name = "Xám" },
                new Color { ID = 3, Name = "Trắng" },
                new Color { ID = 4, Name = "Hồng" },
                new Color { ID = 5, Name = "Xanh navy" },
                new Color { ID = 6, Name = "Trắng/Xanh" },
                new Color { ID = 7, Name = "Hồng pastel" },
                new Color { ID = 8, Name = "Đỏ đậm" }
            );
        }

        public static void SeedSize(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Size>().HasData(
                new Size { ID = 1, Name = "S" },
                new Size { ID = 2, Name = "M" },
                new Size { ID = 3, Name = "L" },
                new Size { ID = 4, Name = "XL" },
                new Size { ID = 5, Name = "36" },
                new Size { ID = 6, Name = "37" },
                new Size { ID = 7, Name = "38" },
                new Size { ID = 8, Name = "39" },
                new Size { ID = 9, Name = "40" },
                new Size { ID = 10, Name = "41" },
                new Size { ID = 11, Name = "42" },
                new Size { ID = 12, Name = "43" },
                new Size { ID = 13, Name = "44" },
                new Size { ID = 14, Name = "Free Size" }
            );
        }

        public static void SeedBrand(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Brand>().HasData(
             new Brand { ID = 1, Name = "Nike", PicturePath = "images/logo/logo_nike.gif", Description = "Thương hiệu giày thể thao và trang phục thể thao nổi tiếng" },
             new Brand { ID = 2, Name = "Adidas", PicturePath = "images/logo/logo_adidas.png", Description = "Chuyên cung cấp giày dép và trang phục thể thao" },
             new Brand { ID = 3, Name = "Zara", PicturePath = "images/logo/logo_zara.png", Description = "Thương hiệu thời trang cao cấp với thiết kế hiện đại" },
             new Brand { ID = 4, Name = "H&M", PicturePath = "images/logo/logo_HM.webp", Description = "H&M cung cấp thời trang giá cả phải chăng và phong cách hiện đại" },
              new Brand { ID = 5, Name = "Uniqlo", PicturePath = "images/logo/logo_uniqlo.png", Description = "Thương hiệu Nhật Bản nổi tiếng với phong cách tối giản và chất liệu tốt" },
              new Brand { ID = 6, Name = "Gucci", PicturePath = "images/logo/logo_gucci.jpg", Description = "Thương hiệu cao cấp đến từ Ý với các sản phẩm thời trang xa xỉ" },
              new Brand { ID = 7, Name = "Levi's", PicturePath = "images/logo/logo_levis.png", Description = "Hãng thời trang nổi tiếng với quần jeans và phong cách cổ điển Mỹ" },
              new Brand { ID = 8, Name = "Lacoste", PicturePath = "images/logo/logo_lacoste.png", Description = "Phong cách thể thao thanh lịch đến từ Pháp" },
              new Products { ID = 9, Name = "Puma", PicturePath = "images/logo/logo_puma.jpg", Description = "Cung cấp thời trang thể thao, giày dép và phụ kiện chất lượng" },
              new Brand { ID = 10, Name = "Chanel", PicturePath = "images/logo/logo_chanel.jpg", Description = "Biểu tượng thời trang xa xỉ với những thiết kế đẳng cấp, cổ điển" }
         );
        }

        //public static void SeedCategory(this ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Category>().HasData(
        //        new Category { ID = 1, Name = "Áo khoác" },
        //        new Category { ID = 2, Name = "Quần" },
        //        new Category { ID = 3, Name = "Giày" },
        //        new Category { ID = 4, Name = "Váy" },
        //        new Category { ID = 5, Name = "Áo thun" },
        //        new Category { ID = 6, Name = "Đồng hồ" }
        //    );
        //}

        public static void SeedCategory(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { ID = 1, Name = "Áo khoác", PicturePath = "images/Category/jacket1.png" },
                new Category { ID = 2, Name = "Quần", PicturePath = "images/Category/trousers1.png" },
                new Category { ID = 3, Name = "Giày", PicturePath = "images/Category/shoes1.png" },
                new Category { ID = 4, Name = "Váy", PicturePath = "images/Category/skirt1.png" },
                new Category { ID = 5, Name = "Áo thun", PicturePath = "images/Category/tshirt1.png" },
                new Category { ID = 6, Name = "Đồng hồ", PicturePath = "images/Category/wristwatch1.png" }
            );
        }

        public static void SeedTag(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tag>().HasData(
               new Tag { ID = 1, Name = "Thời trang", Description = "Các sản phẩm liên quan đến xu hướng thời trang" },
               new Tag { ID = 2, Name = "Thể thao", Description = "Trang phục và phụ kiện dùng trong thể thao" },
               new Tag { ID = 3, Name = "Giày thể thao", Description = "Các loại giày dành cho vận động, thể thao" },
               new Tag { ID = 4, Name = "Nữ", Description = "Sản phẩm dành riêng cho nữ giới" },
               new Tag { ID = 5, Name = "Nam", Description = "Sản phẩm dành riêng cho nam giới" },
               new Tag { ID = 6, Name = "Công sở", Description = "Thời trang cho môi trường làm việc, công sở" },
               new Tag { ID = 7, Name = "Dạo phố", Description = "Thời trang năng động, thoải mái khi đi chơi" },
               new Tag { ID = 8, Name = "Unisex", Description = "Phù hợp cho cả nam và nữ" }
           );
        }

        public static void SeedPoductTag(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductTag>().HasData(
                 new ProductTag { ProductID = 1, TagID = 2 },
                 new ProductTag { ProductID = 1, TagID = 5 },
                 new ProductTag { ProductID = 2, TagID = 2 },
                 new ProductTag { ProductID = 2, TagID = 5 },
                 new ProductTag { ProductID = 3, TagID = 3 },
                 new ProductTag { ProductID = 3, TagID = 5 },
                 new ProductTag { ProductID = 4, TagID = 2 },
                 new ProductTag { ProductID = 4, TagID = 4 },
                 new ProductTag { ProductID = 5, TagID = 5 },
                 new ProductTag { ProductID = 6, TagID = 2 },
                 new ProductTag { ProductID = 6, TagID = 8 },
                 new ProductTag { ProductID = 7, TagID = 2 },
                 new ProductTag { ProductID = 7, TagID = 5 },
                 new ProductTag { ProductID = 8, TagID = 2 },
                 new ProductTag { ProductID = 9, TagID = 3 },
                 new ProductTag { ProductID = 9, TagID = 8 },
                 new ProductTag { ProductID = 10, TagID = 4 },
                 new ProductTag { ProductID = 10, TagID = 2 },
                 new ProductTag { ProductID = 11, TagID = 5 },
                 new ProductTag { ProductID = 12, TagID = 2 },
                 new ProductTag { ProductID = 12, TagID = 8 },
                 new ProductTag { ProductID = 13, TagID = 2 },
                 new ProductTag { ProductID = 13, TagID = 3 },
                 new ProductTag { ProductID = 14, TagID = 1 },
                 new ProductTag { ProductID = 15, TagID = 2 },
                 new ProductTag { ProductID = 15, TagID = 5 },
                 new ProductTag { ProductID = 16, TagID = 4 },
                 new ProductTag { ProductID = 16, TagID = 6 },
                 new ProductTag { ProductID = 17, TagID = 5 },
                 new ProductTag { ProductID = 18, TagID = 7 },
                 new ProductTag { ProductID = 19, TagID = 2 },
                 new ProductTag { ProductID = 20, TagID = 4 },
                 new ProductTag { ProductID = 21, TagID = 3 },
                 new ProductTag { ProductID = 22, TagID = 5 },
                 new ProductTag { ProductID = 23, TagID = 1 },
                 new ProductTag { ProductID = 24, TagID = 4 },
                 new ProductTag { ProductID = 25, TagID = 5 },
                 new ProductTag { ProductID = 26, TagID = 6 },
                 new ProductTag { ProductID = 27, TagID = 2 },
                 new ProductTag { ProductID = 28, TagID = 5 },
                 new ProductTag { ProductID = 29, TagID = 3 },
                 new ProductTag { ProductID = 30, TagID = 4 },
                 new ProductTag { ProductID = 31, TagID = 2 },
                 new ProductTag { ProductID = 32, TagID = 5 },
                 new ProductTag { ProductID = 33, TagID = 6 },
                 new ProductTag { ProductID = 34, TagID = 4 },
                 new ProductTag { ProductID = 35, TagID = 3 },
                 new ProductTag { ProductID = 36, TagID = 5 },
                 new ProductTag { ProductID = 37, TagID = 2 },
                 new ProductTag { ProductID = 38, TagID = 8 },
                 new ProductTag { ProductID = 39, TagID = 1 },
                 new ProductTag { ProductID = 40, TagID = 6 },
                 new ProductTag { ProductID = 41, TagID = 2 },
                 new ProductTag { ProductID = 42, TagID = 5 },
                 new ProductTag { ProductID = 43, TagID = 3 },
                 new ProductTag { ProductID = 44, TagID = 7 },
                 new ProductTag { ProductID = 45, TagID = 2 },
                 new ProductTag { ProductID = 46, TagID = 5 },
                 new ProductTag { ProductID = 47, TagID = 1 },
                 new ProductTag { ProductID = 48, TagID = 3 },
                 new ProductTag { ProductID = 49, TagID = 2 },
                 new ProductTag { ProductID = 50, TagID = 4 },
                 new ProductTag { ProductID = 51, TagID = 5 },
                 new ProductTag { ProductID = 52, TagID = 2 },
                 new ProductTag { ProductID = 53, TagID = 6 },
                 new ProductTag { ProductID = 54, TagID = 3 },
                 new ProductTag { ProductID = 55, TagID = 2 },
                 new ProductTag { ProductID = 56, TagID = 8 },
                 new ProductTag { ProductID = 57, TagID = 1 },
                 new ProductTag { ProductID = 58, TagID = 5 },
                 new ProductTag { ProductID = 59, TagID = 4 },
                 new ProductTag { ProductID = 60, TagID = 2 }
             );
        }

        public static void SeedProductSize(this ModelBuilder modelBuilder)
        {
            var ListProductSize = new List<ProductSize>();
            for (int productID = 1; productID <= 60; productID++)
            {
                ListProductSize.AddRange(new List<ProductSize>()
                {
                    new ProductSize { ProductID = productID, SizeID = 1 },
                    new ProductSize { ProductID = productID, SizeID = 2 },
                    new ProductSize { ProductID = productID, SizeID = 3 },
                    new ProductSize { ProductID = productID, SizeID = 4 },
                });
            }

            modelBuilder.Entity<ProductSize>().HasData(ListProductSize);
        }

        public static void SeedProductColor(this ModelBuilder modelBuilder)
        {
            var ListProductColor = new List<ProductColors>();
            for (int productID = 1; productID <= 60; productID++)
            {
                ListProductColor.AddRange(new List<ProductColors>()
                {
                    new ProductColors { ProductID = productID, ColorID = 1 },
                    new ProductColors { ProductID = productID, ColorID = 2 },
                    new ProductColors { ProductID = productID, ColorID = 3 },
                    new ProductColors { ProductID = productID, ColorID = 5 },
                });
            }

            modelBuilder.Entity<ProductColors>().HasData(ListProductColor);
        }

        public static void SeedProductMaterial(this ModelBuilder modelBuilder)
        {
            // Tái sử dụng dữ liệu GetProducts để tạo Dictionary
            var products = GetProducts();

            // Tạo Dictionary ánh xạ ProductID sang CateID, BrandID
            var productMap = products.ToDictionary(
                p => p.ID,
                p => new { p.CateID, p.BrandID }
            );

            // Tạo danh sách ProductMaterials
            var productMaterials = new List<ProductMaterial>();

            for (int productId = 1; productId <= 60; productId++)
            {
                int? cateId = productMap[productId].CateID;
                int? brandId = productMap[productId].BrandID;

                switch (cateId)
                {
                    case 1: // Áo khoác
                        productMaterials.Add(new ProductMaterial { ProductID = productId, MaterialID = 2 }); // Polyester
                        if (brandId == 6 || brandId == 10) // Gucci, Chanel
                        {
                            productMaterials.Add(new ProductMaterial { ProductID = productId, MaterialID = 3 }); // Silk
                            productMaterials.Add(new ProductMaterial { ProductID = productId, MaterialID = 5 }); // Wool
                        }
                        else if (brandId == 7) // Levi's
                        {
                            productMaterials.Add(new ProductMaterial { ProductID = productId, MaterialID = 1 }); // Cotton
                        }
                        else
                        {
                            productMaterials.Add(new ProductMaterial { ProductID = productId, MaterialID = 5 }); // Wool
                        }
                        break;

                    case 2: // Quần
                        if (brandId == 7) // Levi's
                        {
                            productMaterials.Add(new ProductMaterial { ProductID = productId, MaterialID = 1 }); // Cotton
                        }
                        else if (brandId == 6 || brandId == 10) // Gucci, Chanel
                        {
                            productMaterials.Add(new ProductMaterial { ProductID = productId, MaterialID = 3 }); // Silk
                            productMaterials.Add(new ProductMaterial { ProductID = productId, MaterialID = 5 }); // Wool
                        }
                        else
                        {
                            productMaterials.Add(new ProductMaterial { ProductID = productId, MaterialID = 2 }); // Polyester
                            productMaterials.Add(new ProductMaterial { ProductID = productId, MaterialID = 1 }); // Cotton
                        }
                        break;

                    case 3: // Giày
                        productMaterials.Add(new ProductMaterial { ProductID = productId, MaterialID = 4 }); // Leather
                        if (brandId == 1 || brandId == 2 || brandId == 9) // Nike, Adidas, Puma
                        {
                            productMaterials.Add(new ProductMaterial { ProductID = productId, MaterialID = 2 }); // Polyester
                        }
                        break;

                    case 4: // Váy
                        if (brandId == 6 || brandId == 10) // Gucci, Chanel
                        {
                            productMaterials.Add(new ProductMaterial { ProductID = productId, MaterialID = 3 }); // Silk
                        }
                        else if (brandId == 7) // Levi's
                        {
                            productMaterials.Add(new ProductMaterial { ProductID = productId, MaterialID = 1 }); // Cotton
                        }
                        else
                        {
                            productMaterials.Add(new ProductMaterial { ProductID = productId, MaterialID = 2 }); // Polyester
                            productMaterials.Add(new ProductMaterial { ProductID = productId, MaterialID = 1 }); // Cotton
                        }
                        break;

                    case 5: // Áo thun
                        productMaterials.Add(new ProductMaterial { ProductID = productId, MaterialID = 1 }); // Cotton
                        if (brandId == 1 || brandId == 2 || brandId == 9) // Nike, Adidas, Puma
                        {
                            productMaterials.Add(new ProductMaterial { ProductID = productId, MaterialID = 2 }); // Polyester
                        }
                        if (brandId == 10) // Chanel
                        {
                            productMaterials.Add(new ProductMaterial { ProductID = productId, MaterialID = 3 }); // Silk
                        }
                        break;

                    case 6: // Đồng hồ
                        if (brandId == 1 || brandId == 4 || brandId == 9) // Nike, H&M, Puma
                        {
                            productMaterials.Add(new ProductMaterial { ProductID = productId, MaterialID = 2 }); // Polyester (dây silicon)
                        }
                        else if (brandId == 6 || brandId == 10) // Gucci, Chanel
                        {
                            productMaterials.Add(new ProductMaterial { ProductID = productId, MaterialID = 3 }); // Silk
                            productMaterials.Add(new ProductMaterial { ProductID = productId, MaterialID = 4 }); // Leather
                        }
                        else
                        {
                            productMaterials.Add(new ProductMaterial { ProductID = productId, MaterialID = 4 }); // Leather
                        }
                        break;
                }
            }

            // Seed ProductMaterials
            modelBuilder.Entity<ProductMaterial>().HasData(productMaterials);
        }

        public static void SeedProducts_ProductSeason_ProductStyle(this ModelBuilder modelBuilder)
        {
            var products = GetProducts();
            #region ProductSeason
            // Tạo Dictionary để ánh xạ ProductID sang CateID
            var productCateMap = products.ToDictionary(p => p.ID, p => p.CateID);

            // Tạo danh sách để seed ProductSeasons
            var productSeasons = new List<ProductSeason>();

            for (int productId = 1; productId <= 60; productId++)
            {
                int? cateId = productCateMap[productId];

                switch (cateId)
                {
                    case 1: // Áo khoác
                        productSeasons.AddRange(new List<ProductSeason>
                        {
                            new ProductSeason { ProductID = productId, SeasonID = 1 }, // Mùa xuân
                            new ProductSeason { ProductID = productId, SeasonID = 2 }, // Mùa hè
                            new ProductSeason { ProductID = productId, SeasonID = 3 }, // Mùa thu
                            new ProductSeason { ProductID = productId, SeasonID = 4 }  // Mùa đông
                        });
                        break;

                    case 2: // Quần
                        if (productId == 44) // Quần short Lacoste
                        {
                            productSeasons.Add(new ProductSeason { ProductID = productId, SeasonID = 2 }); // Mùa hè
                        }
                        else // Các loại quần khác
                        {
                            productSeasons.AddRange(new List<ProductSeason>
                            {
                                new ProductSeason { ProductID = productId, SeasonID = 1 }, // Mùa xuân
                                new ProductSeason { ProductID = productId, SeasonID = 2 }, // Mùa hè
                                new ProductSeason { ProductID = productId, SeasonID = 3 }, // Mùa thu
                                new ProductSeason { ProductID = productId, SeasonID = 4 }  // Mùa đông
                            });
                        }
                        break;

                    case 3: // Giày
                        if (productId == 15 || productId == 57) // Giày cao gót
                        {
                            productSeasons.AddRange(new List<ProductSeason>
                            {
                                new ProductSeason { ProductID = productId, SeasonID = 1 }, // Mùa xuân
                                new ProductSeason { ProductID = productId, SeasonID = 2 }  // Mùa hè
                            });
                        }
                        else // Giày thể thao, sneaker, lười
                        {
                            productSeasons.AddRange(new List<ProductSeason>
                            {
                                new ProductSeason { ProductID = productId, SeasonID = 1 }, // Mùa xuân
                                new ProductSeason { ProductID = productId, SeasonID = 2 }, // Mùa hè
                                new ProductSeason { ProductID = productId, SeasonID = 3 }, // Mùa thu
                                new ProductSeason { ProductID = productId, SeasonID = 4 }  // Mùa đông
                            });
                        }
                        break;

                    case 4: // Váy
                        productSeasons.AddRange(new List<ProductSeason>
                        {
                            new ProductSeason { ProductID = productId, SeasonID = 1 }, // Mùa xuân
                            new ProductSeason { ProductID = productId, SeasonID = 2 }  // Mùa hè
                        });
                        if (productId == 58) // Váy dạ hội Chanel
                        {
                            productSeasons.Add(new ProductSeason { ProductID = productId, SeasonID = 3 }); // Mùa thu
                        }
                        break;

                    case 5: // Áo thun
                        productSeasons.AddRange(new List<ProductSeason>
                        {
                            new ProductSeason { ProductID = productId, SeasonID = 1 }, // Mùa xuân
                            new ProductSeason { ProductID = productId, SeasonID = 2 }, // Mùa hè
                            new ProductSeason { ProductID = productId, SeasonID = 3 }  // Mùa thu
                        });
                        break;

                    case 6: // Đồng hồ
                        productSeasons.AddRange(new List<ProductSeason>
                        {
                            new ProductSeason { ProductID = productId, SeasonID = 1 }, // Mùa xuân
                            new ProductSeason { ProductID = productId, SeasonID = 2 }, // Mùa hè
                            new ProductSeason { ProductID = productId, SeasonID = 3 }, // Mùa thu
                            new ProductSeason { ProductID = productId, SeasonID = 4 }  // Mùa đông
                        });
                        break;
                }
            }
            #endregion
            #region ProductStyle
            // Tạo Dictionary ánh xạ ProductID sang CateID và BrandID
            var productMap = products.ToDictionary(
                    p => p.ID,
                    p => new { p.CateID, p.BrandID }
                );

            // Tạo danh sách ProductStyle
            var productStyles = new List<ProductStyle>();

            for (int productID = 1; productID <= 60; productID++)
            {
                int? cateID = productMap[productID].CateID;
                int? brandId = productMap[productID].BrandID;

                switch (cateID)
                {
                    case 1: // Áo khoác
                        if (brandId == 6 || brandId == 10) // Gucci, Chanel
                        {
                            productStyles.Add(new ProductStyle { ProductID = productID, StyleID = 3 }); // Formal
                        }
                        else if (brandId == 7) // Levi's
                        {
                            productStyles.AddRange(new List<ProductStyle>
                            {
                                new ProductStyle { ProductID = productID, StyleID = 5 }, // Vintage
                                new ProductStyle { ProductID = productID, StyleID = 6 }  // Casual
                            });
                        }
                        else // Nike, Adidas, Zara, H&M, Uniqlo, Lacoste, Puma
                        {
                            productStyles.AddRange(new List<ProductStyle>
                            {
                                new ProductStyle { ProductID = productID, StyleID = 2 }, // Sport
                                new ProductStyle { ProductID = productID, StyleID = 6 }  // Casual
                            });
                        }
                        break;

                    case 2: // Quần
                        if (productID == 44) // Quần short Lacoste
                        {
                            productStyles.Add(new ProductStyle { ProductID = productID, StyleID = 2 }); // Sport
                        }
                        else if (brandId == 6 || brandId == 10) // Gucci, Chanel
                        {
                            productStyles.Add(new ProductStyle { ProductID = productID, StyleID = 3 }); // Formal
                        }
                        else if (brandId == 7) // Levi's
                        {
                            productStyles.AddRange(new List<ProductStyle>
                            {
                                new ProductStyle { ProductID = productID, StyleID = 5 }, // Vintage
                                new ProductStyle { ProductID = productID, StyleID = 6 }  // Casual
                            });
                        }
                        else // Nike, Adidas, Zara, H&M, Uniqlo, Lacoste, Puma
                        {
                            productStyles.Add(new ProductStyle { ProductID = productID, StyleID = 6 }); // Casual
                        }
                        break;

                    case 3: // Giày
                        if (productID == 15 || productID == 57) // Giày cao gót Zara, Chanel
                        {
                            productStyles.Add(new ProductStyle { ProductID = productID, StyleID = 3 }); // Formal
                        }
                        else if (brandId == 1 || brandId == 2 || brandId == 9) // Nike, Adidas, Puma
                        {
                            productStyles.Add(new ProductStyle { ProductID = productID, StyleID = 2 }); // Sport
                        }
                        else // Zara, H&M, Uniqlo, Gucci, Levi's, Lacoste
                        {
                            productStyles.AddRange(new List<ProductStyle>
                            {
                                new ProductStyle { ProductID = productID, StyleID = 4 }, // Streetwear
                                new ProductStyle { ProductID = productID, StyleID = 6 }  // Casual
                            });
                        }
                        break;

                    case 4: // Váy
                        if (productID == 58) // Váy dạ hội Chanel
                        {
                            productStyles.Add(new ProductStyle { ProductID = productID, StyleID = 3 }); // Formal
                        }
                        else if (brandId == 1 || brandId == 2 || brandId == 9) // Nike, Adidas, Puma
                        {
                            productStyles.Add(new ProductStyle { ProductID = productID, StyleID = 2 }); // Sport
                        }
                        else // Zara, H&M, Uniqlo, Gucci, Levi's, Lacoste
                        {
                            productStyles.Add(new ProductStyle { ProductID = productID, StyleID = 6 }); // Casual
                        }
                        break;

                    case 5: // Áo thun
                        if (brandId == 6 || brandId == 10) // Gucci, Chanel
                        {
                            productStyles.AddRange(new List<ProductStyle>
                            {
                                new ProductStyle { ProductID = productID, StyleID = 3 }, // Formal
                                new ProductStyle { ProductID = productID, StyleID = 4 }  // Streetwear
                            });
                        }
                        else if (brandId == 1 || brandId == 2 || brandId == 9) // Nike, Adidas, Puma
                        {
                            productStyles.Add(new ProductStyle { ProductID = productID, StyleID = 2 }); // Sport
                        }
                        else if (brandId == 4 || brandId == 5) // H&M, Uniqlo
                        {
                            productStyles.Add(new ProductStyle { ProductID = productID, StyleID = 1 }); // Basic
                        }
                        else // Zara, Levi's, Lacoste
                        {
                            productStyles.Add(new ProductStyle { ProductID = productID, StyleID = 6 }); // Casual
                        }
                        break;

                    case 6: // Đồng hồ
                        if (brandId == 6 || brandId == 10) // Gucci, Chanel
                        {
                            productStyles.Add(new ProductStyle { ProductID = productID, StyleID = 3 }); // Formal
                        }
                        else if (brandId == 1 || brandId == 2 || brandId == 9) // Nike, Adidas, Puma
                        {
                            productStyles.Add(new ProductStyle { ProductID = productID, StyleID = 2 }); // Sport
                        }
                        else // Zara, H&M, Uniqlo, Levi's, Lacoste
                        {
                            productStyles.Add(new ProductStyle { ProductID = productID, StyleID = 6 }); // Casual
                        }
                        break;
                }
            }

            #endregion

            // Seed ProductStyle
            modelBuilder.Entity<Products>().HasData(products);
            modelBuilder.Entity<ProductStyle>().HasData(productStyles);
            modelBuilder.Entity<ProductSeason>().HasData(productSeasons);
        }

        public static List<Products> GetProducts()
        {
            #region Products Seed Data
            var products = new List<Products>
{
            new Products
            {
                ID = 1,
                Name = "Áo khoác nam Nike",
                NormalizedName = RemoveDiacritics("Áo khoác nam Nike").ToLower(),
                Description = "Áo khoác nam chính hãng Nike, phong cách thể thao",
                NormalizedDescription = RemoveDiacritics("Áo khoác nam chính hãng Nike, phong cách thể thao").ToLower(),
                OldPrice = 1800000,
                NewPrice = 1500000,
                Quantity = 100,
                PicturePath = "nike_jacket.jpg",
                Created = new DateTime(2025, 4, 18),
                BrandID = 1,  // Nike
                CateID = 1,   // Áo khoác
                GenderID = 1, // Nam
                FeaturedID = 2
            },
new Products
{
    ID = 2,
    Name = "Quần thể thao Nike",
    NormalizedName = RemoveDiacritics("Quần thể thao Nike").ToLower(),
    Description = "Quần thể thao Nike chất liệu thoáng mát, phù hợp tập luyện",
    NormalizedDescription = RemoveDiacritics("Quần thể thao Nike chất liệu thoáng mát, phù hợp tập luyện").ToLower(),
    OldPrice = 0,
    NewPrice = 900000,
    Quantity = 80,
    PicturePath = "nike_pants.webp",
    Created = new DateTime(2025, 4, 18),
    BrandID = 1,
    CateID = 2,   // Quần
    GenderID = 1, // Nam
    FeaturedID = 2
},
new Products
{
    ID = 3,
    Name = "Giày Nike Air Max",
    NormalizedName = RemoveDiacritics("Giày Nike Air Max").ToLower(),
    Description = "Giày thể thao Nike Air Max thoải mái và bền bỉ",
    NormalizedDescription = RemoveDiacritics("Giày thể thao Nike Air Max thoải mái và bền bỉ").ToLower(),
    OldPrice = 3000000,
    NewPrice = 2800000,
    Quantity = 60,
    PicturePath = "nike_shoes.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 1,
    CateID = 3,   // Giày
    GenderID = 1, // Nam
    FeaturedID = 2
},
new Products
{
    ID = 4,
    Name = "Váy thể thao Nike nữ",
    NormalizedName = RemoveDiacritics("Váy thể thao Nike nữ").ToLower(),
    Description = "Váy thể thao nữ năng động, thiết kế hiện đại",
    NormalizedDescription = RemoveDiacritics("Váy thể thao nữ năng động, thiết kế hiện đại").ToLower(),
    OldPrice = 0,
    NewPrice = 1100000,
    Quantity = 40,
    PicturePath = "nike_dress.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 1,
    CateID = 4,   // Váy
    GenderID = 2, // Nữ
    FeaturedID = 2
},
new Products
{
    ID = 5,
    Name = "Áo thun Nike basic",
    NormalizedName = RemoveDiacritics("Áo thun Nike basic").ToLower(),
    Description = "Áo thun nam Nike cổ tròn, chất liệu cotton thoải mái",
    NormalizedDescription = RemoveDiacritics("Áo thun nam Nike cổ tròn, chất liệu cotton thoải mái").ToLower(),
    OldPrice = 650000,
    NewPrice = 590000,
    Quantity = 200,
    PicturePath = "nike_tshirt.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 1,
    CateID = 5,   // Áo thun
    GenderID = 1, // Nam
    FeaturedID = 2
},
new Products
{
    ID = 6,
    Name = "Đồng hồ thể thao Nike",
    NormalizedName = RemoveDiacritics("Đồng hồ thể thao Nike").ToLower(),
    Description = "Đồng hồ thể thao phong cách Nike, chống nước, dây silicon",
    NormalizedDescription = RemoveDiacritics("Đồng hồ thể thao phong cách Nike, chống nước, dây silicon").ToLower(),
    OldPrice = 0,
    NewPrice = 1900000,
    Quantity = 25,
    PicturePath = "nike_watch.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 1,
    CateID = 6,   // Đồng hồ
    GenderID = 3, // Unisex
    FeaturedID = 2
},
new Products
{
    ID = 7,
    Name = "Áo khoác Adidas Originals",
    NormalizedName = RemoveDiacritics("Áo khoác Adidas Originals").ToLower(),
    Description = "Thiết kế cổ điển, chống gió và giữ ấm tốt",
    NormalizedDescription = RemoveDiacritics("Thiết kế cổ điển, chống gió và giữ ấm tốt").ToLower(),
    OldPrice = 1600000,
    NewPrice = 1450000,
    Quantity = 70,
    PicturePath = "adidas_jacket.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 2,
    CateID = 1,   // Áo khoác
    GenderID = 1, // Nam
    FeaturedID = 2
},
new Products
{
    ID = 8,
    Name = "Quần jogger Adidas",
    NormalizedName = RemoveDiacritics("Quần jogger Adidas").ToLower(),
    Description = "Quần thể thao thoải mái, phù hợp vận động",
    NormalizedDescription = RemoveDiacritics("Quần thể thao thoải mái, phù hợp vận động").ToLower(),
    OldPrice = 0,
    NewPrice = 850000,
    Quantity = 90,
    PicturePath = "adidas_pants.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 2,
    CateID = 2,   // Quần
    GenderID = 1, // Nam
    FeaturedID = 2
},
new Products
{
    ID = 9,
    Name = "Giày Adidas Ultraboost",
    NormalizedName = RemoveDiacritics("Giày Adidas Ultraboost").ToLower(),
    Description = "Đệm Boost êm ái, hỗ trợ chạy bộ hiệu quả",
    NormalizedDescription = RemoveDiacritics("Đệm Boost êm ái, hỗ trợ chạy bộ hiệu quả").ToLower(),
    OldPrice = 3400000,
    NewPrice = 3200000,
    Quantity = 50,
    PicturePath = "adidas_shoes.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 2,
    CateID = 3,   // Giày
    GenderID = 3, // Unisex
    FeaturedID = 2
},
new Products
{
    ID = 10,
    Name = "Váy tennis Adidas nữ",
    NormalizedName = RemoveDiacritics("Váy tennis Adidas nữ").ToLower(),
    Description = "Váy thể thao nhẹ, thấm hút mồ hôi tốt",
    NormalizedDescription = RemoveDiacritics("Váy thể thao nhẹ, thấm hút mồ hôi tốt").ToLower(),
    OldPrice = 0,
    NewPrice = 990000,
    Quantity = 45,
    PicturePath = "adidas_dress.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 2,
    CateID = 4,   // Váy
    GenderID = 2, // Nữ
    FeaturedID = 2
},
new Products
{
    ID = 11,
    Name = "Áo thun Adidas cổ tròn",
    NormalizedName = RemoveDiacritics("Áo thun Adidas cổ tròn").ToLower(),
    Description = "Áo thun cổ điển, logo 3 sọc đặc trưng",
    NormalizedDescription = RemoveDiacritics("Áo thun cổ điển, logo 3 sọc đặc trưng").ToLower(),
    OldPrice = 700000,
    NewPrice = 650000,
    Quantity = 150,
    PicturePath = "adidas_tshirt.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 2,
    CateID = 5,   // Áo thun
    GenderID = 1, // Nam
    FeaturedID = 2
},
new Products
{
    ID = 12,
    Name = "Đồng hồ thể thao Adidas",
    NormalizedName = RemoveDiacritics("Đồng hồ thể thao Adidas").ToLower(),
    Description = "Thiết kế năng động, chống nước tốt",
    NormalizedDescription = RemoveDiacritics("Thiết kế năng động, chống nước tốt").ToLower(),
    OldPrice = 0,
    NewPrice = 1750000,
    Quantity = 30,
    PicturePath = "adidas_watch.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 2,
    CateID = 6,   // Đồng hồ
    GenderID = 3, // Unisex
    FeaturedID = 2
},
new Products
{
    ID = 13,
    Name = "Áo khoác Zara Dáng Dài",
    NormalizedName = RemoveDiacritics("Áo khoác Zara Dáng Dài").ToLower(),
    Description = "Áo khoác nữ dáng dài thanh lịch",
    NormalizedDescription = RemoveDiacritics("Áo khoác nữ dáng dài thanh lịch").ToLower(),
    OldPrice = 2000000,
    NewPrice = 1900000,
    Quantity = 60,
    PicturePath = "zara_jacket.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 3,
    CateID = 1,   // Áo khoác
    GenderID = 2, // Nữ
    FeaturedID = 2
},
new Products
{
    ID = 14,
    Name = "Quần Ống Rộng Zara",
    NormalizedName = RemoveDiacritics("Quần Ống Rộng Zara").ToLower(),
    Description = "Phong cách hiện đại, thời trang",
    NormalizedDescription = RemoveDiacritics("Phong cách hiện đại, thời trang").ToLower(),
    OldPrice = 0,
    NewPrice = 980000,
    Quantity = 40,
    PicturePath = "zara_pants.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 3,
    CateID = 2,   // Quần
    GenderID = 2, // Nữ
    FeaturedID = 2
},
new Products
{
    ID = 15,
    Name = "Giày Cao Gót Zara",
    NormalizedName = RemoveDiacritics("Giày Cao Gót Zara").ToLower(),
    Description = "Thiết kế sang trọng, phù hợp tiệc tùng",
    NormalizedDescription = RemoveDiacritics("Thiết kế sang trọng, phù hợp tiệc tùng").ToLower(),
    OldPrice = 1300000,
    NewPrice = 1200000,
    Quantity = 55,
    PicturePath = "zara_heels.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 3,
    CateID = 3,   // Giày
    GenderID = 2, // Nữ
    FeaturedID = 2
},
new Products
{
    ID = 16,
    Name = "Váy Xếp Ly Zara",
    NormalizedName = RemoveDiacritics("Váy Xếp Ly Zara").ToLower(),
    Description = "Váy dài thướt tha, thanh lịch",
    NormalizedDescription = RemoveDiacritics("Váy dài thướt tha, thanh lịch").ToLower(),
    OldPrice = 0,
    NewPrice = 1250000,
    Quantity = 35,
    PicturePath = "zara_dress.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 3,
    CateID = 4,   // Váy
    GenderID = 2, // Nữ
    FeaturedID = 2
},
new Products
{
    ID = 17,
    Name = "Áo Thun Zara Basic",
    NormalizedName = RemoveDiacritics("Áo Thun Zara Basic").ToLower(),
    Description = "Áo thun đơn giản, dễ phối đồ",
    NormalizedDescription = RemoveDiacritics("Áo thun đơn giản, dễ phối đồ").ToLower(),
    OldPrice = 500000,
    NewPrice = 450000,
    Quantity = 90,
    PicturePath = "zara_tshirt.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 3,
    CateID = 5,   // Áo thun
    GenderID = 3, // Unisex
    FeaturedID = 2
},
new Products
{
    ID = 18,
    Name = "Đồng Hồ Kim Zara",
    NormalizedName = RemoveDiacritics("Đồng Hồ Kim Zara").ToLower(),
    Description = "Thiết kế mặt tròn, dây da tinh tế",
    NormalizedDescription = RemoveDiacritics("Thiết kế mặt tròn, dây da tinh tế").ToLower(),
    OldPrice = 0,
    NewPrice = 1350000,
    Quantity = 25,
    PicturePath = "zara_watch.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 3,
    CateID = 6,   // Đồng hồ
    GenderID = 2, // Nữ
    FeaturedID = 2
},
new Products
{
    ID = 19,
    Name = "Áo Khoác Dù H&M",
    NormalizedName = RemoveDiacritics("Áo Khoác Dù H&M").ToLower(),
    Description = "Chống gió, nhẹ và gọn",
    NormalizedDescription = RemoveDiacritics("Chống gió, nhẹ và gọn").ToLower(),
    OldPrice = 1000000,
    NewPrice = 950000,
    Quantity = 80,
    PicturePath = "hm_jacket.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 4,
    CateID = 1,   // Áo khoác
    GenderID = 1, // Nam
    FeaturedID = 2
},
new Products
{
    ID = 20,
    Name = "Quần Tây H&M",
    NormalizedName = RemoveDiacritics("Quần Tây H&M").ToLower(),
    Description = "Phong cách công sở lịch sự",
    NormalizedDescription = RemoveDiacritics("Phong cách công sở lịch sự").ToLower(),
    OldPrice = 0,
    NewPrice = 700000,
    Quantity = 65,
    PicturePath = "hm_pants.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 4,
    CateID = 2,   // Quần
    GenderID = 1, // Nam
    FeaturedID = 2
},
new Products
{
    ID = 21,
    Name = "Giày Thể Thao H&M",
    NormalizedName = RemoveDiacritics("Giày Thể Thao H&M").ToLower(),
    Description = "Giày đơn giản, năng động",
    NormalizedDescription = RemoveDiacritics("Giày đơn giản, năng động").ToLower(),
    OldPrice = 900000,
    NewPrice = 820000,
    Quantity = 100,
    PicturePath = "hm_shoes.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 4,
    CateID = 3,   // Giày
    GenderID = 3, // Unisex
    FeaturedID = 2
},
new Products
{
    ID = 22,
    Name = "Váy Ngắn Hoa Nhí H&M",
    NormalizedName = RemoveDiacritics("Váy Ngắn Hoa Nhí H&M").ToLower(),
    Description = "Váy xinh xắn cho mùa hè",
    NormalizedDescription = RemoveDiacritics("Váy xinh xắn cho mùa hè").ToLower(),
    OldPrice = 0,
    NewPrice = 600000,
    Quantity = 45,
    PicturePath = "hm_dress.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 4,
    CateID = 4,   // Váy
    GenderID = 2, // Nữ
    FeaturedID = 2
},
new Products
{
    ID = 23,
    Name = "Áo Thun Nam Basic H&M",
    NormalizedName = RemoveDiacritics("Áo Thun Nam Basic H&M").ToLower(),
    Description = "Áo thun trơn giá rẻ",
    NormalizedDescription = RemoveDiacritics("Áo thun trơn giá rẻ").ToLower(),
    OldPrice = 250000,
    NewPrice = 200000,
    Quantity = 150,
    PicturePath = "hm_tshirt.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 4,
    CateID = 5,   // Áo thun
    GenderID = 1, // Nam
    FeaturedID = 2
},
new Products
{
    ID = 24,
    Name = "Đồng Hồ Dây Silicon H&M",
    NormalizedName = RemoveDiacritics("Đồng Hồ Dây Silicon H&M").ToLower(),
    Description = "Thiết kế đơn giản, hiện đại",
    NormalizedDescription = RemoveDiacritics("Thiết kế đơn giản, hiện đại").ToLower(),
    OldPrice = 0,
    NewPrice = 550000,
    Quantity = 40,
    PicturePath = "hm_watch.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 4,
    CateID = 6,   // Đồng hồ
    GenderID = 3, // Unisex
    FeaturedID = 2
},
new Products
{
    ID = 25,
    Name = "Áo khoác lông vũ Uniqlo",
    NormalizedName = RemoveDiacritics("Áo khoác lông vũ Uniqlo").ToLower(),
    Description = "Giữ ấm nhẹ nhàng, tiện lợi",
    NormalizedDescription = RemoveDiacritics("Giữ ấm nhẹ nhàng, tiện lợi").ToLower(),
    OldPrice = 1900000,
    NewPrice = 1750000,
    Quantity = 70,
    PicturePath = "uniqlo_jacket.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 5,
    CateID = 1,   // Áo khoác
    GenderID = 1, // Nam
    FeaturedID = 2
},
new Products
{
    ID = 26,
    Name = "Quần Jean Uniqlo",
    NormalizedName = RemoveDiacritics("Quần Jean Uniqlo").ToLower(),
    Description = "Jean co giãn, phong cách tối giản",
    NormalizedDescription = RemoveDiacritics("Jean co giãn, phong cách tối giản").ToLower(),
    OldPrice = 0,
    NewPrice = 950000,
    Quantity = 50,
    PicturePath = "uniqlo_jeans.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 5,
    CateID = 2,   // Quần
    GenderID = 1, // Nam
    FeaturedID = 2
},
new Products
{
    ID = 27,
    Name = "Giày Sneaker Uniqlo",
    NormalizedName = RemoveDiacritics("Giày Sneaker Uniqlo").ToLower(),
    Description = "Thiết kế tối giản, thoải mái",
    NormalizedDescription = RemoveDiacritics("Thiết kế tối giản, thoải mái").ToLower(),
    OldPrice = 1150000,
    NewPrice = 1050000,
    Quantity = 60,
    PicturePath = "uniqlo_shoes.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 5,
    CateID = 3,   // Giày
    GenderID = 1, // Nam
    FeaturedID = 2
},
new Products
{
    ID = 28,
    Name = "Váy Cotton Uniqlo",
    NormalizedName = RemoveDiacritics("Váy Cotton Uniqlo").ToLower(),
    Description = "Thiết kế tối giản, thoải mái cho mùa hè",
    NormalizedDescription = RemoveDiacritics("Thiết kế tối giản, thoải mái cho mùa hè").ToLower(),
    OldPrice = 0,
    NewPrice = 850000,
    Quantity = 40,
    PicturePath = "uniqlo_dress.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 5,
    CateID = 4,   // Váy
    GenderID = 2, // Nữ
    FeaturedID = 2
},
new Products
{
    ID = 29,
    Name = "Áo thun Airism Uniqlo",
    NormalizedName = RemoveDiacritics("Áo thun Airism Uniqlo").ToLower(),
    Description = "Thoáng mát, nhanh khô, thích hợp mùa hè",
    NormalizedDescription = RemoveDiacritics("Thoáng mát, nhanh khô, thích hợp mùa hè").ToLower(),
    OldPrice = 450000,
    NewPrice = 400000,
    Quantity = 120,
    PicturePath = "uniqlo_tshirt.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 5,
    CateID = 5,   // Áo thun
    GenderID = 3, // Unisex
    FeaturedID = 2
},
new Products
{
    ID = 30,
    Name = "Đồng hồ tối giản Uniqlo",
    NormalizedName = RemoveDiacritics("Đồng hồ tối giản Uniqlo").ToLower(),
    Description = "Phong cách tối giản, dây da đen",
    NormalizedDescription = RemoveDiacritics("Phong cách tối giản, dây da đen").ToLower(),
    OldPrice = 0,
    NewPrice = 980000,
    Quantity = 30,
    PicturePath = "uniqlo_watch.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 5,
    CateID = 6,   // Đồng hồ
    GenderID = 1, // Nam
    FeaturedID = 2
},
new Products
{
    ID = 31,
    Name = "Áo khoác cao cấp Gucci",
    NormalizedName = RemoveDiacritics("Áo khoác cao cấp Gucci").ToLower(),
    Description = "Thiết kế sang trọng, dành cho thời trang cao cấp",
    NormalizedDescription = RemoveDiacritics("Thiết kế sang trọng, dành cho thời trang cao cấp").ToLower(),
    OldPrice = 27000000,
    NewPrice = 25000000,
    Quantity = 10,
    PicturePath = "gucci_jacket.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 6,
    CateID = 1,   // Áo khoác
    GenderID = 2, // Nữ
    FeaturedID = 2
},
new Products
{
    ID = 32,
    Name = "Quần tây Gucci nam",
    NormalizedName = RemoveDiacritics("Quần tây Gucci nam").ToLower(),
    Description = "Phong cách lịch lãm, chất liệu cao cấp",
    NormalizedDescription = RemoveDiacritics("Phong cách lịch lãm, chất liệu cao cấp").ToLower(),
    OldPrice = 0,
    NewPrice = 15500000,
    Quantity = 15,
    PicturePath = "gucci_pants.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 6,
    CateID = 2,   // Quần
    GenderID = 1, // Nam
    FeaturedID = 2
},
new Products
{
    ID = 33,
    Name = "Giày lười Gucci",
    NormalizedName = RemoveDiacritics("Giày lười Gucci").ToLower(),
    Description = "Thiết kế logo đặc trưng, đẳng cấp",
    NormalizedDescription = RemoveDiacritics("Thiết kế logo đặc trưng, đẳng cấp").ToLower(),
    OldPrice = 24000000,
    NewPrice = 22000000,
    Quantity = 12,
    PicturePath = "gucci_shoes.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 6,
    CateID = 3,   // Giày
    GenderID = 1, // Nam
    FeaturedID = 2
},
new Products
{
    ID = 34,
    Name = "Váy lụa cao cấp Gucci",
    NormalizedName = RemoveDiacritics("Váy lụa cao cấp Gucci").ToLower(),
    Description = "Chất liệu mềm mại, sang trọng",
    NormalizedDescription = RemoveDiacritics("Chất liệu mềm mại, sang trọng").ToLower(),
    OldPrice = 0,
    NewPrice = 30000000,
    Quantity = 8,
    PicturePath = "gucci_dress.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 6,
    CateID = 4,   // Váy
    GenderID = 2, // Nữ
    FeaturedID = 2
},
new Products
{
    ID = 35,
    Name = "Áo thun Gucci logo lớn",
    NormalizedName = RemoveDiacritics("Áo thun Gucci logo lớn").ToLower(),
    Description = "Phong cách trẻ trung, thời thượng",
    NormalizedDescription = RemoveDiacritics("Phong cách trẻ trung, thời thượng").ToLower(),
    OldPrice = 9500000,
    NewPrice = 9000000,
    Quantity = 20,
    PicturePath = "gucci_tshirt.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 6,
    CateID = 5,   // Áo thun
    GenderID = 3, // Unisex
    FeaturedID = 2
},
new Products
{
    ID = 36,
    Name = "Đồng hồ đính đá Gucci",
    NormalizedName = RemoveDiacritics("Đồng hồ đính đá Gucci").ToLower(),
    Description = "Đồng hồ xa xỉ với thiết kế tinh xảo",
    NormalizedDescription = RemoveDiacritics("Đồng hồ xa xỉ với thiết kế tinh xảo").ToLower(),
    OldPrice = 0,
    NewPrice = 45000000,
    Quantity = 5,
    PicturePath = "gucci_watch.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 6,
    CateID = 6,   // Đồng hồ
    GenderID = 2, // Nữ
    FeaturedID = 2
},
new Products
{
    ID = 37,
    Name = "Áo khoác jean Levi's",
    NormalizedName = RemoveDiacritics("Áo khoác jean Levi's").ToLower(),
    Description = "Chất liệu jean bền, phong cách cổ điển",
    NormalizedDescription = RemoveDiacritics("Chất liệu jean bền, phong cách cổ điển").ToLower(),
    OldPrice = 1600000,
    NewPrice = 1450000,
    Quantity = 40,
    PicturePath = "levis_jacket.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 7,
    CateID = 1,   // Áo khoác
    GenderID = 1, // Nam
    FeaturedID = 2
},
new Products
{
    ID = 38,
    Name = "Quần jeans Levi's 501",
    NormalizedName = RemoveDiacritics("Quần jeans Levi's 501").ToLower(),
    Description = "Form dáng chuẩn Mỹ, chất lượng cao",
    NormalizedDescription = RemoveDiacritics("Form dáng chuẩn Mỹ, chất lượng cao").ToLower(),
    OldPrice = 0,
    NewPrice = 1250000,
    Quantity = 60,
    PicturePath = "levis_jeans.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 7,
    CateID = 2,   // Quần
    GenderID = 1, // Nam
    FeaturedID = 2
},
new Products
{
    ID = 39,
    Name = "Giày thể thao Levi's",
    NormalizedName = RemoveDiacritics("Giày thể thao Levi's").ToLower(),
    Description = "Thoải mái và thời trang hàng ngày",
    NormalizedDescription = RemoveDiacritics("Thoải mái và thời trang hàng ngày").ToLower(),
    OldPrice = 1250000,
    NewPrice = 1150000,
    Quantity = 35,
    PicturePath = "levis_shoes.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 7,
    CateID = 3,   // Giày
    GenderID = 1, // Nam
    FeaturedID = 2
},
new Products
{
    ID = 40,
    Name = "Váy bò Levi's nữ",
    NormalizedName = RemoveDiacritics("Váy bò Levi's nữ").ToLower(),
    Description = "Thiết kế trẻ trung, năng động",
    NormalizedDescription = RemoveDiacritics("Thiết kế trẻ trung, năng động").ToLower(),
    OldPrice = 0,
    NewPrice = 980000,
    Quantity = 20,
    PicturePath = "levis_dress.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 7,
    CateID = 4,   // Váy
    GenderID = 2, // Nữ
    FeaturedID = 2
},
new Products
{
    ID = 41,
    Name = "Áo thun cổ tròn Levi's",
    NormalizedName = RemoveDiacritics("Áo thun cổ tròn Levi's").ToLower(),
    Description = "Logo cổ điển, phong cách Mỹ",
    NormalizedDescription = RemoveDiacritics("Logo cổ điển, phong cách Mỹ").ToLower(),
    OldPrice = 550000,
    NewPrice = 500000,
    Quantity = 100,
    PicturePath = "levis_tshirt.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 7,
    CateID = 5,   // Áo thun
    GenderID = 3, // Unisex
    FeaturedID = 2
},
new Products
{
    ID = 42,
    Name = "Đồng hồ dây da Levi's",
    NormalizedName = RemoveDiacritics("Đồng hồ dây da Levi's").ToLower(),
    Description = "Đơn giản, mạnh mẽ, cá tính",
    NormalizedDescription = RemoveDiacritics("Đơn giản, mạnh mẽ, cá tính").ToLower(),
    OldPrice = 0,
    NewPrice = 850000,
    Quantity = 15,
    PicturePath = "levis_watch.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 7,
    CateID = 6,   // Đồng hồ
    GenderID = 1, // Nam
    FeaturedID = 2
},
new Products
{
    ID = 43,
    Name = "Áo khoác thể thao Lacoste",
    NormalizedName = RemoveDiacritics("Áo khoác thể thao Lacoste").ToLower(),
    Description = "Thiết kế năng động, thời trang",
    NormalizedDescription = RemoveDiacritics("Thiết kế năng động, thời trang").ToLower(),
    OldPrice = 2900000,
    NewPrice = 2700000,
    Quantity = 30,
    PicturePath = "lacoste_jacket.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 8,
    CateID = 1,   // Áo khoác
    GenderID = 3, // Unisex
    FeaturedID = 2
},
new Products
{
    ID = 44,
    Name = "Quần short Lacoste",
    NormalizedName = RemoveDiacritics("Quần short Lacoste").ToLower(),
    Description = "Phong cách thể thao, thoáng mát",
    NormalizedDescription = RemoveDiacritics("Phong cách thể thao, thoáng mát").ToLower(),
    OldPrice = 0,
    NewPrice = 1450000,
    Quantity = 40,
    PicturePath = "lacoste_short.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 8,
    CateID = 2,   // Quần
    GenderID = 1, // Nam
    FeaturedID = 2
},
new Products
{
    ID = 45,
    Name = "Giày sneaker Lacoste",
    NormalizedName = RemoveDiacritics("Giày sneaker Lacoste").ToLower(),
    Description = "Chất liệu cao cấp, thiết kế tinh tế",
    NormalizedDescription = RemoveDiacritics("Chất liệu cao cấp, thiết kế tinh tế").ToLower(),
    OldPrice = 2400000,
    NewPrice = 2200000,
    Quantity = 25,
    PicturePath = "lacoste_shoes.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 8,
    CateID = 3,   // Giày
    GenderID = 1, // Nam
    FeaturedID = 2
},
new Products
{
    ID = 46,
    Name = "Váy polo Lacoste",
    NormalizedName = RemoveDiacritics("Váy polo Lacoste").ToLower(),
    Description = "Lịch sự, sang trọng cho nữ",
    NormalizedDescription = RemoveDiacritics("Lịch sự, sang trọng cho nữ").ToLower(),
    OldPrice = 0,
    NewPrice = 1900000,
    Quantity = 18,
    PicturePath = "lacoste_dress.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 8,
    CateID = 4,   // Váy
    GenderID = 2, // Nữ
    FeaturedID = 2
},
new Products
{
    ID = 47,
    Name = "Áo thun cá sấu Lacoste",
    NormalizedName = RemoveDiacritics("Áo thun cá sấu Lacoste").ToLower(),
    Description = "Biểu tượng nổi bật với logo cá sấu",
    NormalizedDescription = RemoveDiacritics("Biểu tượng nổi bật với logo cá sấu").ToLower(),
    OldPrice = 1200000,
    NewPrice = 1100000,
    Quantity = 55,
    PicturePath = "lacoste_tshirt.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 8,
    CateID = 5,   // Áo thun
    GenderID = 1, // Nam
    FeaturedID = 2
},
new Products
{
    ID = 48,
    Name = "Đồng hồ thể thao Lacoste",
    NormalizedName = RemoveDiacritics("Đồng hồ thể thao Lacoste").ToLower(),
    Description = "Năng động, trẻ trung, phù hợp với người chơi thể thao",
    NormalizedDescription = RemoveDiacritics("Năng động, trẻ trung, phù hợp với người chơi thể thao").ToLower(),
    OldPrice = 0,
    NewPrice = 2600000,
    Quantity = 20,
    PicturePath = "lacoste_watch.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 8,
    CateID = 6,   // Đồng hồ
    GenderID = 3, // Unisex
    FeaturedID = 2
},
new Products
{
    ID = 49,
    Name = "Áo khoác thể thao Puma",
    NormalizedName = RemoveDiacritics("Áo khoác thể thao Puma").ToLower(),
    Description = "Thiết kế hiện đại, năng động",
    NormalizedDescription = RemoveDiacritics("Thiết kế hiện đại, năng động").ToLower(),
    OldPrice = 1700000,
    NewPrice = 1550000,
    Quantity = 50,
    PicturePath = "puma_jacket.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 9,
    CateID = 1,   // Áo khoác
    GenderID = 1, // Nam
    FeaturedID = 2
},
new Products
{
    ID = 50,
    Name = "Quần jogger Puma",
    NormalizedName = RemoveDiacritics("Quần jogger Puma").ToLower(),
    Description = "Thoải mái, phù hợp vận động",
    NormalizedDescription = RemoveDiacritics("Thoải mái, phù hợp vận động").ToLower(),
    OldPrice = 0,
    NewPrice = 1100000,
    Quantity = 70,
    PicturePath = "puma_pants.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 9,
    CateID = 2,   // Quần
    GenderID = 1, // Nam
    FeaturedID = 2
},
new Products
{
    ID = 51,
    Name = "Giày chạy bộ Puma",
    NormalizedName = RemoveDiacritics("Giày chạy bộ Puma").ToLower(),
    Description = "Thiết kế thể thao, hỗ trợ di chuyển",
    NormalizedDescription = RemoveDiacritics("Thiết kế thể thao, hỗ trợ di chuyển").ToLower(),
    OldPrice = 2000000,
    NewPrice = 1800000,
    Quantity = 40,
    PicturePath = "puma_shoes.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 9,
    CateID = 3,   // Giày
    GenderID = 1, // Nam
    FeaturedID = 2
},
new Products
{
    ID = 52,
    Name = "Váy thể thao Puma nữ",
    NormalizedName = RemoveDiacritics("Váy thể thao Puma nữ").ToLower(),
    Description = "Phù hợp tập luyện hoặc mặc thường ngày",
    NormalizedDescription = RemoveDiacritics("Phù hợp tập luyện hoặc mặc thường ngày").ToLower(),
    OldPrice = 0,
    NewPrice = 950000,
    Quantity = 25,
    PicturePath = "puma_dress.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 9,
    CateID = 4,   // Váy
    GenderID = 2, // Nữ
    FeaturedID = 2
},
new Products
{
    ID = 53,
    Name = "Áo thun thể thao Puma",
    NormalizedName = RemoveDiacritics("Áo thun thể thao Puma").ToLower(),
    Description = "Chất vải thấm hút, thoáng mát",
    NormalizedDescription = RemoveDiacritics("Chất vải thấm hút, thoáng mát").ToLower(),
    OldPrice = 650000,
    NewPrice = 600000,
    Quantity = 90,
    PicturePath = "puma_tshirt.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 9,
    CateID = 5,   // Áo thun
    GenderID = 3, // Unisex
    FeaturedID = 2
},
new Products
{
    ID = 54,
    Name = "Đồng hồ thể thao Puma",
    NormalizedName = RemoveDiacritics("Đồng hồ thể thao Puma").ToLower(),
    Description = "Thiết kế mạnh mẽ, phong cách",
    NormalizedDescription = RemoveDiacritics("Thiết kế mạnh mẽ, phong cách").ToLower(),
    OldPrice = 0,
    NewPrice = 1200000,
    Quantity = 18,
    PicturePath = "puma_watch.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 9,
    CateID = 6,   // Đồng hồ
    GenderID = 1, // Nam
    FeaturedID = 2
},
new Products
{
    ID = 55,
    Name = "Áo khoác dạ Chanel",
    NormalizedName = RemoveDiacritics("Áo khoác dạ Chanel").ToLower(),
    Description = "Đẳng cấp, thời thượng, thiết kế quý phái",
    NormalizedDescription = RemoveDiacritics("Đẳng cấp, thời thượng, thiết kế quý phái").ToLower(),
    OldPrice = 10000000,
    NewPrice = 9500000,
    Quantity = 15,
    PicturePath = "chanel_jacket.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 10,
    CateID = 1,   // Áo khoác
    GenderID = 2, // Nữ
    FeaturedID = 2
},
new Products
{
    ID = 56,
    Name = "Quần vải cao cấp Chanel",
    NormalizedName = RemoveDiacritics("Quần vải cao cấp Chanel").ToLower(),
    Description = "Đường may tinh xảo, chất liệu mềm mịn",
    NormalizedDescription = RemoveDiacritics("Đường may tinh xảo, chất liệu mềm mịn").ToLower(),
    OldPrice = 0,
    NewPrice = 7200000,
    Quantity = 12,
    PicturePath = "chanel_pants.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 10,
    CateID = 2,   // Quần
    GenderID = 2, // Nữ
    FeaturedID = 2
},
    new Products
    {
        ID = 57,
        Name = "Giày cao gót Chanel",
        NormalizedName = RemoveDiacritics("Giày cao gót Chanel").ToLower(),
        Description = "Thanh lịch, sang trọng",
        NormalizedDescription = RemoveDiacritics("Thanh lịch, sang trọng").ToLower(),
        OldPrice = 9000000,
        NewPrice = 8300000,
        Quantity = 10,
        PicturePath = "chanel_shoes.jpg",
        Created = new DateTime(2025, 4, 18),
        BrandID = 10,
        CateID = 3,   // Giày
        GenderID = 2, // Nữ
        FeaturedID = 2,
    },
new Products
{
    ID = 58,
    Name = "Váy dạ hội Chanel",
    NormalizedName = RemoveDiacritics("Váy dạ hội Chanel").ToLower(),
    Description = "Lộng lẫy, thiết kế quyến rũ",
    NormalizedDescription = RemoveDiacritics("Lộng lẫy, thiết kế quyến rũ").ToLower(),
    OldPrice = 0,
    NewPrice = 13000000,
    Quantity = 8,
    PicturePath = "chanel_dress.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 10,
    CateID = 4,   // Váy
    GenderID = 2, // Nữ
    FeaturedID = 2
},
new Products
{
    ID = 59,
    Name = "Áo thun cao cấp Chanel",
    NormalizedName = RemoveDiacritics("Áo thun cao cấp Chanel").ToLower(),
    Description = "Đơn giản nhưng đầy tinh tế",
    NormalizedDescription = RemoveDiacritics("Đơn giản nhưng đầy tinh tế").ToLower(),
    OldPrice = 3500000,
    NewPrice = 3200000,
    Quantity = 20,
    PicturePath = "chanel_tshirt.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 10,
    CateID = 5,   // Áo thun
    GenderID = 2, // Nữ
    FeaturedID = 2
},
new Products
{
    ID = 60,
    Name = "Đồng hồ Chanel sang trọng",
    NormalizedName = RemoveDiacritics("Đồng hồ Chanel sang trọng").ToLower(),
    Description = "Tinh xảo, sang trọng và nữ tính",
    NormalizedDescription = RemoveDiacritics("Tinh xảo, sang trọng và nữ tính").ToLower(),
    OldPrice = 0,
    NewPrice = 18000000,
    Quantity = 6,
    PicturePath = "chanel_watch.jpg",
    Created = new DateTime(2025, 4, 18),
    BrandID = 10,
    CateID = 6,   // Đồng hồ
    GenderID = 2, // Nữ
    FeaturedID = 2
}
            };
            #endregion

            return products;
        }


        public static string RemoveDiacritics(string text)
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
    }



}

