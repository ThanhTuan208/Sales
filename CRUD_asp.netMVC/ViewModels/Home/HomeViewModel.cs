using CRUD_asp.netMVC.DTO.Home;
using CRUD_asp.netMVC.Models.Auth;
using CRUD_asp.netMVC.Models.Cart;
using CRUD_asp.netMVC.Models.Product;
using System.Drawing;

namespace CRUD_asp.netMVC.ViewModels.Home
{
    public class HomeViewModel
    {
        public List<Products>? Products { get; set; }
        public List<Brand>? Brands { get; set; }
        public List<Category>? Categories { get; set; }
        public List<AddToCart>? Carts { get; set; }
        public MailContactDTO? MailContact { get; set; }
        public Users? User { get; set; }
    }
}
