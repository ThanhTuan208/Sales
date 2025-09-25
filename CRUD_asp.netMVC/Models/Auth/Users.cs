using CRUD_asp.netMVC.Models.Cart;
using CRUD_asp.netMVC.Models.Order;
using CRUD_asp.netMVC.Models.Product;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD_asp.netMVC.Models.Auth
{
    public class Users : IdentityUser<int>
    {

        [Required(ErrorMessage = "Cần nhập Tên")]
        [StringLength(50, ErrorMessage = "Họ không được vượt quá 50 ký tự")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Cần nhập Họ")]
        [StringLength(50, ErrorMessage = "Tên không được vượt quá 50 ký tự")]
        public string LastName { get; set; }

        public string? ProfileImage { get; set; }

        [DataType(DataType.Upload), NotMapped]
        public IFormFile? ProfileImageFile { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public int RoleID { get; set; }
        public Roles? Roles { get; set; }

        public Manager? Manager { get; set; }
        public Customer? Customer { get; set; }

        public List<Address>? Addresses { get; set; }
        public List<AddToCart>? Carts { get; set; }
        public List<Orders>? Orders { get; set; }
        public List<Reviews>? Reviews { get; set; }
    }
}
