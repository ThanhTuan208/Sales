using CRUD_asp.netMVC.Models.Product;
using Microsoft.AspNetCore.Identity;

namespace CRUD_asp.netMVC.Models.Account
{
    public class Users : IdentityUser
    {
        public string? ProfileImage { get; set; }

        public string? RoleID { get; set; }
        public Roles? Roles { get; set; }

        public Manager? Manager { get; set; }
        public Staff? Staff { get; set; }
        public Customer? Customer { get; set; }

        public List<AddToCart>? Carts { get; set; }
        public List<Orders>? Orders { get; set; }
        public List<Reviews>? Reviews { get; set; }
    }
}
