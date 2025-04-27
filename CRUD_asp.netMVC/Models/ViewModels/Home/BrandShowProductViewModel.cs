using CRUD_asp.netMVC.Models.Product;

namespace CRUD_asp.netMVC.Models.ViewModels.Home
{
    public class BrandShowProductViewModel
    {
        public PaginatedList<Products>? Products { get; set; }
        public PaginatedList<Brand>? Brands { get; set; }
        public PaginatedList<Category>? Categories { get; set; }
        public Dictionary<int, List<Products>>? RelatedProductByBrands { get; set; }
    }
}
