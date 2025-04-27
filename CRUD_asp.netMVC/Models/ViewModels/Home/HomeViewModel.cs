using CRUD_asp.netMVC.Models.Product;

namespace CRUD_asp.netMVC.Models.ViewModels.Home
{
    public class HomeViewModel
    {
        public List<Products>? Products { get; set; }
        public List<Brand>? Brands { get; set; }
        public List<Category>? Categories { get; set; }
    }
}
