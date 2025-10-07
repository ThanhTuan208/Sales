using CRUD_asp.netMVC.Data.Seed;
using CRUD_asp.netMVC.Models.Auth;
using CRUD_asp.netMVC.Models.Cart;
using CRUD_asp.netMVC.Models.Order;
using CRUD_asp.netMVC.Models.Product;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CRUD_asp.netMVC.Data
{
    public class AppDBContext : IdentityDbContext<Users, Roles, int>
    {
        public AppDBContext(DbContextOptions<AppDBContext> DBContext) : base(DBContext) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Thiết lập quan hệ giữa các thực thể
            modelBuilder.RelationshipEntitys();

            // Them du lieu Featured
            modelBuilder.SeedFeatured();

            // Them du lieu Roles
            modelBuilder.SeedRoels();

            // Them du lieu Gender
            modelBuilder.SeedGenders();

            // Them du lieu Material
            modelBuilder.SeedMaterial();

            // Them du lieu Style
            modelBuilder.SeedStyle();

            // Them du lieu Season
            modelBuilder.SeedSeason();

            // Them du lieu Color
            modelBuilder.SeedColor();

            // Them du lieu Size
            modelBuilder.SeedSize();

            // Them du lieu Brand
            modelBuilder.SeedBrand();

            // Them du lieu Cate
            modelBuilder.SeedCategory();

            // Them du lieu Tag
            modelBuilder.SeedTag();

            // Them du lieu ProductTags
            modelBuilder.SeedPoductTag();

            // Them du lieu ProductSize
            modelBuilder.SeedProductSize();

            // Them du lieu ProductColors
            modelBuilder.SeedProductColor();

            // Them du lieu ProductMaterial
            modelBuilder.SeedProductMaterial();

            // Them du lieu SeedProducts, productSeason, productStyle, productColor, productGender
            modelBuilder.SeedProducts_ProductSeason_ProductStyle();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        // Relationship entity DBSet
        //public DbSet<Users>? Users { get; set; }
        //public DbSet<Roles>? Roles { get; set; }
        //public DbSet<Staff> Staff { get; set; }
        public DbSet<Address>? Addresses { get; set; }
        public DbSet<Featured>? Featured { get; set; }
        public DbSet<Products>? Products { get; set; }
        public DbSet<Brand>? Brand { get; set; }
        public DbSet<Category>? Category { get; set; }
        public DbSet<AddToCart>? Carts { get; set; }
        public DbSet<Orders>? Orders { get; set; }
        public DbSet<OrderDetail>? OrderDetail { get; set; }
        public DbSet<Payment>? Payment { get; set; }
        public DbSet<Reviews>? Reviews { get; set; }
        public DbSet<Manager> Manager { get; set; }
        public DbSet<Customer> Customer { get; set; }
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
