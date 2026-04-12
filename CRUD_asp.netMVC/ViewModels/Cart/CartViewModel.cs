using CRUD_asp.netMVC.DTO.Cart;
using CRUD_asp.netMVC.Models.Addresses;
using CRUD_asp.netMVC.Models.Cart;
using CRUD_asp.netMVC.Models.Order;

namespace CRUD_asp.netMVC.ViewModels.Cart
{
    public class CartViewModel
    {
        public List<AddToCart>? CartItems { get; set; } = null!;
        public List<Address>? AddressUser { get; set; } = null!;
        public List<AddToCart>? CartItemByIDs { get; set; } = null!;
        public QrPaymentViewModel QrPayment { get; set; } = null!;
        public Address? Address { get; set; } = null!;
        public CartValidationResult ValidBuyNow { get; init; } = null!;
        public double? TotalPrice { get; set; }
    }
}
