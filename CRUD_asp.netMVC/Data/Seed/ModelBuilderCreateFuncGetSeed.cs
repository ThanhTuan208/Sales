using CRUD_asp.netMVC.Models.Auth;
using CRUD_asp.netMVC.Models.Cart;
using CRUD_asp.netMVC.Models.Order;
using CRUD_asp.netMVC.Models.Payments;
using CRUD_asp.netMVC.Models.Product;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Tls;
using System.Drawing.Printing;
using System.Globalization;
using System.Text;

namespace CRUD_asp.netMVC.Data.Seed
{
    public static class ModelBuilderCreateFuncGetSeed
    {
        public static void RelationshipEntities(this ModelBuilder modelBuilder)
        {
            var users = modelBuilder.Entity<Users>();
            var orders = modelBuilder.Entity<Orders>();
            var carts = modelBuilder.Entity<AddToCart>();
            var reviews = modelBuilder.Entity<Reviews>();
            var payments = modelBuilder.Entity<Payment>();
            var products = modelBuilder.Entity<Products>();
            var productTags = modelBuilder.Entity<ProductTag>();
            var productSizes = modelBuilder.Entity<ProductSize>();
            var orderDetails = modelBuilder.Entity<OrderDetail>();
            var productQty = modelBuilder.Entity<ProductQuantity>();
            var productStyles = modelBuilder.Entity<ProductStyle>();
            var productColors = modelBuilder.Entity<ProductColors>();
            var productImages = modelBuilder.Entity<ProductImages>();
            var productSeasons = modelBuilder.Entity<ProductSeason>();
            var productMaterials = modelBuilder.Entity<ProductMaterial>();

            // Mockup du lieu mqh 1 - n    
            products.HasOne(m => m.Brands).WithMany(p => p.products).HasForeignKey(mi => mi.BrandID).OnDelete(DeleteBehavior.Cascade);
            products.HasOne(t => t.Cate).WithMany(p => p.products).HasForeignKey(ti => ti.CateID).OnDelete(DeleteBehavior.Cascade);
            products.HasOne(p => p.Gender).WithMany(p => p.Products).HasForeignKey(p => p.GenderID).OnDelete(DeleteBehavior.Cascade);
            products.HasOne(p => p.Featured).WithMany(p => p.products).HasForeignKey(p => p.FeaturedID).OnDelete(DeleteBehavior.Cascade);


            carts.HasOne(p => p.Product).WithMany(c => c.Carts).HasForeignKey(pi => pi.ProductID);
            carts.HasOne(u => u.Users).WithMany(c => c.Carts).HasForeignKey(ui => ui.UserID);

            orderDetails.HasOne(o => o.Orders).WithMany(od => od.OrderDetail).HasForeignKey(oi => oi.OrderID);
            orderDetails.HasOne(p => p.Product).WithMany(od => od.OrderDetails).HasForeignKey(pi => pi.ProductID);

            orders.HasOne(u => u.Users).WithMany(o => o.Orders).HasForeignKey(pi => pi.UserID).OnDelete(DeleteBehavior.NoAction);

            reviews.HasOne(r => r.Users).WithMany(r => r.Reviews).HasForeignKey(u => u.UserID);
            reviews.HasOne(r => r.Product).WithMany(r => r.Reviews).HasForeignKey(u => u.ProductID);

            productQty.HasKey(p => new { p.ProductID, p.SizeID, p.ColorID });
            productQty.HasOne(p => p.Product).WithMany(p => p.ProductQty).HasForeignKey(u => u.ProductID);
            productQty.HasOne(p => p.Size).WithMany(p => p.ProductQty).HasForeignKey(u => u.SizeID);
            productQty.HasOne(p => p.Color).WithMany(p => p.ProductQty).HasForeignKey(u => u.ColorID);

            // Mockup du lieu mqh 1 - 1
            payments.HasOne(o => o.Order).WithOne(p => p.Payment).HasForeignKey<Payment>(o => o.OrderID);
            users.HasOne(r => r.Customer).WithOne(u => u.Users).HasForeignKey<Customer>(mi => mi.UserID).OnDelete(DeleteBehavior.NoAction);
            users.HasOne(r => r.Manager).WithOne(u => u.Users).HasForeignKey<Manager>(mi => mi.UserID).OnDelete(DeleteBehavior.NoAction);

            // Mockup du lieu mqh n - n
            productSeasons.HasKey(p => new { p.SeasonID, p.ProductID });
            productSeasons.HasOne(p => p.Product).WithMany(p => p.ProductSeasons).HasForeignKey(p => p.ProductID);
            productSeasons.HasOne(p => p.Season).WithMany(p => p.ProductSeason).HasForeignKey(p => p.SeasonID);

            productTags.HasKey(p => new { p.TagID, p.ProductID });
            productTags.HasOne(p => p.Product).WithMany(p => p.ProductTags).HasForeignKey(p => p.ProductID);
            productTags.HasOne(p => p.Tag).WithMany(p => p.ProductTag).HasForeignKey(p => p.TagID);

            productStyles.HasKey(p => new { p.StyleID, p.ProductID });
            productStyles.HasOne(p => p.Product).WithMany(p => p.ProductStyles).HasForeignKey(p => p.ProductID);
            productStyles.HasOne(p => p.Style).WithMany(p => p.ProductStyles).HasForeignKey(p => p.StyleID);

            productSizes.HasKey(p => new { p.ProductID, p.SizeID });
            productSizes.HasOne(p => p.Products).WithMany(p => p.ProductSize).HasForeignKey(p => p.ProductID);
            productSizes.HasOne(p => p.Size).WithMany(p => p.ProductSize).HasForeignKey(p => p.SizeID);

            productColors.HasKey(p => new { p.ProductID, p.ColorID });
            productColors.HasOne(p => p.Product).WithMany(p => p.ProductColor).HasForeignKey(p => p.ProductID);
            productColors.HasOne(p => p.Color).WithMany(p => p.ProductColor).HasForeignKey(p => p.ColorID);

            productMaterials.HasKey(p => new { p.ProductID, p.MaterialID });
            productMaterials.HasOne(p => p.Product).WithMany(p => p.ProductMaterial).HasForeignKey(p => p.ProductID);
            productMaterials.HasOne(p => p.Material).WithMany(p => p.ProductMaterial).HasForeignKey(p => p.MaterialID);

            productImages.HasOne(p => p.Products).WithMany(p => p.ProductImages).HasForeignKey(p => p.ProductID);
        }

