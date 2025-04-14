using CRUD_asp.netMVC.Models.Account;
using CRUD_asp.netMVC.Models.Account.ActionViewModel;
using CRUD_asp.netMVC.Models.Product;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Diagnostics;
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
            modelBuilder.Entity<Products>().HasOne(m => m.Manufactures).WithMany(p => p.products).HasForeignKey(mi => mi.manuID).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Products>().HasOne(t => t.Types).WithMany(p => p.products).HasForeignKey(ti => ti.typeID).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AddToCart>().HasOne(p => p.Product).WithMany(c => c.Carts).HasForeignKey(pi => pi.ProductID).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<AddToCart>().HasOne(u => u.Users).WithMany(c => c.Carts).HasForeignKey(ui => ui.UserID).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderDetail>().HasOne(o => o.Orders).WithMany(od => od.OrderDetail).HasForeignKey(oi => oi.orderID).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<OrderDetail>().HasOne(p => p.Product).WithMany(od => od.OrderDetails).HasForeignKey(pi => pi.ProductID).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Orders>().HasOne(u => u.Users).WithMany(o => o.Orders).HasForeignKey(pi => pi.UserID).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Payment>().HasOne(o => o.Order).WithOne(p => p.Payment).HasForeignKey<Payment>(o => o.OrderID).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reviews>().HasOne(r => r.Users).WithMany(r => r.Reviews).HasForeignKey(u => u.UserID).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Reviews>().HasOne(r => r.Product).WithMany(r => r.Reviews).HasForeignKey(u => u.ProductID).OnDelete(DeleteBehavior.Cascade);

            // Quan li du lieu khach hang -> AccountModel
            modelBuilder.Entity<Users>().HasOne(r => r.Roles).WithMany(u => u.Users).HasForeignKey(mi => mi.RoleID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Users>().HasOne(r => r.Manager).WithOne(u => u.Users).HasForeignKey<Manager>(mi => mi.UserID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Users>().HasOne(r => r.Staff).WithOne(u => u.Users).HasForeignKey<Staff>(mi => mi.UserID).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Users>().HasOne(r => r.Customer).WithOne(u => u.Users).HasForeignKey<Customer>(mi => mi.UserID).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Login>().HasNoKey();
            modelBuilder.Entity<Register>().HasNoKey();

            var hashPass1 = new PasswordHasher<Users>().HashPassword(null, "admin123");
            var hashPass2 = new PasswordHasher<Users>().HashPassword(null, "123456");
            Console.WriteLine(hashPass1);
            Console.WriteLine(hashPass2);

            modelBuilder.Entity<Roles>().HasData(

                new Roles { Id = 1, Name = "Manager", UserID = 1},
                new Roles { Id = 2, Name = "Customer", UserID = 2 }
                );

            modelBuilder.Entity<Users>().HasData(
                new Users
                {
                    Id = 1,
                    UserName = "admin",
                    PasswordHash = "AQAAAAIAAYagAAAAEGhZBXBsy/SAWop7np1SB500eP6vRfUwZNb8eitfnV26cygqJSe+Ks/v2da7SxC+uQ==",
                    Email = "nguyenthanhtuankrp1@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumber = "0358986823",
                    ProfileImage = "",
                    RoleID = 1,
                    ConcurrencyStamp = "22222222-2222-2222-2222-222222222222",
                    //SecurityStamp = Guid.NewGuid().ToString("D"),
                    SecurityStamp = "5f8a17cb-bb0d-4ae4-99cb-abcde1234567"
                },

                new Users
                {
                    Id = 2,
                    UserName = "LongTinh",
                    PasswordHash = "AQAAAAIAAYagAAAAEAGa4VXA3HYnofgsMNO/bcZ8e+1MBrDEjtBTeCfYi0k1HUn9ForK2SKEUvKj+idFfw",
                    Email = "nguyenthanhtuankrp1@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumber = "034342434",
                    ProfileImage = "",
                    RoleID = 2,
                    ConcurrencyStamp = "22222222-2222-2222-2222-222222222222",
                    //SecurityStamp = Guid.NewGuid().ToString("D"),
                    SecurityStamp = "5f8a17cb-bb0d-4ae4-99cb-abcde1234567"
                }
                );

            modelBuilder.Entity<Manager>().HasData(
                new Manager
                {
                    ID = 1,
                    UserID = 1,
                    FullName = "Nguyen Thanh Tuan",
                    UserName = "admin",
                    Password = "admmin",
                    Position = "Manager",
                    PhoneNumber = "0358986823",
                    Email = "nguyenthanhtuankrp1@gmail.com",
                    Address = "TP HCM",
                    StartDate = DateTime.Parse("2025-04-13")
                }
                );

            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    ID = 1,
                    UserID = 2,
                    FullName = "Nguyen Thanh Long",
                    UserName = "LongTinh",
                    Password = "123456",
                    Position = "Customer",
                    PhoneNumber = "0343424334",
                    Email = "longtinh@gmail.com",
                    Address = "TP HCM",
                    Date = DateTime.Parse("2025-04-13")
                }
                );

            modelBuilder.Entity<Manufacturer>().HasData(
                new Manufacturer { ID = 1, Name = "Apple" },
                new Manufacturer { ID = 2, Name = "Samsung" },
                new Manufacturer { ID = 3, Name = "Sony" },
                new Manufacturer { ID = 4, Name = "Dell" },
                new Manufacturer { ID = 5, Name = "Asus" }
            );

            modelBuilder.Entity<Prototype>().HasData(
                new Prototype { ID = 1, Name = "Smartphone" },
                new Prototype { ID = 2, Name = "Laptop" },
                new Prototype { ID = 3, Name = "Tablet" },
                new Prototype { ID = 4, Name = "Headphones" },
                new Prototype { ID = 5, Name = "Smartwatch" }
            );



            modelBuilder.Entity<Products>().HasData(
               new Products
               {
                   ID = 1,
                   Name = "iPhone 14 Pro",
                   PicturePath = "iphone14pro.jpg",
                   Description = "Điện thoại Apple với camera 48MP, chip A16 Bionic.",
                   Price = 999,
                   Created = new DateTime(2025, 4, 8),
                   manuID = 1,
                   typeID = 1
               },
               new Products
               {
                   ID = 2,
                   Name = "Samsung Galaxy S23 Ultra",
                   PicturePath = "s23ultra.jpg",
                   Description = "Flagship mạnh mẽ nhất của Samsung, màn hình 120Hz.",
                   Price = 1199,
                   Created = new DateTime(2025, 4, 8),
                   manuID = 2,
                   typeID = 1
               },
               new Products
               {
                   ID = 3,
                   Name = "Sony WH-1000XM5",
                   PicturePath = "sonyxm5.jpg",
                   Description = "Tai nghe chống ồn tốt nhất, chất âm cao cấp.",
                   Price = 349,
                   Created = new DateTime(2025, 4, 8),
                   manuID = 3,
                   typeID = 4
               },
               new Products
               {
                   ID = 4,
                   Name = "MacBook Pro M2 13 inch",
                   PicturePath = "macbookpro13.jpg",
                   Description = "Laptop Apple chạy chip M2, hiệu năng mạnh mẽ.",
                   Price = 1299,
                   Created = new DateTime(2025, 4, 8),
                   manuID = 1,
                   typeID = 2
               },
               new Products
               {
                   ID = 5,
                   Name = "Asus ROG Zephyrus G14",
                   PicturePath = "zephyrusg14.jpg",
                   Description = "Laptop gaming mỏng nhẹ, card RTX 4060.",
                   Price = 1599,
                   Created = new DateTime(2025, 4, 8),
                   manuID = 5,
                   typeID = 2
               },
               new Products
               {
                   ID = 6,
                   Name = "iPad Air 5th Gen",
                   PicturePath = "ipadair5.jpg",
                   Description = "Tablet Apple với chip M1, thiết kế mỏng nhẹ.",
                   Price = 599,
                   Created = new DateTime(2025, 4, 8),
                   manuID = 1,
                   typeID = 3
               },
               new Products
               {
                   ID = 7,
                   Name = "Samsung Galaxy Tab S8",
                   PicturePath = "tabs8.jpg",
                   Description = "Tablet cao cấp với màn hình AMOLED 120Hz.",
                   Price = 699,
                   Created = new DateTime(2025, 4, 8),
                   manuID = 2,
                   typeID = 3
               },
               new Products
               {
                   ID = 8,
                   Name = "Apple Watch Series 9",
                   PicturePath = "applewatch9.jpg",
                   Description = "Đồng hồ thông minh của Apple, theo dõi sức khỏe.",
                   Price = 399,
                   Created = new DateTime(2025, 4, 8),
                   manuID = 1,
                   typeID = 5
               },
               new Products
               {
                   ID = 9,
                   Name = "Apple Watch Series 10",
                   PicturePath = "applewatch10.jpg",
                   Description = "Đồng hồ thông minh của Apple, theo dõi sức khỏe.",
                   Price = 999,
                   Created = new DateTime(2025, 4, 8),
                   manuID = 1,
                   typeID = 5
               }
            );

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        //public DbSet<Users>? Users { get; set; }
        public DbSet<Products>? Products { get; set; }
        public DbSet<Manufacturer>? Manufactures { get; set; }
        public DbSet<Prototype>? Prototypes { get; set; }
        public DbSet<AddToCart>? Carts { get; set; }
        public DbSet<Orders>? Orders { get; set; }
        public DbSet<OrderDetail>? OrderDetail { get; set; }
        public DbSet<Payment>? Payment { get; set; }
        //public DbSet<Roles>? Roles { get; set; }
        public DbSet<Reviews>? Reviews { get; set; }
        public DbSet<Manager> Manager { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<Customer> Customer { get; set; }

    }
}
