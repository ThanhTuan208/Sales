using CRUD_asp.netMVC.Models.Account;
using CRUD_asp.netMVC.Models.Account.ActionViewModel;
using CRUD_asp.netMVC.Models.Product;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CRUD_asp.netMVC.Data
{
    public class AppDBContext : IdentityDbContext<Users, Roles, int>
    {
        public AppDBContext(DbContextOptions<AppDBContext> DBContext) : base(DBContext)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Quan li san pham -> ProductModel
            modelBuilder.Entity<Products>().HasOne(m => m.Brand).WithMany(p => p.products).HasForeignKey(mi => mi.BrandID).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Products>().HasOne(t => t.Cate).WithMany(p => p.products).HasForeignKey(ti => ti.CateID).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AddToCart>().HasOne(p => p.Product).WithMany(c => c.Carts).HasForeignKey(pi => pi.ProductID).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<AddToCart>().HasOne(u => u.Users).WithMany(c => c.Carts).HasForeignKey(ui => ui.UserID).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderDetail>().HasOne(o => o.Orders).WithMany(od => od.OrderDetail).HasForeignKey(oi => oi.orderID).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<OrderDetail>().HasOne(p => p.Product).WithMany(od => od.OrderDetails).HasForeignKey(pi => pi.ProductID).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Orders>().HasOne(u => u.Users).WithMany(o => o.Orders).HasForeignKey(pi => pi.UserID).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Payment>().HasOne(o => o.Order).WithOne(p => p.Payment).HasForeignKey<Payment>(o => o.OrderID).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reviews>().HasOne(r => r.Users).WithMany(r => r.Reviews).HasForeignKey(u => u.UserID).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Reviews>().HasOne(r => r.Product).WithMany(r => r.Reviews).HasForeignKey(u => u.ProductID).OnDelete(DeleteBehavior.Cascade);

            // Quan li du lieu khach hang -> AccountModel

            modelBuilder.Entity<Users>().HasOne(r => r.Manager).WithOne(u => u.Users).HasForeignKey<Manager>(mi => mi.UserID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Users>().HasOne(r => r.Staff).WithOne(u => u.Users).HasForeignKey<Staff>(mi => mi.UserID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Users>().HasOne(r => r.Customer).WithOne(u => u.Users).HasForeignKey<Customer>(mi => mi.UserID).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Login>().HasNoKey();
            modelBuilder.Entity<Register>().HasNoKey();

            modelBuilder.Entity<ProductSeason>().HasKey(p => new { p.SeasonID, p.ProductID });
            modelBuilder.Entity<ProductSeason>().HasOne(p => p.Product).WithMany(p => p.ProductSeason).HasForeignKey(p => p.ProductID);
            modelBuilder.Entity<ProductSeason>().HasOne(p => p.Season).WithMany(p => p.ProductSeason).HasForeignKey(p => p.SeasonID);

            modelBuilder.Entity<ProductTag>().HasKey(p => new { p.TagID, p.ProductID });
            modelBuilder.Entity<ProductTag>().HasOne(p => p.Product).WithMany(p => p.ProductTag).HasForeignKey(p => p.ProductID);
            modelBuilder.Entity<ProductTag>().HasOne(p => p.Tag).WithMany(p => p.ProductTag).HasForeignKey(p => p.TagID);

            modelBuilder.Entity<ProductStyle>().HasKey(p => new { p.StyleID, p.ProductID });
            modelBuilder.Entity<ProductStyle>().HasOne(p => p.Product).WithMany(p => p.ProductStyles).HasForeignKey(p => p.ProductID);
            modelBuilder.Entity<ProductStyle>().HasOne(p => p.Style).WithMany(p => p.ProductStyles).HasForeignKey(p => p.StyleID);

            var hashPass1 = new PasswordHasher<Users>().HashPassword(null, "admin123");
            var hashPass2 = new PasswordHasher<Users>().HashPassword(null, "123456");

            modelBuilder.Entity<Roles>().HasData(

                new Roles { Id = 1, Name = "Manager" },
                new Roles { Id = 2, Name = "Customer" }
                );

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

            // Dữ liệu mẫu cho Category (Loại sản phẩm)
            modelBuilder.Entity<Category>().HasData(
                new Category { ID = 1, Name = "Áo khoác" },
                new Category { ID = 2, Name = "Quần" },
                new Category { ID = 3, Name = "Giày" },
                new Category { ID = 4, Name = "Váy" },
                new Category { ID = 5, Name = "Áo thun" },
                new Category { ID = 6, Name = "Đồng hồ" }
            );

            // Dữ liệu mẫu cho Product (Sản phẩm)
            modelBuilder.Entity<Products>().HasData(
            new Products
            {
                ID = 1,
                Name = "Áo khoác nam Nike",
                Description = "Áo khoác nam chính hãng Nike, màu đen, phong cách thể thao",
                Price = 1500000,
                Size = "L",
                Color = "Đen",
                Gender = "Nam",
                Material = "Vải co giãn",
                Quantity = 100,
                PicturePath = "jacket1.jpg",
                Created = new DateTime(2025, 4, 18),
                BrandID = 1,  // Nike
                CateID = 1    // Áo khoác
            },
            new Products
            {
                ID = 2,
                Name = "Quần thể thao Nike",
                Description = "Quần thể thao Nike chất liệu thoáng mát, phù hợp tập luyện",
                Price = 900000,
                Size = "M",
                Color = "Xám",
                Gender = "Nam",
                Material = "Polyester",
                Quantity = 80,
                PicturePath = "pants1.webp",
                Created = new DateTime(2025, 4, 18),
                BrandID = 1,
                CateID = 2    // Quần
            },
            new Products
            {
                ID = 3,
                Name = "Giày Nike Air Max",
                Description = "Giày thể thao Nike Air Max thoải mái và bền bỉ",
                Price = 2800000,
                Size = "42",
                Color = "Trắng",
                Gender = "Nam",
                Material = "Vải lưới",
                Quantity = 60,
                PicturePath = "shoes1.jpg",
                Created = new DateTime(2025, 4, 18),
                BrandID = 1,
                CateID = 3    // Giày
            },
            new Products
            {
                ID = 4,
                Name = "Váy thể thao Nike nữ",
                Description = "Váy thể thao nữ năng động, thiết kế hiện đại",
                Price = 1100000,
                Size = "S",
                Color = "Hồng",
                Gender = "Nữ",
                Material = "Thun lạnh",
                Quantity = 40,
                PicturePath = "dress1.jpg",
                Created = new DateTime(2025, 4, 18),
                BrandID = 1,
                CateID = 4    // Váy
            },
            new Products
            {
                ID = 5,
                Name = "Áo thun Nike basic",
                Description = "Áo thun nam Nike cổ tròn, chất liệu cotton thoải mái",
                Price = 590000,
                Size = "L",
                Color = "Trắng",
                Gender = "Nam",
                Material = "Cotton",
                Quantity = 200,
                PicturePath = "tshirt1.jpg",
                Created = new DateTime(2025, 4, 18),
                BrandID = 1,
                CateID = 5    // Áo thun
            },
            new Products
            {
                ID = 6,
                Name = "Đồng hồ thể thao Nike",
                Description = "Đồng hồ thể thao phong cách Nike, chống nước, dây silicon",
                Price = 1900000,
                Size = "Free",
                Color = "Đen",
                Gender = "Unisex",
                Material = "Nhựa cao cấp",
                Quantity = 25,
                PicturePath = "watch1.jpg",
                Created = new DateTime(2025, 4, 18),
                BrandID = 1,
                CateID = 6    // Đồng hồ
            },
            new Products
            {
                ID = 7,
                Name = "Áo khoác Adidas Originals",
                Description = "Thiết kế cổ điển, chống gió và giữ ấm tốt",
                Price = 1450000,
                Size = "L",
                Color = "Xanh navy",
                Gender = "Nam",
                Material = "Polyester",
                Quantity = 70,
                PicturePath = "adidas_jacket.jpg",
                Created = new DateTime(2025, 4, 18),
                BrandID = 2,
                CateID = 1
            },
            new Products
            {
                ID = 8,
                Name = "Quần jogger Adidas",
                Description = "Quần thể thao thoải mái, phù hợp vận động",
                Price = 850000,
                Size = "M",
                Color = "Xám",
                Gender = "Nam",
                Material = "Cotton pha",
                Quantity = 90,
                PicturePath = "adidas_pants.jpg",
                Created = new DateTime(2025, 4, 18),
                BrandID = 2,
                CateID = 2
            },
            new Products
            {
                ID = 9,
                Name = "Giày Adidas Ultraboost",
                Description = "Đệm Boost êm ái, hỗ trợ chạy bộ hiệu quả",
                Price = 3200000,
                Size = "41",
                Color = "Trắng/Xanh",
                Gender = "Unisex",
                Material = "Vải Primeknit",
                Quantity = 50,
                PicturePath = "adidas_shoes.webp",
                Created = new DateTime(2025, 4, 18),
                BrandID = 2,
                CateID = 3
            },
            new Products
            {
                ID = 10,
                Name = "Váy tennis Adidas nữ",
                Description = "Váy thể thao nhẹ, thấm hút mồ hôi tốt",
                Price = 990000,
                Size = "S",
                Color = "Hồng pastel",
                Gender = "Nữ",
                Material = "Polyester",
                Quantity = 45,
                PicturePath = "adidas_dress.jpg",
                Created = new DateTime(2025, 4, 18),
                BrandID = 2,
                CateID = 4
            },
            new Products
            {
                ID = 11,
                Name = "Áo thun Adidas cổ tròn",
                Description = "Áo thun cổ điển, logo 3 sọc đặc trưng",
                Price = 650000,
                Size = "XL",
                Color = "Đen",
                Gender = "Nam",
                Material = "Cotton",
                Quantity = 150,
                PicturePath = "adidas_tshirt.jpg",
                Created = new DateTime(2025, 4, 18),
                BrandID = 2,
                CateID = 5
            },
            new Products
            {
                ID = 12,
                Name = "Đồng hồ thể thao Adidas",
                Description = "Thiết kế năng động, chống nước tốt",
                Price = 1750000,
                Size = "Free",
                Color = "Đỏ đậm",
                Gender = "Unisex",
                Material = "Silicon",
                Quantity = 30,
                PicturePath = "adidas_watch.jpg",
                Created = new DateTime(2025, 4, 18),
                BrandID = 2,
                CateID = 6
            },
            new Products { ID = 13, Name = "Áo khoác Zara Dáng Dài", Description = "Áo khoác nữ dáng dài thanh lịch", Price = 1900000, Size = "M", Color = "Be", Gender = "Nữ", Material = "Len pha", Quantity = 60, PicturePath = "zara_jacket.jpg", Created = new DateTime(2025, 4, 18), BrandID = 3, CateID = 1 },
            new Products { ID = 14, Name = "Quần Ống Rộng Zara", Description = "Phong cách hiện đại, thời trang", Price = 980000, Size = "S", Color = "Đen", Gender = "Nữ", Material = "Vải tuyết mưa", Quantity = 40, PicturePath = "zara_pants.jpg", Created = new DateTime(2025, 4, 18), BrandID = 3, CateID = 2 },
            new Products { ID = 15, Name = "Giày Cao Gót Zara", Description = "Thiết kế sang trọng, phù hợp tiệc tùng", Price = 1200000, Size = "38", Color = "Kem", Gender = "Nữ", Material = "Da tổng hợp", Quantity = 55, PicturePath = "zara_heels.jpg", Created = new DateTime(2025, 4, 18), BrandID = 3, CateID = 3 },
            new Products { ID = 16, Name = "Váy Xếp Ly Zara", Description = "Váy dài thướt tha, thanh lịch", Price = 1250000, Size = "M", Color = "Trắng", Gender = "Nữ", Material = "Voan", Quantity = 35, PicturePath = "zara_dress.jpg", Created = new DateTime(2025, 4, 18), BrandID = 3, CateID = 4 },
            new Products { ID = 17, Name = "Áo Thun Zara Basic", Description = "Áo thun đơn giản, dễ phối đồ", Price = 450000, Size = "M", Color = "Xám", Gender = "Unisex", Material = "Cotton", Quantity = 90, PicturePath = "zara_tshirt.jpg", Created = new DateTime(2025, 4, 18), BrandID = 3, CateID = 5 },
            new Products { ID = 18, Name = "Đồng Hồ Kim Zara", Description = "Thiết kế mặt tròn, dây da tinh tế", Price = 1350000, Size = "Free", Color = "Nâu", Gender = "Nữ", Material = "Thép không gỉ & da", Quantity = 25, PicturePath = "zara_watch.jpg", Created = new DateTime(2025, 4, 18), BrandID = 3, CateID = 6 },

             new Products { ID = 19, Name = "Áo Khoác Dù H&M", Description = "Chống gió, nhẹ và gọn", Price = 950000, Size = "L", Color = "Xanh rêu", Gender = "Nam", Material = "Nylon", Quantity = 80, PicturePath = "hm_jacket.jpg", Created = new DateTime(2025, 4, 18), BrandID = 4, CateID = 1 },
            new Products { ID = 20, Name = "Quần Tây H&M", Description = "Phong cách công sở lịch sự", Price = 700000, Size = "M", Color = "Xám đậm", Gender = "Nam", Material = "Polyester", Quantity = 65, PicturePath = "hm_pants.jpg", Created = new DateTime(2025, 4, 18), BrandID = 4, CateID = 2 },
            new Products { ID = 21, Name = "Giày Thể Thao H&M", Description = "Giày đơn giản, năng động", Price = 820000, Size = "42", Color = "Trắng", Gender = "Unisex", Material = "Canvas", Quantity = 100, PicturePath = "hm_shoes.jpg", Created = new DateTime(2025, 4, 18), BrandID = 4, CateID = 3 },
            new Products { ID = 22, Name = "Váy Ngắn Hoa Nhí H&M", Description = "Váy xinh xắn cho mùa hè", Price = 600000, Size = "S", Color = "Hồng", Gender = "Nữ", Material = "Cotton", Quantity = 45, PicturePath = "hm_dress.jpg", Created = new DateTime(2025, 4, 18), BrandID = 4, CateID = 4 },
            new Products { ID = 23, Name = "Áo Thun Nam Basic H&M", Description = "Áo thun trơn giá rẻ", Price = 200000, Size = "L", Color = "Xanh dương", Gender = "Nam", Material = "Cotton", Quantity = 150, PicturePath = "hm_tshirt.jpg", Created = new DateTime(2025, 4, 18), BrandID = 4, CateID = 5 },
            new Products { ID = 24, Name = "Đồng Hồ Dây Silicon H&M", Description = "Thiết kế đơn giản, hiện đại", Price = 550000, Size = "Free", Color = "Đen", Gender = "Unisex", Material = "Silicon", Quantity = 40, PicturePath = "hm_watch.jpg", Created = new DateTime(2025, 4, 18), BrandID = 4, CateID = 6 },

            new Products { ID = 25, Name = "Áo khoác lông vũ Uniqlo", Description = "Giữ ấm nhẹ nhàng, tiện lợi", Price = 1750000, Size = "M", Color = "Xanh navy", Gender = "Nam", Material = "Lông vũ", Quantity = 70, PicturePath = "uniqlo_jacket.jpg", Created = new DateTime(2025, 4, 18), BrandID = 5, CateID = 1 },
            new Products { ID = 26, Name = "Quần Jean Uniqlo", Description = "Jean co giãn, phong cách tối giản", Price = 950000, Size = "L", Color = "Xanh đậm", Gender = "Nam", Material = "Denim", Quantity = 50, PicturePath = "uniqlo_jeans.jpg", Created = new DateTime(2025, 4, 18), BrandID = 5, CateID = 2 },
            new Products { ID = 27, Name = "Giày Sneaker Uniqlo", Description = "Thiết kế tối giản, thoải mái", Price = 1050000, Size = "41", Color = "Trắng", Gender = "Nam", Material = "Vải dệt", Quantity = 60, PicturePath = "uniqlo_shoes.jpg", Created = new DateTime(2025, 4, 18), BrandID = 5, CateID = 3 },
            new Products { ID = 28, Name = "Váy Cotton Uniqlo", Description = "Thiết kế tối giản, thoải mái cho mùa hè", Price = 850000, Size = "S", Color = "Be", Gender = "Nữ", Material = "Cotton", Quantity = 40, PicturePath = "uniqlo_dress.jpg", Created = new DateTime(2025, 4, 18), BrandID = 5, CateID = 4 },
            new Products { ID = 29, Name = "Áo thun Airism Uniqlo", Description = "Thoáng mát, nhanh khô, thích hợp mùa hè", Price = 400000, Size = "L", Color = "Xám", Gender = "Unisex", Material = "Vải Airism", Quantity = 120, PicturePath = "uniqlo_tshirt.jpg", Created = new DateTime(2025, 4, 18), BrandID = 5, CateID = 5 },
            new Products { ID = 30, Name = "Đồng hồ tối giản Uniqlo", Description = "Phong cách tối giản, dây da đen", Price = 980000, Size = "Free", Color = "Đen", Gender = "Nam", Material = "Kim loại", Quantity = 30, PicturePath = "uniqlo_watch.jpg", Created = new DateTime(2025, 4, 18), BrandID = 5, CateID = 6 },

             new Products { ID = 31, Name = "Áo khoác cao cấp Gucci", Description = "Thiết kế sang trọng, dành cho thời trang cao cấp", Price = 25000000, Size = "M", Color = "Đỏ đô", Gender = "Nữ", Material = "Lông cừu", Quantity = 10, PicturePath = "gucci_jacket.jpg", Created = new DateTime(2025, 4, 18), BrandID = 6, CateID = 1 },
            new Products { ID = 32, Name = "Quần tây Gucci nam", Description = "Phong cách lịch lãm, chất liệu cao cấp", Price = 15500000, Size = "L", Color = "Đen", Gender = "Nam", Material = "Lụa pha", Quantity = 15, PicturePath = "gucci_pants.jpg", Created = new DateTime(2025, 4, 18), BrandID = 6, CateID = 2 },
            new Products { ID = 33, Name = "Giày lười Gucci", Description = "Thiết kế logo đặc trưng, đẳng cấp", Price = 22000000, Size = "42", Color = "Nâu", Gender = "Nam", Material = "Da thật", Quantity = 12, PicturePath = "gucci_shoes.jpg", Created = new DateTime(2025, 4, 18), BrandID = 6, CateID = 3 },
            new Products { ID = 34, Name = "Váy lụa cao cấp Gucci", Description = "Chất liệu mềm mại, sang trọng", Price = 30000000, Size = "S", Color = "Vàng", Gender = "Nữ", Material = "Lụa", Quantity = 8, PicturePath = "gucci_dress.jpg", Created = new DateTime(2025, 4, 18), BrandID = 6, CateID = 4 },
            new Products { ID = 35, Name = "Áo thun Gucci logo lớn", Description = "Phong cách trẻ trung, thời thượng", Price = 9000000, Size = "L", Color = "Trắng", Gender = "Unisex", Material = "Cotton Ý", Quantity = 20, PicturePath = "gucci_tshirt.jpg", Created = new DateTime(2025, 4, 18), BrandID = 6, CateID = 5 },
            new Products { ID = 36, Name = "Đồng hồ đính đá Gucci", Description = "Đồng hồ xa xỉ với thiết kế tinh xảo", Price = 45000000, Size = "Free", Color = "Vàng hồng", Gender = "Nữ", Material = "Kim cương nhân tạo", Quantity = 5, PicturePath = "gucci_watch.jpg", Created = new DateTime(2025, 4, 18), BrandID = 6, CateID = 6 },

             new Products { ID = 37, Name = "Áo khoác jean Levi's", Description = "Chất liệu jean bền, phong cách cổ điển", Price = 1450000, Size = "M", Color = "Xanh jean", Gender = "Nam", Material = "Denim", Quantity = 40, PicturePath = "levis_jacket.jpg", Created = new DateTime(2025, 4, 18), BrandID = 7, CateID = 1 },
            new Products { ID = 38, Name = "Quần jeans Levi's 501", Description = "Form dáng chuẩn Mỹ, chất lượng cao", Price = 1250000, Size = "L", Color = "Xanh", Gender = "Nam", Material = "Denim", Quantity = 60, PicturePath = "levis_jeans.jpg", Created = new DateTime(2025, 4, 18), BrandID = 7, CateID = 2 },
            new Products { ID = 39, Name = "Giày thể thao Levi's", Description = "Thoải mái và thời trang hàng ngày", Price = 1150000, Size = "42", Color = "Trắng", Gender = "Nam", Material = "Vải canvas", Quantity = 35, PicturePath = "levis_shoes.jpg", Created = new DateTime(2025, 4, 18), BrandID = 7, CateID = 3 },
            new Products { ID = 40, Name = "Váy bò Levi's nữ", Description = "Thiết kế trẻ trung, năng động", Price = 980000, Size = "S", Color = "Xanh jean", Gender = "Nữ", Material = "Denim", Quantity = 20, PicturePath = "levis_dress.jpg", Created = new DateTime(2025, 4, 18), BrandID = 7, CateID = 4 },
            new Products { ID = 41, Name = "Áo thun cổ tròn Levi's", Description = "Logo cổ điển, phong cách Mỹ", Price = 500000, Size = "M", Color = "Trắng", Gender = "Unisex", Material = "Cotton", Quantity = 100, PicturePath = "levis_tshirt.jpg", Created = new DateTime(2025, 4, 18), BrandID = 7, CateID = 5 },
            new Products { ID = 42, Name = "Đồng hồ dây da Levi's", Description = "Đơn giản, mạnh mẽ, cá tính", Price = 850000, Size = "Free", Color = "Nâu", Gender = "Nam", Material = "Thép + da", Quantity = 15, PicturePath = "levis_watch.jpg", Created = new DateTime(2025, 4, 18), BrandID = 7, CateID = 6 },

             new Products { ID = 43, Name = "Áo khoác thể thao Lacoste", Description = "Thiết kế năng động, thời trang", Price = 2700000, Size = "L", Color = "Trắng", Gender = "Nam", Material = "Polyester", Quantity = 30, PicturePath = "lacoste_jacket.jpg", Created = new DateTime(2025, 4, 18), BrandID = 8, CateID = 1 },
            new Products { ID = 44, Name = "Quần short Lacoste", Description = "Phong cách thể thao, thoáng mát", Price = 1450000, Size = "M", Color = "Xanh dương", Gender = "Nam", Material = "Cotton", Quantity = 40, PicturePath = "lacoste_shorts.jpg", Created = new DateTime(2025, 4, 18), BrandID = 8, CateID = 2 },
            new Products { ID = 45, Name = "Giày sneaker Lacoste", Description = "Chất liệu cao cấp, thiết kế tinh tế", Price = 2200000, Size = "41", Color = "Trắng", Gender = "Nam", Material = "Da", Quantity = 25, PicturePath = "lacoste_shoes.jpg", Created = new DateTime(2025, 4, 18), BrandID = 8, CateID = 3 },
            new Products { ID = 46, Name = "Váy polo Lacoste", Description = "Lịch sự, sang trọng cho nữ", Price = 1900000, Size = "S", Color = "Hồng", Gender = "Nữ", Material = "Cotton pha", Quantity = 18, PicturePath = "lacoste_dress.jpg", Created = new DateTime(2025, 4, 18), BrandID = 8, CateID = 4 },
            new Products { ID = 47, Name = "Áo thun cá sấu Lacoste", Description = "Biểu tượng nổi bật với logo cá sấu", Price = 1100000, Size = "L", Color = "Xanh lá", Gender = "Nam", Material = "Cotton", Quantity = 55, PicturePath = "lacoste_tshirt.jpg", Created = new DateTime(2025, 4, 18), BrandID = 8, CateID = 5 },
            new Products { ID = 48, Name = "Đồng hồ thể thao Lacoste", Description = "Năng động, trẻ trung, phù hợp với người chơi thể thao", Price = 2600000, Size = "Free", Color = "Xanh navy", Gender = "Unisex", Material = "Nhựa + kim loại", Quantity = 20, PicturePath = "lacoste_watch.jpg", Created = new DateTime(2025, 4, 18), BrandID = 8, CateID = 6 },

             new Products { ID = 49, Name = "Áo khoác thể thao Puma", Description = "Thiết kế hiện đại, năng động", Price = 1550000, Size = "L", Color = "Xám", Gender = "Nam", Material = "Polyester", Quantity = 50, PicturePath = "puma_jacket.jpg", Created = new DateTime(2025, 4, 18), BrandID = 9, CateID = 1 },
            new Products { ID = 50, Name = "Quần jogger Puma", Description = "Thoải mái, phù hợp vận động", Price = 1100000, Size = "M", Color = "Đen", Gender = "Nam", Material = "Thun co giãn", Quantity = 70, PicturePath = "puma_pants.jpg", Created = new DateTime(2025, 4, 18), BrandID = 9, CateID = 2 },
            new Products { ID = 51, Name = "Giày chạy bộ Puma", Description = "Thiết kế thể thao, hỗ trợ di chuyển", Price = 1800000, Size = "42", Color = "Xanh", Gender = "Nam", Material = "Vải + Cao su", Quantity = 40, PicturePath = "puma_shoes.jpg", Created = new DateTime(2025, 4, 18), BrandID = 9, CateID = 3 },
            new Products { ID = 52, Name = "Váy thể thao Puma nữ", Description = "Phù hợp tập luyện hoặc mặc thường ngày", Price = 950000, Size = "S", Color = "Hồng", Gender = "Nữ", Material = "Cotton", Quantity = 25, PicturePath = "puma_dress.jpg", Created = new DateTime(2025, 4, 18), BrandID = 9, CateID = 4 },
            new Products { ID = 53, Name = "Áo thun thể thao Puma", Description = "Chất vải thấm hút, thoáng mát", Price = 600000, Size = "M", Color = "Trắng", Gender = "Unisex", Material = "Polyester", Quantity = 90, PicturePath = "puma_tshirt.jpg", Created = new DateTime(2025, 4, 18), BrandID = 9, CateID = 5 },
            new Products { ID = 54, Name = "Đồng hồ thể thao Puma", Description = "Thiết kế mạnh mẽ, phong cách", Price = 1200000, Size = "Free", Color = "Đen", Gender = "Nam", Material = "Nhựa cao cấp", Quantity = 18, PicturePath = "puma_watch.jpg", Created = new DateTime(2025, 4, 18), BrandID = 9, CateID = 6 },

            new Products { ID = 55, Name = "Áo khoác dạ Chanel", Description = "Đẳng cấp, thời thượng, thiết kế quý phái", Price = 9500000, Size = "S", Color = "Đen", Gender = "Nữ", Material = "Dạ cao cấp", Quantity = 15, PicturePath = "chanel_jacket.jpg", Created = new DateTime(2025, 4, 18), BrandID = 10, CateID = 1 },
            new Products { ID = 56, Name = "Quần vải cao cấp Chanel", Description = "Đường may tinh xảo, chất liệu mềm mịn", Price = 7200000, Size = "M", Color = "Be", Gender = "Nữ", Material = "Vải lụa", Quantity = 12, PicturePath = "chanel_pants.jpg", Created = new DateTime(2025, 4, 18), BrandID = 10, CateID = 2 },
            new Products { ID = 57, Name = "Giày cao gót Chanel", Description = "Thanh lịch, sang trọng", Price = 8300000, Size = "37", Color = "Đỏ đô", Gender = "Nữ", Material = "Da thật", Quantity = 10, PicturePath = "chanel_heels.jpg", Created = new DateTime(2025, 4, 18), BrandID = 10, CateID = 3 },
            new Products { ID = 58, Name = "Váy dạ hội Chanel", Description = "Lộng lẫy, thiết kế quyến rũ", Price = 13000000, Size = "S", Color = "Trắng", Gender = "Nữ", Material = "Lụa", Quantity = 8, PicturePath = "chanel_dress.jpg", Created = new DateTime(2025, 4, 18), BrandID = 10, CateID = 4 },
            new Products { ID = 59, Name = "Áo thun cao cấp Chanel", Description = "Đơn giản nhưng đầy tinh tế", Price = 3200000, Size = "M", Color = "Xám", Gender = "Nữ", Material = "Cotton", Quantity = 20, PicturePath = "chanel_tshirt.jpg", Created = new DateTime(2025, 4, 18), BrandID = 10, CateID = 5 },
            new Products { ID = 60, Name = "Đồng hồ Chanel sang trọng", Description = "Tinh xảo, sang trọng và nữ tính", Price = 18000000, Size = "Free", Color = "Vàng", Gender = "Nữ", Material = "Thép mạ vàng", Quantity = 6, PicturePath = "chanel_watch.jpg", Created = new DateTime(2025, 4, 18), BrandID = 10, CateID = 6 }
        );

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

            // Dữ liệu mẫu cho ProductTag (Liên kết giữa sản phẩm và thẻ)
            modelBuilder.Entity<ProductTag>().HasData(
                new ProductTag { ProductID = 1, TagID = 1 }, // Áo khoác nam Nike, Thẻ "Thời trang"
                new ProductTag { ProductID = 2, TagID = 2 }, // Quần Jeans Adidas, Thẻ "Thể thao"
                new ProductTag { ProductID = 3, TagID = 3 }, // Giày thể thao Adidas UltraBoost, Thẻ "Giày thể thao"
                new ProductTag { ProductID = 4, TagID = 4 }  // Váy nữ Zara, Thẻ "Nữ"
            );
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        //public DbSet<Users>? Users { get; set; }
        //public DbSet<Roles>? Roles { get; set; }
        public DbSet<Products>? Products { get; set; }
        public DbSet<Brand>? Brand { get; set; }
        public DbSet<Category>? Category { get; set; }
        public DbSet<AddToCart>? Carts { get; set; }
        public DbSet<Orders>? Orders { get; set; }
        public DbSet<OrderDetail>? OrderDetail { get; set; }
        public DbSet<Payment>? Payment { get; set; }
        public DbSet<Reviews>? Reviews { get; set; }
        public DbSet<Manager> Manager { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Season> Season { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<Style> Style { get; set; }
    }
}