        // Them indexing site user entity, rang buoc thuoc tinh Date
        public static void IndexPropertySiteUser(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SiteUsers>().HasIndex(p => p.Date).IsUnique();
            modelBuilder.Entity<SiteUsers>(p =>
            {
                p.Property<DateOnly>("Day").HasComputedColumnSql("CONVERT(DATE, [Date]) PERSISTED")
                                                    .ValueGeneratedOnAddOrUpdate();

                p.HasIndex("Day").HasDatabaseName("IX_SiteUsers_Day");
            });
        }

        // Them computed column, indexing entity Payment
        public static void IndexPropertyPayment(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payment>(p =>
            {
                p.Property<DateOnly>("StartDay").HasComputedColumnSql("CONVERT(DATE, [PaymentDate]) PERSISTED")
                                                .ValueGeneratedOnAddOrUpdate();

                p.HasIndex("StartDay").HasDatabaseName("IX_Payment_PaymentByDay");

                p.Property<int>("StartMonth").HasComputedColumnSql("DATEPART(MONTH, [PaymentDate]) PERSISTED")
                                               .ValueGeneratedOnAddOrUpdate();

                p.Property<int>("StartYear").HasComputedColumnSql("DATEPART(YEAR, [PaymentDate]) PERSISTED")
                                               .ValueGeneratedOnAddOrUpdate();

                p.HasIndex("StartMonth", "StartYear").HasDatabaseName("IX_Payment_PaymentByMonth");
            });
        }

        // Them computed column, indexing entity Users
        public static void IndexPropertyUser(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>(p =>
            {
                //p.Property<int>("StartYear").HasComputedColumnSql("DATEPART(YEAR, [StartDate]) PERSISTED")
                //                                .ValueGeneratedOnAddOrUpdate();

                //p.HasIndex("StartYear").HasDatabaseName("IX_Users_UserByYear");

                //p.Property<int>("StartQuarter").HasComputedColumnSql("DATEPART(QUARTER, [StartDate]) PERSISTED")
                //                                .ValueGeneratedOnAddOrUpdate();

                //p.HasIndex("StartQuarter", "StartYear").HasDatabaseName("IX_Users_UserByQuarter");

                p.Property<DateOnly>("StartDay").HasComputedColumnSql("CONVERT(DATE, [StartDate]) PERSISTED")
                                                .ValueGeneratedOnAddOrUpdate();

                p.HasIndex("StartDay").HasDatabaseName("IX_Users_UserByDay");
            });
        }

        public static void SeedUsers(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>().HasData(
                new Users
                {
                    Id = 8,
                    FirstName = "Nguyễn",
                    LastName = "Tuấn",
                    ProfileImage = "images/avatar/2bc6c1a6-74ab-43e3-9c14-7b900fc59112.jpg",
                    StartDate = new DateTime(2025, 7, 25, 12, 6, 38),
                    DateOfBirth = new DateTime(1, 1, 30),
                    Gender = "Nam",
                    RoleId = 2,
                    UserName = "userdtrai",
                    NormalizedUserName = "USERDTRAI",
                    Email = "user_demo@gmail.com",
                    NormalizedEmail = "USER_DEMO@GMAIL.COM",
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAIAAYagAAAAEIGrep56jZMETG3pvlfvY8ZuqfD8kcKSnkL2LdTVXMmuouD38WnwdkAp5z3lqzIqCg==",
                    SecurityStamp = "T6GUP2BYL5M7Y63HDKWNDRF2TRBJ3VHW",
                    ConcurrencyStamp = "87fa214c-77b7-49b6-969d-b05cd4b8a85a",
                    PhoneNumber = "0358986824",
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = false,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    LockoutEnd = new DateTime(2025, 7, 25)
                },
                new Users
                {
                    Id = 10,
                    FirstName = "Nguyễn",
                    LastName = "Tuấn",
                    ProfileImage = "images/avatar/2bc6c1a6-74ab-43e3-9c14-7b900fc59112.jpg",
                    StartDate = new DateTime(2025, 11, 26, 19, 44, 21),
                    DateOfBirth = new DateTime(2005, 8, 20),
                    Gender = "Nam",
                    RoleId = 1,
                    UserName = "admindtrai",
                    NormalizedUserName = "ADMINDTRAI",
                    Email = "admin_demo@gmail.com",
                    NormalizedEmail = "ADMIN_DEMO@GMAIL.COM",
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAIAAYagAAAAEET3xX8a043bIf6s88V8JHayJR91oPDtwcbSwBu4DNGtnr4err11wI/IwyJSUI/l0Q==",
                    SecurityStamp = "4SKEY4LFJ5YFEM5REPISLB4YBHVSTUR3",
                    ConcurrencyStamp = "6eef443c-bba9-4122-a821-920e06361f5a",
                    PhoneNumber = "0358986823",
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = false,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    LockoutEnd = new DateTime(2025, 11, 26)
                }
            );
        }

        public static void SeedUserRoles(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserRole<int>>().HasData(
                new IdentityUserRole<int>
                {
                    UserId = 10, // admin
                    RoleId = 1   // Admin
                },
                new IdentityUserRole<int>
                {
                    UserId = 8,  // user
                    RoleId = 2   // Customer
                }
            );
        }

        // Them du lieu cho color, size, quantity cua san pham
        public static void SeedProductQty(this ModelBuilder modelBuilder)
        {
            int colorCount = 8;
            int sizeCount = 14;
            int productCount = 60;
            var productQuantities = new List<ProductQuantity>();

            Random rnd = new Random(12345);

            for (int productId = 1; productId <= productCount; productId++)
            {
                // Chọn random 3–6 màu
                var selectedColors = Enumerable.Range(1, colorCount)
                    .OrderBy(x => rnd.Next())
                    .Take(rnd.Next(3, 7))
                    .ToList();

                // Chọn random 2–5 size
                var selectedSizes = Enumerable.Range(1, sizeCount)
                    .OrderBy(x => rnd.Next())
                    .Take(rnd.Next(2, 6))
                    .ToList();

                // Tạo combination color × size
                foreach (var colorId in selectedColors)
                {
                    foreach (var sizeId in selectedSizes)
                    {
                        productQuantities.Add(new ProductQuantity
                        {
                            ProductID = productId,
                            ColorID = colorId,
                            SizeID = sizeId,
                            Quantity = rnd.Next(5, 50)
                        });
                    }
                }
            }

            modelBuilder.Entity<ProductQuantity>().HasData(productQuantities);
        }


