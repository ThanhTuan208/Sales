using CRUD_asp.netMVC.Models.Cart;

namespace CRUD_asp.netMVC.Models.ViewModels.Cart
{
    public class CartViewModel
    {
        public List<AddToCart>? CartItems { get; set; }
        public List<AddToCart>? CartItemByIDs { get; set; }
        public double TotalPrice { get; set; }
    }
}
