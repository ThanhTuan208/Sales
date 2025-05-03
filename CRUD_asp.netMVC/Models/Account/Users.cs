using CRUD_asp.netMVC.Models.Cart;
using CRUD_asp.netMVC.Models.Product;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;

namespace CRUD_asp.netMVC.Models.Account
{
    public class Users : IdentityUser<int>
    {
        public string? ProfileImage { get; set; }
        public DateTime StartDate { get; set;  }

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