        // Them du lieu hinh anh cho san pham
        public static void SeedProductImage(this ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ProductImages>().HasData(
            // Nike
            new ProductImages { ID = 1, PathNameImage = "nike_jacket_1.1.jpg", ProductID = 1 },
            new ProductImages { ID = 2, PathNameImage = "nike_jacket_1.2.jpg", ProductID = 1 },
            new ProductImages { ID = 3, PathNameImage = "nike_jacket_1.3.jpg", ProductID = 1 },
            new ProductImages { ID = 4, PathNameImage = "nike_jacket_1.4.jpg", ProductID = 1 },
            new ProductImages { ID = 5, PathNameImage = "nike_pants_1.1.jpg", ProductID = 2 },
            new ProductImages { ID = 6, PathNameImage = "nike_pants_1.2.jpg", ProductID = 2 },
            new ProductImages { ID = 7, PathNameImage = "nike_pants_1.3.jpg", ProductID = 2 },
            new ProductImages { ID = 8, PathNameImage = "nike_pants_1.4.jpg", ProductID = 2 },
            new ProductImages { ID = 9, PathNameImage = "nike_shoes_1.1.jpg", ProductID = 3 },
            new ProductImages { ID = 10, PathNameImage = "nike_shoes_1.2.jpg", ProductID = 3 },
            new ProductImages { ID = 11, PathNameImage = "nike_shoes_1.3.jpg", ProductID = 3 },
            new ProductImages { ID = 12, PathNameImage = "nike_shoes_1.4.jpg", ProductID = 3 },
            new ProductImages { ID = 13, PathNameImage = "nike_dress_1.1.jpg", ProductID = 4 },
            new ProductImages { ID = 14, PathNameImage = "nike_dress_1.2.jpg", ProductID = 4 },
            new ProductImages { ID = 15, PathNameImage = "nike_dress_1.3.jpg", ProductID = 4 },
            new ProductImages { ID = 16, PathNameImage = "nike_dress_1.4.jpg", ProductID = 4 },
            new ProductImages { ID = 17, PathNameImage = "nike_tshirt_1.1.jpg", ProductID = 5 },
            new ProductImages { ID = 18, PathNameImage = "nike_tshirt_1.2.jpg", ProductID = 5 },
            new ProductImages { ID = 19, PathNameImage = "nike_tshirt_1.3.jpg", ProductID = 5 },
            new ProductImages { ID = 20, PathNameImage = "nike_tshirt_1.4.jpg", ProductID = 5 },
            new ProductImages { ID = 21, PathNameImage = "nike_watch_1.1.jpg", ProductID = 6 },
            new ProductImages { ID = 22, PathNameImage = "nike_watch_1.2.jpg", ProductID = 6 },

            // Adidas
            new ProductImages { ID = 25, PathNameImage = "adidas_jacket_1.1.jpg", ProductID = 7 },
            new ProductImages { ID = 26, PathNameImage = "adidas_jacket_1.2.jpg", ProductID = 7 },
            new ProductImages { ID = 27, PathNameImage = "adidas_jacket_1.3.jpg", ProductID = 7 },
            new ProductImages { ID = 28, PathNameImage = "adidas_jacket_1.4.jpg", ProductID = 7 },
            new ProductImages { ID = 29, PathNameImage = "adidas_pants_1.1.jpg", ProductID = 8 },
            new ProductImages { ID = 30, PathNameImage = "adidas_pants_1.2.jpg", ProductID = 8 },
            new ProductImages { ID = 31, PathNameImage = "adidas_pants_1.3.jpg", ProductID = 8 },
            new ProductImages { ID = 32, PathNameImage = "adidas_pants_1.4.jpg", ProductID = 8 },
            new ProductImages { ID = 33, PathNameImage = "adidas_shoes_1.1.jpg", ProductID = 9 },
            new ProductImages { ID = 34, PathNameImage = "adidas_shoes_1.2.jpg", ProductID = 9 },
            new ProductImages { ID = 35, PathNameImage = "adidas_shoes_1.3.jpg", ProductID = 9 },
            new ProductImages { ID = 36, PathNameImage = "adidas_shoes_1.4.jpg", ProductID = 9 },
            new ProductImages { ID = 37, PathNameImage = "adidas_dress_1.1.jpg", ProductID = 10 },
            new ProductImages { ID = 38, PathNameImage = "adidas_dress_1.2.jpg", ProductID = 10 },
            new ProductImages { ID = 39, PathNameImage = "adidas_dress_1.3.jpg", ProductID = 10 },
            new ProductImages { ID = 40, PathNameImage = "adidas_dress_1.4.jpg", ProductID = 10 },
            new ProductImages { ID = 41, PathNameImage = "adidas_tshirt_1.1.jpg", ProductID = 11 },
            new ProductImages { ID = 42, PathNameImage = "adidas_tshirt_1.2.jpg", ProductID = 11 },
            new ProductImages { ID = 43, PathNameImage = "adidas_tshirt_1.3.jpg", ProductID = 11 },
            new ProductImages { ID = 44, PathNameImage = "adidas_tshirt_1.4.jpg", ProductID = 11 },
            new ProductImages { ID = 45, PathNameImage = "adidas_watch_1.1.jpg", ProductID = 12 },
            new ProductImages { ID = 46, PathNameImage = "adidas_watch_1.2.jpg", ProductID = 12 },
            new ProductImages { ID = 47, PathNameImage = "adidas_watch_1.3.jpg", ProductID = 12 },
            new ProductImages { ID = 48, PathNameImage = "adidas_watch_1.4.jpg", ProductID = 12 },

            // Zara
            new ProductImages { ID = 49, PathNameImage = "zara_jacket_1.1.jpg", ProductID = 13 },
            new ProductImages { ID = 50, PathNameImage = "zara_jacket_1.2.jpg", ProductID = 13 },
            new ProductImages { ID = 51, PathNameImage = "zara_jacket_1.3.jpg", ProductID = 13 },
            new ProductImages { ID = 52, PathNameImage = "zara_jacket_1.4.jpg", ProductID = 13 },
            new ProductImages { ID = 53, PathNameImage = "zara_pants_1.1.jpg", ProductID = 14 },
            new ProductImages { ID = 54, PathNameImage = "zara_pants_1.2.jpg", ProductID = 14 },
            new ProductImages { ID = 55, PathNameImage = "zara_pants_1.3.jpg", ProductID = 14 },
            new ProductImages { ID = 56, PathNameImage = "zara_pants_1.4.jpg", ProductID = 14 },
            new ProductImages { ID = 57, PathNameImage = "zara_shoes_1.1.jpg", ProductID = 15 },
            new ProductImages { ID = 58, PathNameImage = "zara_shoes_1.2.jpg", ProductID = 15 },
            new ProductImages { ID = 59, PathNameImage = "zara_shoes_1.3.jpg", ProductID = 15 },
            new ProductImages { ID = 60, PathNameImage = "zara_shoes_1.4.jpg", ProductID = 15 },
            new ProductImages { ID = 61, PathNameImage = "zara_dress_1.1.jpg", ProductID = 16 },
            new ProductImages { ID = 62, PathNameImage = "zara_dress_1.2.jpg", ProductID = 16 },
            new ProductImages { ID = 63, PathNameImage = "zara_dress_1.3.jpg", ProductID = 16 },
            new ProductImages { ID = 64, PathNameImage = "zara_dress_1.4.jpg", ProductID = 16 },
            new ProductImages { ID = 65, PathNameImage = "zara_tshirt_1.1.jpg", ProductID = 17 },
            new ProductImages { ID = 66, PathNameImage = "zara_tshirt_1.2.jpg", ProductID = 17 },
            new ProductImages { ID = 67, PathNameImage = "zara_tshirt_1.3.jpg", ProductID = 17 },
            new ProductImages { ID = 68, PathNameImage = "zara_tshirt_1.4.jpg", ProductID = 17 },
            new ProductImages { ID = 69, PathNameImage = "zara_watch_1.1.jpg", ProductID = 18 },
            new ProductImages { ID = 70, PathNameImage = "zara_watch_1.2.jpg", ProductID = 18 },

            // H&M
            new ProductImages { ID = 73, PathNameImage = "h&m_jacket_1.1.jpg", ProductID = 19 },
            new ProductImages { ID = 74, PathNameImage = "h&m_jacket_1.2.jpg", ProductID = 19 },
            new ProductImages { ID = 75, PathNameImage = "h&m_jacket_1.3.jpg", ProductID = 19 },
            new ProductImages { ID = 76, PathNameImage = "h&m_jacket_1.4.jpg", ProductID = 19 },
            new ProductImages { ID = 77, PathNameImage = "h&m_pants_1.1.jpg", ProductID = 20 },
            new ProductImages { ID = 78, PathNameImage = "h&m_pants_1.2.jpg", ProductID = 20 },
            new ProductImages { ID = 79, PathNameImage = "h&m_pants_1.3.jpg", ProductID = 20 },
            new ProductImages { ID = 80, PathNameImage = "h&m_pants_1.4.jpg", ProductID = 20 },
            new ProductImages { ID = 81, PathNameImage = "h&m_shoes_1.1.jpg", ProductID = 21 },
            new ProductImages { ID = 82, PathNameImage = "h&m_shoes_1.2.jpg", ProductID = 21 },
            new ProductImages { ID = 83, PathNameImage = "h&m_shoes_1.3.jpg", ProductID = 21 },
            new ProductImages { ID = 84, PathNameImage = "h&m_shoes_1.4.jpg", ProductID = 21 },
            new ProductImages { ID = 85, PathNameImage = "h&m_dress_1.1.jpg", ProductID = 22 },
            new ProductImages { ID = 86, PathNameImage = "h&m_dress_1.2.jpg", ProductID = 22 },
            new ProductImages { ID = 87, PathNameImage = "h&m_dress_1.3.jpg", ProductID = 22 },
            new ProductImages { ID = 88, PathNameImage = "h&m_dress_1.4.jpg", ProductID = 22 },
            new ProductImages { ID = 89, PathNameImage = "h&m_tshirt_1.1.jpg", ProductID = 23 },
            new ProductImages { ID = 90, PathNameImage = "h&m_tshirt_1.2.jpg", ProductID = 23 },
            new ProductImages { ID = 91, PathNameImage = "h&m_tshirt_1.3.jpg", ProductID = 23 },
            new ProductImages { ID = 92, PathNameImage = "h&m_tshirt_1.4.jpg", ProductID = 23 },
            new ProductImages { ID = 93, PathNameImage = "h&m_watch_1.1.jpg", ProductID = 24 },
            new ProductImages { ID = 94, PathNameImage = "h&m_watch_1.2.jpg", ProductID = 24 },
            new ProductImages { ID = 95, PathNameImage = "h&m_watch_1.3.jpg", ProductID = 24 },

            // Uniqlo
            new ProductImages { ID = 97, PathNameImage = "uniqlo_jacket_1.1.jpg", ProductID = 25 },
            new ProductImages { ID = 98, PathNameImage = "uniqlo_jacket_1.2.jpg", ProductID = 25 },
            new ProductImages { ID = 99, PathNameImage = "uniqlo_jacket_1.3.jpg", ProductID = 25 },
            new ProductImages { ID = 100, PathNameImage = "uniqlo_jacket_1.4.jpg", ProductID = 25 },
            new ProductImages { ID = 101, PathNameImage = "uniqlo_pants_1.1.jpg", ProductID = 26 },
            new ProductImages { ID = 102, PathNameImage = "uniqlo_pants_1.2.jpg", ProductID = 26 },
            new ProductImages { ID = 103, PathNameImage = "uniqlo_pants_1.3.jpg", ProductID = 26 },
            new ProductImages { ID = 104, PathNameImage = "uniqlo_pants_1.4.jpg", ProductID = 26 },
            new ProductImages { ID = 105, PathNameImage = "uniqlo_shoes_1.1.jpg", ProductID = 27 },
            new ProductImages { ID = 106, PathNameImage = "uniqlo_shoes_1.2.jpg", ProductID = 27 },
            new ProductImages { ID = 107, PathNameImage = "uniqlo_shoes_1.3.jpg", ProductID = 27 },
            new ProductImages { ID = 108, PathNameImage = "uniqlo_shoes_1.4.jpg", ProductID = 27 },
            new ProductImages { ID = 109, PathNameImage = "uniqlo_dress_1.1.jpg", ProductID = 28 },
            new ProductImages { ID = 110, PathNameImage = "uniqlo_dress_1.2.jpg", ProductID = 28 },
            new ProductImages { ID = 111, PathNameImage = "uniqlo_dress_1.3.jpg", ProductID = 28 },
            new ProductImages { ID = 112, PathNameImage = "uniqlo_dress_1.4.jpg", ProductID = 28 },
            new ProductImages { ID = 113, PathNameImage = "uniqlo_tshirt_1.1.jpg", ProductID = 29 },
            new ProductImages { ID = 114, PathNameImage = "uniqlo_tshirt_1.2.jpg", ProductID = 29 },
            new ProductImages { ID = 115, PathNameImage = "uniqlo_tshirt_1.3.jpg", ProductID = 29 },
            new ProductImages { ID = 116, PathNameImage = "uniqlo_tshirt_1.4.jpg", ProductID = 29 },
            new ProductImages { ID = 117, PathNameImage = "uniqlo_watch_1.1.jpg", ProductID = 30 },
            new ProductImages { ID = 118, PathNameImage = "uniqlo_watch_1.2.jpg", ProductID = 30 },
            new ProductImages { ID = 119, PathNameImage = "uniqlo_watch_1.3.jpg", ProductID = 30 },
            //new ProductImages { ID = 120, PathNameImage = "uniqlo_watch_1.4.jpg", ProductID = 30 },

            // Gucci
            new ProductImages { ID = 121, PathNameImage = "gucci_jacket_1.1.jpg", ProductID = 31 },
            new ProductImages { ID = 122, PathNameImage = "gucci_jacket_1.2.jpg", ProductID = 31 },
            new ProductImages { ID = 123, PathNameImage = "gucci_jacket_1.3.jpg", ProductID = 31 },
            new ProductImages { ID = 124, PathNameImage = "gucci_jacket_1.4.jpg", ProductID = 31 },

            new ProductImages { ID = 125, PathNameImage = "gucci_pants_1.1.jpg", ProductID = 32 },
            new ProductImages { ID = 126, PathNameImage = "gucci_pants_1.2.jpg", ProductID = 32 },
            new ProductImages { ID = 127, PathNameImage = "gucci_pants_1.3.jpg", ProductID = 32 },
            new ProductImages { ID = 128, PathNameImage = "gucci_pants_1.4.jpg", ProductID = 32 },

            new ProductImages { ID = 129, PathNameImage = "gucci_shoes_1.1.jpg", ProductID = 33 },
            new ProductImages { ID = 130, PathNameImage = "gucci_shoes_1.2.jpg", ProductID = 33 },
            new ProductImages { ID = 131, PathNameImage = "gucci_shoes_1.3.jpg", ProductID = 33 },
            new ProductImages { ID = 132, PathNameImage = "gucci_shoes_1.4.jpg", ProductID = 33 },

            new ProductImages { ID = 133, PathNameImage = "gucci_dress_1.1.jpg", ProductID = 34 },
            new ProductImages { ID = 134, PathNameImage = "gucci_dress_1.2.jpg", ProductID = 34 },
            new ProductImages { ID = 135, PathNameImage = "gucci_dress_1.3.jpg", ProductID = 34 },
            new ProductImages { ID = 136, PathNameImage = "gucci_dress_1.4.jpg", ProductID = 34 },

            new ProductImages { ID = 137, PathNameImage = "gucci_tshirt_1.1.jpg", ProductID = 35 },
            new ProductImages { ID = 138, PathNameImage = "gucci_tshirt_1.2.jpg", ProductID = 35 },
            new ProductImages { ID = 139, PathNameImage = "gucci_tshirt_1.3.jpg", ProductID = 35 },
            new ProductImages { ID = 140, PathNameImage = "gucci_tshirt_1.4.jpg", ProductID = 35 },

            new ProductImages { ID = 141, PathNameImage = "gucci_watch_1.1.jpg", ProductID = 36 },
            new ProductImages { ID = 142, PathNameImage = "gucci_watch_1.2.jpg", ProductID = 36 },

            // Levi's
            new ProductImages { ID = 143, PathNameImage = "levis_jacket_1.1.jpg", ProductID = 37 },
            new ProductImages { ID = 144, PathNameImage = "levis_jacket_1.2.jpg", ProductID = 37 },
            new ProductImages { ID = 145, PathNameImage = "levis_jacket_1.3.jpg", ProductID = 37 },
            new ProductImages { ID = 146, PathNameImage = "levis_jacket_1.4.jpg", ProductID = 37 },

            new ProductImages { ID = 147, PathNameImage = "levis_pants_1.1.jpg", ProductID = 38 },
            new ProductImages { ID = 148, PathNameImage = "levis_pants_1.2.jpg", ProductID = 38 },
            new ProductImages { ID = 149, PathNameImage = "levis_pants_1.3.jpg", ProductID = 38 },
            new ProductImages { ID = 150, PathNameImage = "levis_pants_1.4.jpg", ProductID = 38 },

            new ProductImages { ID = 151, PathNameImage = "levis_shoes_1.1.jpg", ProductID = 39 },
            new ProductImages { ID = 152, PathNameImage = "levis_shoes_1.2.jpg", ProductID = 39 },
            new ProductImages { ID = 153, PathNameImage = "levis_shoes_1.3.jpg", ProductID = 39 },
            new ProductImages { ID = 154, PathNameImage = "levis_shoes_1.4.jpg", ProductID = 39 },

            new ProductImages { ID = 155, PathNameImage = "levis_dress_1.1.jpg", ProductID = 40 },
            new ProductImages { ID = 156, PathNameImage = "levis_dress_1.2.jpg", ProductID = 40 },
            new ProductImages { ID = 157, PathNameImage = "levis_dress_1.3.jpg", ProductID = 40 },
            new ProductImages { ID = 158, PathNameImage = "levis_dress_1.4.jpg", ProductID = 40 },

            new ProductImages { ID = 159, PathNameImage = "levis_tshirt_1.1.jpg", ProductID = 41 },
            new ProductImages { ID = 160, PathNameImage = "levis_tshirt_1.2.jpg", ProductID = 41 },
            new ProductImages { ID = 161, PathNameImage = "levis_tshirt_1.3.jpg", ProductID = 41 },
            new ProductImages { ID = 162, PathNameImage = "levis_tshirt_1.4.jpg", ProductID = 41 },

            new ProductImages { ID = 163, PathNameImage = "levis_watch_1.1.jpg", ProductID = 42 },
            new ProductImages { ID = 164, PathNameImage = "levis_watch_1.2.jpg", ProductID = 42 },
            new ProductImages { ID = 165, PathNameImage = "levis_watch_1.3.jpg", ProductID = 42 },

            // Lacoste
            new ProductImages { ID = 166, PathNameImage = "lacoste_jacket_1.1.jpg", ProductID = 43 },
            new ProductImages { ID = 167, PathNameImage = "lacoste_jacket_1.2.jpg", ProductID = 43 },
            new ProductImages { ID = 168, PathNameImage = "lacoste_jacket_1.3.jpg", ProductID = 43 },
            new ProductImages { ID = 169, PathNameImage = "lacoste_jacket_1.4.jpg", ProductID = 43 },

            new ProductImages { ID = 170, PathNameImage = "lacoste_pants_1.1.jpg", ProductID = 44 },
            new ProductImages { ID = 171, PathNameImage = "lacoste_pants_1.2.jpg", ProductID = 44 },
            new ProductImages { ID = 172, PathNameImage = "lacoste_pants_1.3.jpg", ProductID = 44 },
            new ProductImages { ID = 173, PathNameImage = "lacoste_pants_1.4.jpg", ProductID = 44 },

            new ProductImages { ID = 174, PathNameImage = "lacoste_shoes_1.1.jpg", ProductID = 45 },
            new ProductImages { ID = 175, PathNameImage = "lacoste_shoes_1.2.jpg", ProductID = 45 },
            new ProductImages { ID = 176, PathNameImage = "lacoste_shoes_1.3.jpg", ProductID = 45 },
            new ProductImages { ID = 177, PathNameImage = "lacoste_shoes_1.4.jpg", ProductID = 45 },

            new ProductImages { ID = 178, PathNameImage = "lacoste_dress_1.1.jpg", ProductID = 46 },
            new ProductImages { ID = 179, PathNameImage = "lacoste_dress_1.2.jpg", ProductID = 46 },
            new ProductImages { ID = 180, PathNameImage = "lacoste_dress_1.3.jpg", ProductID = 46 },
            new ProductImages { ID = 181, PathNameImage = "lacoste_dress_1.4.jpg", ProductID = 46 },

            new ProductImages { ID = 182, PathNameImage = "lacoste_tshirt_1.1.jpg", ProductID = 47 },
            new ProductImages { ID = 183, PathNameImage = "lacoste_tshirt_1.2.jpg", ProductID = 47 },
            new ProductImages { ID = 184, PathNameImage = "lacoste_tshirt_1.3.jpg", ProductID = 47 },
            new ProductImages { ID = 185, PathNameImage = "lacoste_tshirt_1.4.jpg", ProductID = 47 },

            new ProductImages { ID = 186, PathNameImage = "lacoste_watch_1.1.jpg", ProductID = 48 },
            new ProductImages { ID = 187, PathNameImage = "lacoste_watch_1.2.jpg", ProductID = 48 },
            new ProductImages { ID = 188, PathNameImage = "lacoste_watch_1.3.jpg", ProductID = 48 },

            // Puma
            new ProductImages { ID = 189, PathNameImage = "puma_jacket_1.1.jpg", ProductID = 49 },
            new ProductImages { ID = 190, PathNameImage = "puma_jacket_1.2.jpg", ProductID = 49 },
            new ProductImages { ID = 191, PathNameImage = "puma_jacket_1.3.jpg", ProductID = 49 },
            new ProductImages { ID = 192, PathNameImage = "puma_jacket_1.4.jpg", ProductID = 49 },

            new ProductImages { ID = 193, PathNameImage = "puma_pants_1.1.jpg", ProductID = 50 },
            new ProductImages { ID = 194, PathNameImage = "puma_pants_1.2.jpg", ProductID = 50 },
            new ProductImages { ID = 195, PathNameImage = "puma_pants_1.3.jpg", ProductID = 50 },
            new ProductImages { ID = 196, PathNameImage = "puma_pants_1.4.jpg", ProductID = 50 },

            new ProductImages { ID = 197, PathNameImage = "puma_shoes_1.1.jpg", ProductID = 51 },
            new ProductImages { ID = 198, PathNameImage = "puma_shoes_1.2.jpg", ProductID = 51 },
            new ProductImages { ID = 199, PathNameImage = "puma_shoes_1.3.jpg", ProductID = 51 },
            new ProductImages { ID = 200, PathNameImage = "puma_shoes_1.4.jpg", ProductID = 51 },

            new ProductImages { ID = 201, PathNameImage = "puma_dress_1.1.jpg", ProductID = 52 },
            new ProductImages { ID = 202, PathNameImage = "puma_dress_1.2.jpg", ProductID = 52 },
            new ProductImages { ID = 203, PathNameImage = "puma_dress_1.3.jpg", ProductID = 52 },
            new ProductImages { ID = 204, PathNameImage = "puma_dress_1.4.jpg", ProductID = 52 },

            new ProductImages { ID = 205, PathNameImage = "puma_tshirt_1.1.jpg", ProductID = 53 },
            new ProductImages { ID = 206, PathNameImage = "puma_tshirt_1.2.jpg", ProductID = 53 },
            new ProductImages { ID = 207, PathNameImage = "puma_tshirt_1.3.jpg", ProductID = 53 },
            new ProductImages { ID = 208, PathNameImage = "puma_tshirt_1.4.jpg", ProductID = 53 },

            new ProductImages { ID = 209, PathNameImage = "puma_watch_1.1.jpg", ProductID = 54 },
            new ProductImages { ID = 210, PathNameImage = "puma_watch_1.2.jpg", ProductID = 54 },
            new ProductImages { ID = 211, PathNameImage = "puma_watch_1.3.jpg", ProductID = 54 },

            // Chanel
            new ProductImages { ID = 212, PathNameImage = "chanel_jacket_1.1.jpg", ProductID = 55 },
            new ProductImages { ID = 213, PathNameImage = "chanel_jacket_1.2.jpg", ProductID = 55 },
            new ProductImages { ID = 214, PathNameImage = "chanel_jacket_1.3.jpg", ProductID = 55 },
            new ProductImages { ID = 215, PathNameImage = "chanel_jacket_1.4.jpg", ProductID = 55 },

            new ProductImages { ID = 216, PathNameImage = "chanel_pants_1.1.jpg", ProductID = 56 },
            new ProductImages { ID = 217, PathNameImage = "chanel_pants_1.2.jpg", ProductID = 56 },
            new ProductImages { ID = 218, PathNameImage = "chanel_pants_1.3.jpg", ProductID = 56 },
            new ProductImages { ID = 219, PathNameImage = "chanel_pants_1.4.jpg", ProductID = 56 },

            new ProductImages { ID = 220, PathNameImage = "chanel_shoes_1.1.jpg", ProductID = 57 },
            new ProductImages { ID = 221, PathNameImage = "chanel_shoes_1.2.jpg", ProductID = 57 },
            new ProductImages { ID = 222, PathNameImage = "chanel_shoes_1.3.jpg", ProductID = 57 },
            new ProductImages { ID = 223, PathNameImage = "chanel_shoes_1.4.jpg", ProductID = 57 },

            new ProductImages { ID = 224, PathNameImage = "chanel_dress_1.1.jpg", ProductID = 58 },
            new ProductImages { ID = 225, PathNameImage = "chanel_dress_1.2.jpg", ProductID = 58 },
            new ProductImages { ID = 226, PathNameImage = "chanel_dress_1.3.jpg", ProductID = 58 },
            new ProductImages { ID = 227, PathNameImage = "chanel_dress_1.4.jpg", ProductID = 58 },

            new ProductImages { ID = 228, PathNameImage = "chanel_tshirt_1.1.jpg", ProductID = 59 },
            new ProductImages { ID = 229, PathNameImage = "chanel_tshirt_1.2.jpg", ProductID = 59 },
            new ProductImages { ID = 230, PathNameImage = "chanel_tshirt_1.3.jpg", ProductID = 59 },
            new ProductImages { ID = 231, PathNameImage = "chanel_tshirt_1.4.jpg", ProductID = 59 },

            new ProductImages { ID = 232, PathNameImage = "chanel_watch_1.1.jpg", ProductID = 60 },
            new ProductImages { ID = 233, PathNameImage = "chanel_watch_1.2.jpg", ProductID = 60 },
            new ProductImages { ID = 234, PathNameImage = "chanel_watch_1.3.jpg", ProductID = 60 }

            );
        }

