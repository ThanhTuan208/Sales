using CRUD_asp.netMVC.Models.Cart;
using CRUD_asp.netMVC.Models.Product;

namespace CRUD_asp.netMVC.ViewModels.Home
{
    public class getPaginationByProductViewModel
    {
        public PaginatedList<Products>? Products { get; set; }
        public PaginatedList<Brand>? Brands { get; set; }
        public PaginatedList<Category>? Categories { get; set; }
        public List<AddToCart>? Carts { get; set; }
    }
}
