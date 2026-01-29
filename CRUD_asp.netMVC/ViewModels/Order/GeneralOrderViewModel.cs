using CRUD_asp.netMVC.Models.Order;
using CRUD_asp.netMVC.Models.Product;

namespace CRUD_asp.netMVC.ViewModels.Order
{
    public class GeneralOrderViewModel
    {
        public List<Products> Product { get; set; } = null!;
        public Orders Order { get; set; } = null!;
        public bool Success { get; set; }
    }
}
