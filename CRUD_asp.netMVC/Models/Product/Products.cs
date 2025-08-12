using CRUD_asp.netMVC.Models.Cart;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD_asp.netMVC.Models.Product
{
    public class Products
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Ban can phai nhap Ten !!!"), StringLength(50, MinimumLength = 5, ErrorMessage = "Do dai ten tu {2} den {1} ky tu")]
        public string? Name { get; set; }
        public string? NormalizedName { get; set; }

        [NotMapped]
        [DataType(DataType.Upload)]
        public IFormFile[]? picture { get; set; }

        public string? PicturePath { get; set; }

        [Required, Column(TypeName = "nvarchar(max)")]
        public string? Description { get; set; }
        public string? NormalizedDescription { get; set; }

        [DataType(DataType.Currency)]
        public double OldPrice { get; set; }

        [Required(ErrorMessage = "Ban can phai nhap {0} sp !!!")]
        [DataType(DataType.Currency)]
        public double NewPrice { get; set; }

        [Required(ErrorMessage = "Ban can phai nhap {0} !!!")]
        public int Quantity { get; set; }

        [DataType(DataType.Date)]
        public DateTime Created { get; set; } = DateTime.Now;

        [Required]
        public int? FeaturedID { get; set; }
        public Featured? Featured { get; set; }

        [Required]
        public int? GenderID { get; set; }
        public Gender? Gender { get; set; }

        [Required]
        public int? BrandID { get; set; }
        public Brand? Brands { get; set; }

        [Required]
        public int? CateID { get; set; }
        public Category? Cate { get; set; }

        public List<AddToCart>? Carts { get; set; }
        public List<OrderDetail>? OrderDetails { get; set; }
        public List<Reviews>? Reviews { get; set; }

        public List<ProductColors>? ProductColor { get; set; }
        public List<ProductSize>? ProductSize { get; set; }
        public List<ProductStyle>? ProductStyles { get; set; }
        public List<ProductTag>? ProductTags { get; set; }
        public List<ProductSeason>? ProductSeasons { get; set; }
        public List<ProductMaterial>? ProductMaterial { get; set; }
        public List<ProductImages>? ProductImages { get; set; }


    }
}
