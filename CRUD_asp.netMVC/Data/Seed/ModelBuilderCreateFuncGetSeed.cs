using CRUD_asp.netMVC.Models.Account;
using CRUD_asp.netMVC.Models.Account.ActionViewModel;
using CRUD_asp.netMVC.Models.Product;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;

namespace CRUD_asp.netMVC.Data.Seed
{
    public static class ModelBuilderCreateFuncGetSeed
    {
        public static void RelationshipEntitys(this ModelBuilder modelBuilder)
        {
            // Mockup du lieu mqh 1 - n
            modelBuilder.Entity<Products>().HasOne(m => m.Brand).WithMany(p => p.products).HasForeignKey(mi => mi.BrandID).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Products>().HasOne(t => t.Cate).WithMany(p => p.products).HasForeignKey(ti => ti.CateID).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Products>().HasOne(p => p.Gender).WithMany(p => p.Products).HasForeignKey(p => p.GenderID).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AddToCart>().HasOne(p => p.Product).WithMany(c => c.Carts).HasForeignKey(pi => pi.ProductID).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<AddToCart>().HasOne(u => u.Users).WithMany(c => c.Carts).HasForeignKey(ui => ui.UserID).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderDetail>().HasOne(o => o.Orders).WithMany(od => od.OrderDetail).HasForeignKey(oi => oi.orderID).OnDelete(DeleteBehavior.Cascade);
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
            modelBuilder.Entity<ProductSeason>().HasOne(p => p.Product).WithMany(p => p.ProductSeason).HasForeignKey(p => p.ProductID);
            modelBuilder.Entity<ProductSeason>().HasOne(p => p.Season).WithMany(p => p.ProductSeason).HasForeignKey(p => p.SeasonID);

            modelBuilder.Entity<ProductTag>().HasKey(p => new { p.TagID, p.ProductID });
            modelBuilder.Entity<ProductTag>().HasOne(p => p.Product).WithMany(p => p.ProductTag).HasForeignKey(p => p.ProductID);
            modelBuilder.Entity<ProductTag>().HasOne(p => p.Tag).WithMany(p => p.ProductTag).HasForeignKey(p => p.TagID);

            modelBuilder.Entity<ProductStyle>().HasKey(p => new { p.StyleID, p.ProductID });
            modelBuilder.Entity<ProductStyle>().HasOne(p => p.Product).WithMany(p => p.ProductStyles).HasForeignKey(p => p.ProductID);
            modelBuilder.Entity<ProductStyle>().HasOne(p => p.Style).WithMany(p => p.ProductStyles).HasForeignKey(p => p.StyleID);

            modelBuilder.Entity<ProductSize>().HasKey(p => new { p.ProductID, p.SizeID });
            modelBuilder.Entity<ProductSize>().HasOne(p => p.products).WithMany(p => p.ProductSize).HasForeignKey(p => p.ProductID);
            modelBuilder.Entity<ProductSize>().HasOne(p => p.size).WithMany(p => p.ProductSize).HasForeignKey(p => p.SizeID);

            modelBuilder.Entity<ProductColor>().HasKey(p => new { p.ProductID, p.ColorID });
            modelBuilder.Entity<ProductColor>().HasOne(p => p.Product).WithMany(p => p.ProductColor).HasForeignKey(p => p.ProductID);
            modelBuilder.Entity<ProductColor>().HasOne(p => p.Color).WithMany(p => p.ProductColor).HasForeignKey(p => p.ColorID);

            modelBuilder.Entity<ProductMaterial>().HasKey(p => new { p.ProductID, p.MaterialID });
            modelBuilder.Entity<ProductMaterial>().HasOne(p => p.Product).WithMany(p => p.ProductMaterial).HasForeignKey(p => p.ProductID);
            modelBuilder.Entity<ProductMaterial>().HasOne(p => p.Material).WithMany(p => p.ProductMaterial).HasForeignKey(p => p.MaterialID);

            modelBuilder.Entity<ProductColor>().HasKey(p => new { p.ProductID, p.ColorID });
            modelBuilder.Entity<ProductColor>().HasOne(p => p.Product).WithMany(p => p.ProductColor).HasForeignKey(p => p.ProductID);
            modelBuilder.Entity<ProductColor>().HasOne(p => p.Color).WithMany(p => p.ProductColor).HasForeignKey(p => p.ColorID);

            //-------------------------------------------------------------------------
            modelBuilder.Entity<Login>().HasNoKey();
            modelBuilder.Entity<Register>().HasNoKey();

            var hashPass1 = new PasswordHasher<Users>().HashPassword(null, "admin123");
            var hashPass2 = new PasswordHasher<Users>().HashPassword(null, "123456");
        }