        // Them du lieu noi bat
        public static void SeedFeatured(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Featured>().HasData(

                 new Featured { ID = 1, Name = "Nổi bật" },
                 new Featured { ID = 2, Name = "Không nổi bật" }
                 );
        }

        // Them du lieu vai tro
        public static void SeedRoels(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Roles>().HasData(

                 new Roles { Id = 1, Name = "Admin", NormalizedName = "ADMIN" },
                 new Roles { Id = 2, Name = "Customer", NormalizedName = "CUSTOMER" },
                 new Roles { Id = 3, Name = "Guest", NormalizedName = "GUEST" }
            );
        }

        // Them du lieu gioi tinh cho san pham
        public static void SeedGenders(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Gender>().HasData(
                new Gender { ID = 1, Name = "Nam" },
                new Gender { ID = 2, Name = "Nữ" },
                new Gender { ID = 3, Name = "Unisex" }
            );
        }

        // Them du lieu chat loai cho san pham
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

        // Them du lieu kieu mau san pham
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

        // Them du lieu theo mua
        public static void SeedSeason(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Season>().HasData(
                new Season { ID = 1, Name = "Mùa xuân" },
                new Season { ID = 2, Name = "Mùa hạ" },
                new Season { ID = 3, Name = "Mùa thu" },
                new Season { ID = 4, Name = "Mùa đông" }
            );
        }

