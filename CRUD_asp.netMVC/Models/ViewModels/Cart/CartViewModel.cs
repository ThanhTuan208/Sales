using CRUD_asp.netMVC.Models.Auth;
using CRUD_asp.netMVC.Models.Cart;
using CRUD_asp.netMVC.Models.Order;

namespace CRUD_asp.netMVC.Models.ViewModels.Cart
{
    public class CartViewModel
    {
        public List<AddToCart>? CartItems { get; set; }
        public List<AddToCart>? CartItemByIDs { get; set; }
        public QrPaymentViewModel QrPayment { get; set; }
        public Address? Address { get; set; }
        public double TotalPrice { get; set; }
    }
}
