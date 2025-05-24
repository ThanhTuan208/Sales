using CRUD_asp.netMVC.Models.Cart;
using CRUD_asp.netMVC.Models.Product;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace CRUD_asp.netMVC.Models.Account
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
        public DateTime StartDate { get; set; }

        public int? RoleID { get; set; }
        public Roles? Roles { get; set; }

        public Manager? Manager { get; set; }
        public Staff? Staff { get; set; }
        public Customer? Customer { get; set; }

        public List<AddToCart>? Carts { get; set; }
        public List<Orders>? Orders { get; set; }
        public List<Reviews>? Reviews { get; set; }
    }
}