        // Them du lieu mau sac cho san pha 
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

        // Them du lieu kich co cho san pham
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

        // Them thuong hieu
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
              new Brand { ID = 9, Name = "Puma", PicturePath = "images/logo/logo_puma.jpg", Description = "Cung cấp thời trang thể thao, giày dép và phụ kiện chất lượng" },
              new Brand { ID = 10, Name = "Chanel", PicturePath = "images/logo/logo_chanel.jpg", Description = "Biểu tượng thời trang xa xỉ với những thiết kế đẳng cấp, cổ điển" }
         );
        }

        // Them danh muc
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

        // Them du lieu nhan dan
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

        // Them du lieu trung gian Product - Tag
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
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 1,  // Nike
                    CateID = 1,   // Áo khoác
                    GenderID = 1, // Nam
                    FeaturedID = 2,
                    Weight = 300
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
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 1,
                    CateID = 2,   // Quần
                    GenderID = 1, // Nam
                    FeaturedID = 2,
                    Weight = 200
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
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 1,
                    CateID = 3,   // Giày
                    GenderID = 1, // Nam
                    FeaturedID = 2,
                    Weight = 400
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
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 1,
                    CateID = 4,   // Váy
                    GenderID = 2, // Nữ
                    FeaturedID = 2,
                    Weight = 150
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
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 1,
                    CateID = 5,   // Áo thun
                    GenderID = 1, // Nam
                    FeaturedID = 2,
                    Weight = 150
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
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 1,
                    CateID = 6,   // Đồng hồ
                    GenderID = 3, // Unisex
                    FeaturedID = 2,
                    Weight = 50
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
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 2,
                    CateID = 1,   // Áo khoác
                    GenderID = 1, // Nam
                    FeaturedID = 2,
                    Weight = 350
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
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 2,
                    CateID = 2,   // Quần
                    GenderID = 1, // Nam
                    FeaturedID = 2,
                    Weight = 200
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
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 2,
                    CateID = 3,   // Giày
                    GenderID = 1, // Nam
                    FeaturedID = 2,
                    Weight = 400
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
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 2,
                    CateID = 4,   // Váy
                    GenderID = 2, // Nữ
                    FeaturedID = 2,
                    Weight = 150
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
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 2,
                    CateID = 5,   // Áo thun
                    GenderID = 1, // Nam
                    FeaturedID = 2,
                    Weight = 150
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
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 2,
                    CateID = 6,   // Đồng hồ
                    GenderID = 3, // Unisex
                    FeaturedID = 2,
                    Weight = 50
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
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 3,
                    CateID = 1,   // Áo khoác
                    GenderID = 2, // Nữ
                    FeaturedID = 2,
                    Weight = 400
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
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 3,
                    CateID = 2,   // Quần
                    GenderID = 2, // Nữ
                    FeaturedID = 2,
                    Weight = 250
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
                    PicturePath = "zara_shoes.jpg",
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 3,
                    CateID = 3,   // Giày
                    GenderID = 2, // Nữ
                    FeaturedID = 2,
                    Weight = 300
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
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 3,
                    CateID = 4,   // Váy
                    GenderID = 2, // Nữ
                    FeaturedID = 2,
                    Weight = 200
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
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 3,
                    CateID = 5,   // Áo thun
                    GenderID = 3, // Unisex
                    FeaturedID = 2,
                    Weight = 150
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
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 3,
                    CateID = 6,   // Đồng hồ
                    GenderID = 2, // Nữ
                    FeaturedID = 2,
                    Weight = 50
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
                    PicturePath = "h&m_jacket.jpg",
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 4,
                    CateID = 1,   // Áo khoác
                    GenderID = 1, // Nam
                    FeaturedID = 2,
                    Weight = 300
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
                    PicturePath = "h&m_pants.jpg",
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 4,
                    CateID = 2,   // Quần
                    GenderID = 1, // Nam
                    FeaturedID = 2,
                    Weight = 250
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
                    PicturePath = "h&m_shoes.jpg",
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 4,
                    CateID = 3,   // Giày
                    GenderID = 3, // Unisex
                    FeaturedID = 2,
                    Weight = 350
                },
                new Products
                {
                    ID = 22,
                    Name = "Váy Ngắn Hoa Nhí H&M",
                    NormalizedName = RemoveDiacritics("Váy Ngắn Hoa Nhí H&m").ToLower(),
                    Description = "Váy xinh xắn cho mùa hè",
                    NormalizedDescription = RemoveDiacritics("Váy xinh xắn cho mùa hè").ToLower(),
                    OldPrice = 0,
                    NewPrice = 600000,
                    Quantity = 45,
                    PicturePath = "h&m_dress.jpg",
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 4,
                    CateID = 4,   // Váy
                    GenderID = 2, // Nữ
                    FeaturedID = 2,
                    Weight = 150
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
                    PicturePath = "h&m_tshirt.jpg",
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 4,
                    CateID = 5,   // Áo thun
                    GenderID = 1, // Nam
                    FeaturedID = 2,
                    Weight = 150
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
                    PicturePath = "h&m_watch.jpg",
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 4,
                    CateID = 6,   // Đồng hồ
                    GenderID = 3, // Unisex
                    FeaturedID = 2,
                    Weight = 50
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
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 5,
                    CateID = 1,   // Áo khoác
                    GenderID = 1, // Nam
                    FeaturedID = 2,
                    Weight = 250
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
                    PicturePath = "uniqlo_pants.jpg",
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 5,
                    CateID = 2,   // Quần
                    GenderID = 1, // Nam
                    FeaturedID = 2,
                    Weight = 300
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
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 5,
                    CateID = 3,   // Giày
                    GenderID = 1, // Nam
                    FeaturedID = 2,
                    Weight = 350
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
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 5,
                    CateID = 4,   // Váy
                    GenderID = 2, // Nữ
                    FeaturedID = 2,
                    Weight = 150
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
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 5,
                    CateID = 5,   // Áo thun
                    GenderID = 3, // Unisex
                    FeaturedID = 2,
                    Weight = 150
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
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 5,
                    CateID = 6,   // Đồng hồ
                    GenderID = 1, // Nam
                    FeaturedID = 2,
                    Weight = 50
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
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 6,
                    CateID = 1,   // Áo khoác
                    GenderID = 2, // Nữ
                    FeaturedID = 2,
                    Weight = 500
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
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 6,
                    CateID = 2,   // Quần
                    GenderID = 1, // Nam
                    FeaturedID = 2,
                    Weight = 300
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
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 6,
                    CateID = 3,   // Giày
                    GenderID = 1, // Nam
                    FeaturedID = 2,
                    Weight = 450
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
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 6,
                    CateID = 4,   // Váy
                    GenderID = 2, // Nữ
                    FeaturedID = 2,
                    Weight = 200
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
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 6,
                    CateID = 5,   // Áo thun
                    GenderID = 3, // Unisex
                    FeaturedID = 2,
                    Weight = 150
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
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 6,
                    CateID = 6,   // Đồng hồ
                    GenderID = 2, // Nữ
                    FeaturedID = 2,
                    Weight = 100
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
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 7,
                    CateID = 1,   // Áo khoác
                    GenderID = 1, // Nam
                    FeaturedID = 2,
                    Weight = 400
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
                    PicturePath = "levis_pants.jpg",
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 7,
                    CateID = 2,   // Quần
                    GenderID = 1, // Nam
                    FeaturedID = 2,
                    Weight = 300
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
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 7,
                    CateID = 3,   // Giày
                    GenderID = 1, // Nam
                    FeaturedID = 2,
                    Weight = 350
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
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 7,
                    CateID = 4,   // Váy
                    GenderID = 2, // Nữ
                    FeaturedID = 2,
                    Weight = 250
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
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 7,
                    CateID = 5,   // Áo thun
                    GenderID = 3, // Unisex
                    FeaturedID = 2,
                    Weight = 150
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
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 7,
                    CateID = 6,   // Đồng hồ
                    GenderID = 1, // Nam
                    FeaturedID = 2,
                    Weight = 50
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
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 8,
                    CateID = 1,   // Áo khoác
                    GenderID = 3, // Unisex
                    FeaturedID = 2,
                    Weight = 300
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
                    PicturePath = "lacoste_pants.jpg",
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 8,
                    CateID = 2,   // Quần
                    GenderID = 1, // Nam
                    FeaturedID = 2,
                    Weight = 150
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
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 8,
                    CateID = 3,   // Giày
                    GenderID = 1, // Nam
                    FeaturedID = 2,
                    Weight = 400
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
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 8,
                    CateID = 4,   // Váy
                    GenderID = 2, // Nữ
                    FeaturedID = 2,
                    Weight = 150
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
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 8,
                    CateID = 5,   // Áo thun
                    GenderID = 1, // Nam
                    FeaturedID = 2,
                    Weight = 150
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
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 8,
                    CateID = 6,   // Đồng hồ
                    GenderID = 3, // Unisex
                    FeaturedID = 2,
                    Weight = 50
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
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 9,
                    CateID = 1,   // Áo khoác
                    GenderID = 1, // Nam
                    FeaturedID = 2,
                    Weight = 300
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
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 9,
                    CateID = 2,   // Quần
                    GenderID = 1, // Nam
                    FeaturedID = 2,
                    Weight = 200
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
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 9,
                    CateID = 3,   // Giày
                    GenderID = 1, // Nam
                    FeaturedID = 2,
                    Weight = 400
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
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 9,
                    CateID = 4,   // Váy
                    GenderID = 2, // Nữ
                    FeaturedID = 2,
                    Weight = 150
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
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 9,
                    CateID = 5,   // Áo thun
                    GenderID = 3, // Unisex
                    FeaturedID = 2,
                    Weight = 150
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
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 9,
                    CateID = 6,   // Đồng hồ
                    GenderID = 1, // Nam
                    FeaturedID = 2,
                    Weight = 50
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
                     Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 10,
                    CateID = 1,   // Áo khoác
                    GenderID = 2, // Nữ
                    FeaturedID = 1,
                    Weight = 450
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
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 10,
                    CateID = 2,   // Quần
                    GenderID = 2, // Nữ
                    FeaturedID = 1,
                    Weight = 250
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
                     Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 10,
                    CateID = 3,   // Giày
                    GenderID = 2, // Nữ
                    FeaturedID = 1,
                    Weight = 300
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
                     Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 10,
                    CateID = 4,   // Váy
                    GenderID = 2, // Nữ
                    FeaturedID = 1,
                    Weight = 200
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
                    Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 10,
                    CateID = 5,   // Áo thun
                    GenderID = 2, // Nữ
                    FeaturedID = 1,
                    Weight = 150
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
                     Created = DateTime.SpecifyKind(new DateTime(2025, 4, 18), DateTimeKind.Utc),
                    BrandID = 10,
                    CateID = 6,   // Đồng hồ
                    GenderID = 2, // Nữ
                    FeaturedID = 1,
                    Weight = 100
                },

            };
            #endregion

            // Chuyen doi duong dan hinh --> !IMPORTANT
            foreach (var product in products)
            {
                var baseName = System.IO.Path.GetFileNameWithoutExtension(product.PicturePath);

                //product.PicturePath.Replace("h&m", "h&m");
                product.PicturePath = $"{baseName}_1.1.jpg";
            }

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

