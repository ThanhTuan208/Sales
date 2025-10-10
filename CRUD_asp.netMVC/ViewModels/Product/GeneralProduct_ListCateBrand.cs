using CRUD_asp.netMVC.Models.Cart;
using CRUD_asp.netMVC.Models.Product;

namespace CRUD_asp.netMVC.ViewModels.Product
{
    public class GeneralProduct_ListCateBrand
    {
        public Products? Product { get; set; }
        public List<Brand>? Brands { get; set; }
        public List<Category>? Categories { get; set; }
        public List<AddToCart>? Carts { get; set; }
        public int? Sold { get; set; }
    }
}
