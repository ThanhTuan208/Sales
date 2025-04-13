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

        [StringLength(50, MinimumLength = 5, ErrorMessage = "Do dai ten tu {2} den {1} ky tu")]
        [Required(ErrorMessage = "Ban can phai nhap Ten !!!")]
        public string? Name { get; set; }

        [NotMapped]
        [DataType(DataType.Upload)]
        public IFormFile? picture { get; set; }

        public string? PicturePath { get; set; }

        [StringLength(300, MinimumLength = 3, ErrorMessage = "Do dai mo ta tu {2} den {1} ky tu")]
        [Required(ErrorMessage = "Ban can phai nhap Mo ta !!!")]
        [Column(TypeName = "nvarchar(max)")]
        public string? Description { get; set; }

        [Range(5, 1000, ErrorMessage = "Gia cua san pham nam trong {1} - {2}")]
        [Required(ErrorMessage = "Ban can phai nhap Gia sp !!!")]
        [DataType(DataType.Currency)]
        public double Price { get; set; }

        [DataType(DataType.Date)]
        public DateTime Created { get; set; }

        [Required]
        [DisplayName("Manufactures")]
        public int? manuID { get; set; }
        public Manufacturer? Manufactures { get; set; }

        [Required]
        [DisplayName("Types")]
        public int? typeID { get; set; }
        public Prototype? Types { get; set; }

        public List<AddToCart>? Carts { get; set; }
        public List<OrderDetail>? OrderDetails { get; set; }
        public List<Reviews>? Reviews { get; set; }

    }

}