        public static void SeedRoels(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Roles>().HasData(

                 new Roles { Id = 1, Name = "Manager" },
                 new Roles { Id = 2, Name = "Customer" }
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
                new Size { ID = 5, Name = "42" },
                new Size { ID = 6, Name = "41" },
                new Size { ID = 7, Name = "Free" }
            );
        }

        public static void SeedBrand(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Brand>().HasData(
             new Brand { ID = 1, Name = "Nike", Description = "Thương hiệu giày thể thao và trang phục thể thao nổi tiếng" },
             new Brand { ID = 2, Name = "Adidas", Description = "Chuyên cung cấp giày dép và trang phục thể thao" },
             new Brand { ID = 3, Name = "Zara", Description = "Thương hiệu thời trang cao cấp với thiết kế hiện đại" },
             new Brand { ID = 4, Name = "H&M", Description = "H&M cung cấp thời trang giá cả phải chăng và phong cách hiện đại" },
              new Brand { ID = 5, Name = "Uniqlo", Description = "Thương hiệu Nhật Bản nổi tiếng với phong cách tối giản và chất liệu tốt" },
              new Brand { ID = 6, Name = "Gucci", Description = "Thương hiệu cao cấp đến từ Ý với các sản phẩm thời trang xa xỉ" },
              new Brand { ID = 7, Name = "Levi's", Description = "Hãng thời trang nổi tiếng với quần jeans và phong cách cổ điển Mỹ" },
              new Brand { ID = 8, Name = "Lacoste", Description = "Phong cách thể thao thanh lịch đến từ Pháp" },
              new Brand { ID = 9, Name = "Puma", Description = "Cung cấp thời trang thể thao, giày dép và phụ kiện chất lượng" },
              new Brand { ID = 10, Name = "Chanel", Description = "Biểu tượng thời trang xa xỉ với những thiết kế đẳng cấp, cổ điển" }
         );
        }

        public static void SeedCategory(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { ID = 1, Name = "Áo khoác" },
                new Category { ID = 2, Name = "Quần" },
                new Category { ID = 3, Name = "Giày" },
                new Category { ID = 4, Name = "Váy" },
                new Category { ID = 5, Name = "Áo thun" },
                new Category { ID = 6, Name = "Đồng hồ" }
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
            var ListProductColor = new List<ProductColor>();
            for (int productID = 1; productID <= 60; productID++)
            {
                ListProductColor.AddRange(new List<ProductColor>()
                {
                    new ProductColor { ProductID = productID, ColorID = 1 },
                    new ProductColor { ProductID = productID, ColorID = 2 },
                    new ProductColor { ProductID = productID, ColorID = 3 },
                    new ProductColor { ProductID = productID, ColorID = 5 },
                });
            }

            modelBuilder.Entity<ProductColor>().HasData(ListProductColor);
        }

