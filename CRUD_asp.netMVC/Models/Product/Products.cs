using CRUD_asp.netMVC.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CRUD_asp.netMVC.Models.Product
{
    public class Products
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Ban can phai nhap Ten !!!"), StringLength(50, MinimumLength = 5, ErrorMessage = "Do dai ten tu {2} den {1} ky tu")]
        public string? Name { get; set; }

        [NotMapped]
        [DataType(DataType.Upload)]
        public IFormFile? picture { get; set; }
        public string? PicturePath { get; set; }

        [Required, StringLength(300, MinimumLength = 3, ErrorMessage = "Do dai mo ta tu {2} den {1} ky tu")]
        public string? Description { get; set; }

        [Range(5, 1000, ErrorMessage = "Gia cua san pham nam trong {1} - {2}")]
        [Required(ErrorMessage = "Ban can phai nhap Gia sp !!!")]
        [DataType(DataType.Currency)]
        public double Price { get; set; }

        [Required(ErrorMessage = "Ban can phai nhap {0} !!!")]
        public string? Size { get; set; }

        [Required(ErrorMessage = "Ban can phai nhap {0} !!!")]
        public string? Color { get; set; }

        [Required(ErrorMessage = "Ban can phai nhap {0} !!!")]
        public string? Gender { get; set; }

        [Required(ErrorMessage = "Ban can phai nhap {0} !!!")]
        public string? Material { get; set; }

        [Required(ErrorMessage = "Ban can phai nhap {0} !!!")]
        public int? Quantity { get; set; }

        [DataType(DataType.Date)]
        public DateTime Created { get; set; } = DateTime.Now;

        [Required]
        public int? BrandID { get; set; }
        public Brand? Brand { get; set; }

        [Required]
        public int? CateID { get; set; }
        public Category? Cate { get; set; }

        public List<AddToCart>? Carts { get; set; }
        public List<OrderDetail>? OrderDetails { get; set; }
        public List<Reviews>? Reviews { get; set; }

        public List<ProductStyle>? ProductStyles { get; set; }
        public List<ProductTag>? ProductTag { get; set; }
        public List<ProductSeason>? ProductSeason { get; set; }
    }
}
