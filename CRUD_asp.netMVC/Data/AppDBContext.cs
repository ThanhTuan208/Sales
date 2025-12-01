using CRUD_asp.netMVC.Data.Seed;
using CRUD_asp.netMVC.Models.Auth;
using CRUD_asp.netMVC.Models.Cart;
using CRUD_asp.netMVC.Models.Order;
using CRUD_asp.netMVC.Models.Product;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CRUD_asp.netMVC.Data
{
    public class AppDBContext : IdentityDbContext<Users, Roles, int>
    {
        public AppDBContext(DbContextOptions<AppDBContext> DBContext) : base(DBContext) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Fluent API entity
            modelBuilder.RelationshipEntitys();

            // khong rang buoc
            modelBuilder.SeedFeatured();
            modelBuilder.SeedRoels();
            modelBuilder.SeedGenders();
            modelBuilder.SeedMaterial();
            modelBuilder.SeedStyle();
            modelBuilder.SeedSeason();
            modelBuilder.SeedColor();
            modelBuilder.SeedSize();
            modelBuilder.SeedTag();

            // Rang buoc khoa ngoai 1 - n
            modelBuilder.SeedBrand();
            modelBuilder.SeedCategory();

            modelBuilder.SeedProducts_ProductSeason_ProductStyle();
            modelBuilder.SeedProductImage();

            // Rang buoc khoa ngoai n - n
            modelBuilder.SeedProductSize();
            modelBuilder.SeedProductColor();
            modelBuilder.SeedProductMaterial();
            modelBuilder.SeedPoductTag();
            modelBuilder.SeedProductQty();

            // Indexing entity user
            modelBuilder.IndexPropertyUser();

            // Indexing entity payment
            modelBuilder.IndexPropertyPayment();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        // Entity-Relationship DBSet
        //public DbSet<Users>? Users { get; set; }
        //public DbSet<Roles>? Roles { get; set; }
        //public DbSet<Staff> Staff { get; set; }

        public DbSet<ProductQuantity> ProductQty { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Featured> Featured { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Brand> Brand { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<AddToCart> Carts { get; set; }

        public DbSet<Orders> Orders { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<Reviews> Reviews { get; set; }
        public DbSet<Manager> Manager { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }

        public DbSet<Season> Season { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<Style> Style { get; set; }
        public DbSet<Color> Color { get; set; }
        public DbSet<Size> Size { get; set; }
        public DbSet<Gender> Gender { get; set; }
        public DbSet<Material> Material { get; set; }
        public DbSet<ProductMaterial> ProductMaterial { get; set; }
        public DbSet<ProductColors> ProductColor { get; set; }
        public DbSet<ProductSize> ProductSize { get; set; }
        public DbSet<ProductTag> ProductTag { get; set; }
        public DbSet<ProductSeason> ProductSeason { get; set; }
        public DbSet<ProductStyle> ProductStyle { get; set; }
        public DbSet<ProductImages> ProductImages { get; set; }
    }
}