        public static void SeedProductMaterial(this ModelBuilder modelBuilder)
        {
            // Tái sử dụng dữ liệu Products để tạo Dictionary
            var products = new List<Products>
            {
                new Products { ID = 1, CateID = 1, BrandID = 1 }, // Áo khoác nam Nike
                new Products { ID = 2, CateID = 2, BrandID = 1 }, // Quần thể thao Nike
                new Products { ID = 3, CateID = 3, BrandID = 1 }, // Giày Nike Air Max
                new Products { ID = 4, CateID = 4, BrandID = 1 }, // Váy thể thao Nike nữ
                new Products { ID = 5, CateID = 5, BrandID = 1 }, // Áo thun Nike basic
                new Products { ID = 6, CateID = 6, BrandID = 1 }, // Đồng hồ thể thao Nike
                new Products { ID = 7, CateID = 1, BrandID = 2 }, // Áo khoác Adidas Originals
                new Products { ID = 8, CateID = 2, BrandID = 2 }, // Quần jogger Adidas
                new Products { ID = 9, CateID = 3, BrandID = 2 }, // Giày Adidas Ultraboost
                new Products { ID = 10, CateID = 4, BrandID = 2 }, // Váy tennis Adidas nữ
                new Products { ID = 11, CateID = 5, BrandID = 2 }, // Áo thun Adidas cổ tròn
                new Products { ID = 12, CateID = 6, BrandID = 2 }, // Đồng hồ thể thao Adidas
                new Products { ID = 13, CateID = 1, BrandID = 3 }, // Áo khoác Zara Dáng Dài
                new Products { ID = 14, CateID = 2, BrandID = 3 }, // Quần Ống Rộng Zara
                new Products { ID = 15, CateID = 3, BrandID = 3 }, // Giày Cao Gót Zara
                new Products { ID = 16, CateID = 4, BrandID = 3 }, // Váy Xếp Ly Zara
                new Products { ID = 17, CateID = 5, BrandID = 3 }, // Áo Thun Zara Basic
                new Products { ID = 18, CateID = 6, BrandID = 3 }, // Đồng Hồ Kim Zara
                new Products { ID = 19, CateID = 1, BrandID = 4 }, // Áo Khoác Dù H&M
                new Products { ID = 20, CateID = 2, BrandID = 4 }, // Quần Tây H&M
                new Products { ID = 21, CateID = 3, BrandID = 4 }, // Giày Thể Thao H&M
                new Products { ID = 22, CateID = 4, BrandID = 4 }, // Váy Ngắn Hoa Nhí H&M
                new Products { ID = 23, CateID = 5, BrandID = 4 }, // Áo Thun Nam Basic H&M
                new Products { ID = 24, CateID = 6, BrandID = 4 }, // Đồng Hồ Dây Silicon H&M
                new Products { ID = 25, CateID = 1, BrandID = 5 }, // Áo khoác lông vũ Uniqlo
                new Products { ID = 26, CateID = 2, BrandID = 5 }, // Quần Jean Uniqlo
                new Products { ID = 27, CateID = 3, BrandID = 5 }, // Giày Sneaker Uniqlo
                new Products { ID = 28, CateID = 4, BrandID = 5 }, // Váy Cotton Uniqlo
                new Products { ID = 29, CateID = 5, BrandID = 5 }, // Áo thun Airism Uniqlo
                new Products { ID = 30, CateID = 6, BrandID = 5 }, // Đồng hồ tối giản Uniqlo
                new Products { ID = 31, CateID = 1, BrandID = 6 }, // Áo khoác cao cấp Gucci
                new Products { ID = 32, CateID = 2, BrandID = 6 }, // Quần tây Gucci nam
                new Products { ID = 33, CateID = 3, BrandID = 6 }, // Giày lười Gucci
                new Products { ID = 34, CateID = 4, BrandID = 6 }, // Váy lụa cao cấp Gucci
                new Products { ID = 35, CateID = 5, BrandID = 6 }, // Áo thun Gucci logo lớn
                new Products { ID = 36, CateID = 6, BrandID = 6 }, // Đồng hồ đính đá Gucci
                new Products { ID = 37, CateID = 1, BrandID = 7 }, // Áo khoác jean Levi's
                new Products { ID = 38, CateID = 2, BrandID = 7 }, // Quần jeans Levi's 501
                new Products { ID = 39, CateID = 3, BrandID = 7 }, // Giày thể thao Levi's
                new Products { ID = 40, CateID = 4, BrandID = 7 }, // Váy bò Levi's nữ
                new Products { ID = 41, CateID = 5, BrandID = 7 }, // Áo thun cổ tròn Levi's
                new Products { ID = 42, CateID = 6, BrandID = 7 }, // Đồng hồ dây da Levi's
                new Products { ID = 43, CateID = 1, BrandID = 8 }, // Áo khoác thể thao Lacoste
                new Products { ID = 44, CateID = 2, BrandID = 8 }, // Quần short Lacoste
                new Products { ID = 45, CateID = 3, BrandID = 8 }, // Giày sneaker Lacoste
                new Products { ID = 46, CateID = 4, BrandID = 8 }, // Váy polo Lacoste
                new Products { ID = 47, CateID = 5, BrandID = 8 }, // Áo thun cá sấu Lacoste
                new Products { ID = 48, CateID = 6, BrandID = 8 }, // Đồng hồ thể thao Lacoste
                new Products { ID = 49, CateID = 1, BrandID = 9 }, // Áo khoác thể thao Puma
                new Products { ID = 50, CateID = 2, BrandID = 9 }, // Quần jogger Puma
                new Products { ID = 51, CateID = 3, BrandID = 9 }, // Giày chạy bộ Puma
                new Products { ID = 52, CateID = 4, BrandID = 9 }, // Váy thể thao Puma nữ
                new Products { ID = 53, CateID = 5, BrandID = 9 }, // Áo thun thể thao Puma
                new Products { ID = 54, CateID = 6, BrandID = 9 }, // Đồng hồ thể thao Puma
                new Products { ID = 55, CateID = 1, BrandID = 10 }, // Áo khoác dạ Chanel
                new Products { ID = 56, CateID = 2, BrandID = 10 }, // Quần vải cao cấp Chanel
                new Products { ID = 57, CateID = 3, BrandID = 10 }, // Giày cao gót Chanel
                new Products { ID = 58, CateID = 4, BrandID = 10 }, // Váy dạ hội Chanel
                new Products { ID = 59, CateID = 5, BrandID = 10 }, // Áo thun cao cấp Chanel
                new Products { ID = 60, CateID = 6, BrandID = 10 }  // Đồng hồ Chanel sang trọng
            };

            // Tạo Dictionary ánh xạ ProductID sang CateID, BrandID
            var productMap = products.ToDictionary(
                p => p.ID,
                p => new { p.CateID, p.BrandID }
            );

            // Tạo danh sách ProductMaterial
            var productMaterials = new List<ProductMaterial>();

            for (int productId = 1; productId <= 60; productId++)
            {
                int cateId = productMap[productId].CateID;
                int brandId = productMap[productId].BrandID;

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

            // Seed ProductMaterial
            modelBuilder.Entity<ProductMaterial>().HasData(productMaterials);
        }

        public static void SeedProducts_ProductSeason_ProductStyle(this ModelBuilder modelBuilder)
        {
            #region Product
            var products = new List<Products>
                {
                new Products
                    {
                        ID = 1,
                        Name = "Áo khoác nam Nike",
                        Description = "Áo khoác nam chính hãng Nike, phong cách thể thao",
                        Price = 1500000,
                        Quantity = 100,
                        PicturePath = "jacket1.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 1,  // Nike
                        CateID = 1,   // Áo khoác
                        GenderID = 1  // Nam
                    },
                    new Products
                    {
                        ID = 2,
                        Name = "Quần thể thao Nike",
                        Description = "Quần thể thao Nike chất liệu thoáng mát, phù hợp tập luyện",
                        Price = 900000,
                        Quantity = 80,
                        PicturePath = "pants1.webp",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 1,
                        CateID = 2,   // Quần
                        GenderID = 1  // Nam
                    },
                    new Products
                    {
                        ID = 3,
                        Name = "Giày Nike Air Max",
                        Description = "Giày thể thao Nike Air Max thoải mái và bền bỉ",
                        Price = 2800000,
                        Quantity = 60,
                        PicturePath = "shoes1.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 1,
                        CateID = 3,   // Giày
                        GenderID = 1  // Nam
                    },
                    new Products
                    {
                        ID = 4,
                        Name = "Váy thể thao Nike nữ",
                        Description = "Váy thể thao nữ năng động, thiết kế hiện đại",
                        Price = 1100000,
                        Quantity = 40,
                        PicturePath = "dress1.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 1,
                        CateID = 4,   // Váy
                        GenderID = 2  // Nữ
                    },
                    new Products
                    {
                        ID = 5,
                        Name = "Áo thun Nike basic",
                        Description = "Áo thun nam Nike cổ tròn, chất liệu cotton thoải mái",
                        Price = 590000,
                        Quantity = 200,
                        PicturePath = "tshirt1.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 1,
                        CateID = 5,   // Áo thun
                        GenderID = 1  // Nam
                    },
                    new Products
                    {
                        ID = 6,
                        Name = "Đồng hồ thể thao Nike",
                        Description = "Đồng hồ thể thao phong cách Nike, chống nước, dây silicon",
                        Price = 1900000,
                        Quantity = 25,
                        PicturePath = "watch1.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 1,
                        CateID = 6,   // Đồng hồ
                        GenderID = 3  // Unisex
                    },
                    new Products
                    {
                        ID = 7,
                        Name = "Áo khoác Adidas Originals",
                        Description = "Thiết kế cổ điển, chống gió và giữ ấm tốt",
                        Price = 1450000,
                        Quantity = 70,
                        PicturePath = "adidas_jacket.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 2,
                        CateID = 1,   // Áo khoác
                        GenderID = 1  // Nam
                    },
                    new Products
                    {
                        ID = 8,
                        Name = "Quần jogger Adidas",
                        Description = "Quần thể thao thoải mái, phù hợp vận động",
                        Price = 850000,
                        Quantity = 90,
                        PicturePath = "adidas_pants.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 2,
                        CateID = 2,   // Quần
                        GenderID = 1  // Nam
                    },
                    new Products
                    {
                        ID = 9,
                        Name = "Giày Adidas Ultraboost",
                        Description = "Đệm Boost êm ái, hỗ trợ chạy bộ hiệu quả",
                        Price = 3200000,
                        Quantity = 50,
                        PicturePath = "adidas_shoes.webp",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 2,
                        CateID = 3,   // Giày
                        GenderID = 3  // Unisex
                    },
                    new Products
                    {
                        ID = 10,
                        Name = "Váy tennis Adidas nữ",
                        Description = "Váy thể thao nhẹ, thấm hút mồ hôi tốt",
                        Price = 990000,
                        Quantity = 45,
                        PicturePath = "adidas_dress.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 2,
                        CateID = 4,   // Váy
                        GenderID = 2  // Nữ
                    },
                    new Products
                    {
                        ID = 11,
                        Name = "Áo thun Adidas cổ tròn",
                        Description = "Áo thun cổ điển, logo 3 sọc đặc trưng",
                        Price = 650000,
                        Quantity = 150,
                        PicturePath = "adidas_tshirt.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 2,
                        CateID = 5,   // Áo thun
                        GenderID = 1  // Nam
                    },
                    new Products
                    {
                        ID = 12,
                        Name = "Đồng hồ thể thao Adidas",
                        Description = "Thiết kế năng động, chống nước tốt",
                        Price = 1750000,
                        Quantity = 30,
                        PicturePath = "adidas_watch.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 2,
                        CateID = 6,   // Đồng hồ
                        GenderID = 3  // Unisex
                    },
                    new Products
                    {
                        ID = 13,
                        Name = "Áo khoác Zara Dáng Dài",
                        Description = "Áo khoác nữ dáng dài thanh lịch",
                        Price = 1900000,
                        Quantity = 60,
                        PicturePath = "zara_jacket.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 3,
                        CateID = 1,   // Áo khoác
                        GenderID = 2  // Nữ
                    },
                    new Products
                    {
                        ID = 14,
                        Name = "Quần Ống Rộng Zara",
                        Description = "Phong cách hiện đại, thời trang",
                        Price = 980000,
                        Quantity = 40,
                        PicturePath = "zara_pants.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 3,
                        CateID = 2,   // Quần
                        GenderID = 2  // Nữ
                    },
                    new Products
                    {
                        ID = 15,
                        Name = "Giày Cao Gót Zara",
                        Description = "Thiết kế sang trọng, phù hợp tiệc tùng",
                        Price = 1200000,
                        Quantity = 55,
                        PicturePath = "zara_heels.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 3,
                        CateID = 3,   // Giày
                        GenderID = 2  // Nữ
                    },
                    new Products
                    {
                        ID = 16,
                        Name = "Váy Xếp Ly Zara",
                        Description = "Váy dài thướt tha, thanh lịch",
                        Price = 1250000,
                        Quantity = 35,
                        PicturePath = "zara_dress.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 3,
                        CateID = 4,   // Váy
                        GenderID = 2  // Nữ
                    },
                    new Products
                    {
                        ID = 17,
                        Name = "Áo Thun Zara Basic",
                        Description = "Áo thun đơn giản, dễ phối đồ",
                        Price = 450000,
                        Quantity = 90,
                        PicturePath = "zara_tshirt.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 3,
                        CateID = 5,   // Áo thun
                        GenderID = 3  // Unisex
                    },
                    new Products
                    {
                        ID = 18,
                        Name = "Đồng Hồ Kim Zara",
                        Description = "Thiết kế mặt tròn, dây da tinh tế",
                        Price = 1350000,
                        Quantity = 25,
                        PicturePath = "zara_watch.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 3,
                        CateID = 6,   // Đồng hồ
                        GenderID = 2  // Nữ
                    },
                    new Products
                    {
                        ID = 19,
                        Name = "Áo Khoác Dù H&M",
                        Description = "Chống gió, nhẹ và gọn",
                        Price = 950000,
                        Quantity = 80,
                        PicturePath = "hm_jacket.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 4,
                        CateID = 1,   // Áo khoác
                        GenderID = 1  // Nam
                    },
                    new Products
                    {
                        ID = 20,
                        Name = "Quần Tây H&M",
                        Description = "Phong cách công sở lịch sự",
                        Price = 700000,
                        Quantity = 65,
                        PicturePath = "hm_pants.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 4,
                        CateID = 2,   // Quần
                        GenderID = 1  // Nam
                    },
                    new Products
                    {
                        ID = 21,
                        Name = "Giày Thể Thao H&M",
                        Description = "Giày đơn giản, năng động",
                        Price = 820000,
                        Quantity = 100,
                        PicturePath = "hm_shoes.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 4,
                        CateID = 3,   // Giày
                        GenderID = 3  // Unisex
                    },
                    new Products
                    {
                        ID = 22,
                        Name = "Váy Ngắn Hoa Nhí H&M",
                        Description = "Váy xinh xắn cho mùa hè",
                        Price = 600000,
                        Quantity = 45,
                        PicturePath = "hm_dress.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 4,
                        CateID = 4,   // Váy
                        GenderID = 2  // Nữ
                    },
                    new Products
                    {
                        ID = 23,
                        Name = "Áo Thun Nam Basic H&M",
                        Description = "Áo thun trơn giá rẻ",
                        Price = 200000,
                        Quantity = 150,
                        PicturePath = "hm_tshirt.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 4,
                        CateID = 5,   // Áo thun
                        GenderID = 1  // Nam
                    },
                    new Products
                    {
                        ID = 24,
                        Name = "Đồng Hồ Dây Silicon H&M",
                        Description = "Thiết kế đơn giản, hiện đại",
                        Price = 550000,
                        Quantity = 40,
                        PicturePath = "hm_watch.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 4,
                        CateID = 6,   // Đồng hồ
                        GenderID = 3  // Unisex
                    },
                    new Products
                    {
                        ID = 25,
                        Name = "Áo khoác lông vũ Uniqlo",
                        Description = "Giữ ấm nhẹ nhàng, tiện lợi",
                        Price = 1750000,
                        Quantity = 70,
                        PicturePath = "uniqlo_jacket.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 5,
                        CateID = 1,   // Áo khoác
                        GenderID = 1  // Nam
                    },
                    new Products
                    {
                        ID = 26,
                        Name = "Quần Jean Uniqlo",
                        Description = "Jean co giãn, phong cách tối giản",
                        Price = 950000,
                        Quantity = 50,
                        PicturePath = "uniqlo_jeans.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 5,
                        CateID = 2,   // Quần
                        GenderID = 1  // Nam
                    },
                    new Products
                    {
                        ID = 27,
                        Name = "Giày Sneaker Uniqlo",
                        Description = "Thiết kế tối giản, thoải mái",
                        Price = 1050000,
                        Quantity = 60,
                        PicturePath = "uniqlo_shoes.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 5,
                        CateID = 3,   // Giày
                        GenderID = 1  // Nam
                    },
                    new Products
                    {
                        ID = 28,
                        Name = "Váy Cotton Uniqlo",
                        Description = "Thiết kế tối giản, thoải mái cho mùa hè",
                        Price = 850000,
                        Quantity = 40,
                        PicturePath = "uniqlo_dress.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 5,
                        CateID = 4,   // Váy
                        GenderID = 2  // Nữ
                    },
                    new Products
                    {
                        ID = 29,
                        Name = "Áo thun Airism Uniqlo",
                        Description = "Thoáng mát, nhanh khô, thích hợp mùa hè",
                        Price = 400000,
                        Quantity = 120,
                        PicturePath = "uniqlo_tshirt.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 5,
                        CateID = 5,   // Áo thun
                        GenderID = 3  // Unisex
                    },
                    new Products
                    {
                        ID = 30,
                        Name = "Đồng hồ tối giản Uniqlo",
                        Description = "Phong cách tối giản, dây da đen",
                        Price = 980000,
                        Quantity = 30,
                        PicturePath = "uniqlo_watch.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 5,
                        CateID = 6,   // Đồng hồ
                        GenderID = 1  // Nam
                    },
                    new Products
                    {
                        ID = 31,
                        Name = "Áo khoác cao cấp Gucci",
                        Description = "Thiết kế sang trọng, dành cho thời trang cao cấp",
                        Price = 25000000,
                        Quantity = 10,
                        PicturePath = "gucci_jacket.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 6,
                        CateID = 1,   // Áo khoác
                        GenderID = 2  // Nữ
                    },
                    new Products
                    {
                        ID = 32,
                        Name = "Quần tây Gucci nam",
                        Description = "Phong cách lịch lãm, chất liệu cao cấp",
                        Price = 15500000,
                        Quantity = 15,
                        PicturePath = "gucci_pants.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 6,
                        CateID = 2,   // Quần
                        GenderID = 1  // Nam
                    },
                    new Products
                    {
                        ID = 33,
                        Name = "Giày lười Gucci",
                        Description = "Thiết kế logo đặc trưng, đẳng cấp",
                        Price = 22000000,
                        Quantity = 12,
                        PicturePath = "gucci_shoes.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 6,
                        CateID = 3,   // Giày
                        GenderID = 1  // Nam
                    },
                    new Products
                    {
                        ID = 34,
                        Name = "Váy lụa cao cấp Gucci",
                        Description = "Chất liệu mềm mại, sang trọng",
                        Price = 30000000,
                        Quantity = 8,
                        PicturePath = "gucci_dress.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 6,
                        CateID = 4,   // Váy
                        GenderID = 2  // Nữ
                    },
                    new Products
                    {
                        ID = 35,
                        Name = "Áo thun Gucci logo lớn",
                        Description = "Phong cách trẻ trung, thời thượng",
                        Price = 9000000,
                        Quantity = 20,
                        PicturePath = "gucci_tshirt.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 6,
                        CateID = 5,   // Áo thun
                        GenderID = 3  // Unisex
                    },
                    new Products
                    {
                        ID = 36,
                        Name = "Đồng hồ đính đá Gucci",
                        Description = "Đồng hồ xa xỉ với thiết kế tinh xảo",
                        Price = 45000000,
                        Quantity = 5,
                        PicturePath = "gucci_watch.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 6,
                        CateID = 6,   // Đồng hồ
                        GenderID = 2  // Nữ
                    },
                    new Products
                    {
                        ID = 37,
                        Name = "Áo khoác jean Levi's",
                        Description = "Chất liệu jean bền, phong cách cổ điển",
                        Price = 1450000,
                        Quantity = 40,
                        PicturePath = "levis_jacket.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 7,
                        CateID = 1,   // Áo khoác
                        GenderID = 1  // Nam
                    },
                    new Products
                    {
                        ID = 38,
                        Name = "Quần jeans Levi's 501",
                        Description = "Form dáng chuẩn Mỹ, chất lượng cao",
                        Price = 1250000,
                        Quantity = 60,
                        PicturePath = "levis_jeans.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 7,
                        CateID = 2,   // Quần
                        GenderID = 1  // Nam
                    },
                    new Products
                    {
                        ID = 39,
                        Name = "Giày thể thao Levi's",
                        Description = "Thoải mái và thời trang hàng ngày",
                        Price = 1150000,
                        Quantity = 35,
                        PicturePath = "levis_shoes.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 7,
                        CateID = 3,   // Giày
                        GenderID = 1  // Nam
                    },
                    new Products
                    {
                        ID = 40,
                        Name = "Váy bò Levi's nữ",
                        Description = "Thiết kế trẻ trung, năng động",
                        Price = 980000,
                        Quantity = 20,
                        PicturePath = "levis_dress.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 7,
                        CateID = 4,   // Váy
                        GenderID = 2  // Nữ
                    },
                    new Products
                    {
                        ID = 41,
                        Name = "Áo thun cổ tròn Levi's",
                        Description = "Logo cổ điển, phong cách Mỹ",
                        Price = 500000,
                        Quantity = 100,
                        PicturePath = "levis_tshirt.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 7,
                        CateID = 5,   // Áo thun
                        GenderID = 3  // Unisex
                    },
                    new Products
                    {
                        ID = 42,
                        Name = "Đồng hồ dây da Levi's",
                        Description = "Đơn giản, mạnh mẽ, cá tính",
                        Price = 850000,
                        Quantity = 15,
                        PicturePath = "levis_watch.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 7,
                        CateID = 6,   // Đồng hồ
                        GenderID = 1  // Nam
                    },
                    new Products
                    {
                        ID = 43,
                        Name = "Áo khoác thể thao Lacoste",
                        Description = "Thiết kế năng động, thời trang",
                        Price = 2700000,
                        Quantity = 30,
                        PicturePath = "lacoste_jacket.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 8,
                        CateID = 1,   // Áivisex
                        GenderID = 3  // Nam
                    },
                    new Products
                    {
                        ID = 44,
                        Name = "Quần short Lacoste",
                        Description = "Phong cách thể thao, thoáng mát",
                        Price = 1450000,
                        Quantity = 40,
                        PicturePath = "lacoste_shorts.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 8,
                        CateID = 2,   // Quần
                        GenderID = 1  // Nam
                    },
                    new Products
                    {
                        ID = 45,
                        Name = "Giày sneaker Lacoste",
                        Description = "Chất liệu cao cấp, thiết kế tinh tế",
                        Price = 2200000,
                        Quantity = 25,
                        PicturePath = "lacoste_shoes.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 8,
                        CateID = 3,   // Giày
                        GenderID = 1  // Nam
                    },
                    new Products
                    {
                        ID = 46,
                        Name = "Váy polo Lacoste",
                        Description = "Lịch sự, sang trọng cho nữ",
                        Price = 1900000,
                        Quantity = 18,
                        PicturePath = "lacoste_dress.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 8,
                        CateID = 4,   // Váy
                        GenderID = 2  // Nữ
                    },
                    new Products
                    {
                        ID = 47,
                        Name = "Áo thun cá sấu Lacoste",
                        Description = "Biểu tượng nổi bật với logo cá sấu",
                        Price = 1100000,
                        Quantity = 55,
                        PicturePath = "lacoste_tshirt.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 8,
                        CateID = 5,   // Áo thun
                        GenderID = 1  // Nam
                    },
                    new Products
                    {
                        ID = 48,
                        Name = "Đồng hồ thể thao Lacoste",
                        Description = "Năng động, trẻ trung, phù hợp với người chơi thể thao",
                        Price = 2600000,
                        Quantity = 20,
                        PicturePath = "lacoste_watch.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 8,
                        CateID = 6,   // Đồng hồ
                        GenderID = 3  // Unisex
                    },
                    new Products
                    {
                        ID = 49,
                        Name = "Áo khoác thể thao Puma",
                        Description = "Thiết kế hiện đại, năng động",
                        Price = 1550000,
                        Quantity = 50,
                        PicturePath = "puma_jacket.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 9,
                        CateID = 1,   // Áo khoác
                        GenderID = 1  // Nam
                    },
                    new Products
                    {
                        ID = 50,
                        Name = "Quần jogger Puma",
                        Description = "Thoải mái, phù hợp vận động",
                        Price = 1100000,
                        Quantity = 70,
                        PicturePath = "puma_pants.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 9,
                        CateID = 2,   // Quần
                        GenderID = 1  // Nam
                    },
                    new Products
                    {
                        ID = 51,
                        Name = "Giày chạy bộ Puma",
                        Description = "Thiết kế thể thao, hỗ trợ di chuyển",
                        Price = 1800000,
                        Quantity = 40,
                        PicturePath = "puma_shoes.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 9,
                        CateID = 3,   // Giày
                        GenderID = 1  // Nam
                    },
                    new Products
                    {
                        ID = 52,
                        Name = "Váy thể thao Puma nữ",
                        Description = "Phù hợp tập luyện hoặc mặc thường ngày",
                        Price = 950000,
                        Quantity = 25,
                        PicturePath = "puma_dress.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 9,
                        CateID = 4,   // Váy
                        GenderID = 2  // Nữ
                    },
                    new Products
                    {
                        ID = 53,
                        Name = "Áo thun thể thao Puma",
                        Description = "Chất vải thấm hút, thoáng mát",
                        Price = 600000,
                        Quantity = 90,
                        PicturePath = "puma_tshirt.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 9,
                        CateID = 5,   // Áo thun
                        GenderID = 3  // Unisex
                    },
                    new Products
                    {
                        ID = 54,
                        Name = "Đồng hồ thể thao Puma",
                        Description = "Thiết kế mạnh mẽ, phong cách",
                        Price = 1200000,
                        Quantity = 18,
                        PicturePath = "puma_watch.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 9,
                        CateID = 6,   // Đồng hồ
                        GenderID = 1  // Nam
                    },
                    new Products
                    {
                        ID = 55,
                        Name = "Áo khoác dạ Chanel",
                        Description = "Đẳng cấp, thời thượng, thiết kế quý phái",
                        Price = 9500000,
                        Quantity = 15,
                        PicturePath = "chanel_jacket.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 10,
                        CateID = 1,   // Áo khoác
                        GenderID = 2  // Nữ
                    },
                    new Products
                    {
                        ID = 56,
                        Name = "Quần vải cao cấp Chanel",
                        Description = "Đường may tinh xảo, chất liệu mềm mịn",
                        Price = 7200000,
                        Quantity = 12,
                        PicturePath = "chanel_pants.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 10,
                        CateID = 2,   // Quần
                        GenderID = 2  // Nữ
                    },
                    new Products
                    {
                        ID = 57,
                        Name = "Giày cao gót Chanel",
                        Description = "Thanh lịch, sang trọng",
                        Price = 8300000,
                        Quantity = 10,
                        PicturePath = "chanel_heels.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 10,
                        CateID = 3,   // Giày
                        GenderID = 2  // Nữ
                    },
                    new Products
                    {
                        ID = 58,
                        Name = "Váy dạ hội Chanel",
                        Description = "Lộng lẫy, thiết kế quyến rũ",
                        Price = 13000000,
                        Quantity = 8,
                        PicturePath = "chanel_dress.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 10,
                        CateID = 4,   // Váy
                        GenderID = 2  // Nữ
                    },
                    new Products
                    {
                        ID = 59,
                        Name = "Áo thun cao cấp Chanel",
                        Description = "Đơn giản nhưng đầy tinh tế",
                        Price = 3200000,
                        Quantity = 20,
                        PicturePath = "chanel_tshirt.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 10,
                        CateID = 5,   // Áo thun
                        GenderID = 2  // Nữ
                    },
                    new Products
                    {
                        ID = 60,
                        Name = "Đồng hồ Chanel sang trọng",
                        Description = "Tinh xảo, sang trọng và nữ tính",
                        Price = 18000000,
                        Quantity = 6,
                        PicturePath = "chanel_watch.jpg",
                        Created = new DateTime(2025, 4, 18),
                        BrandID = 10,
                        CateID = 6,   // Đồng hồ
                        GenderID = 2  // Nữ
              
                    }
                };

            #endregion
            #region ProductSeason
            // Tạo Dictionary để ánh xạ ProductID sang CateID
            var productCateMap = products.ToDictionary(p => p.ID, p => p.CateID);

            // Tạo danh sách để seed ProductSeason
            var productSeasons = new List<ProductSeason>();

            for (int productId = 1; productId <= 60; productId++)
            {
                int cateId = productCateMap[productId];

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
                int cateID = productMap[productID].CateID;
                int brandId = productMap[productID].BrandID;

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
    }

}

