using CRUD_asp.netMVC.Models.Auth;
using CRUD_asp.netMVC.Models.Product;

namespace CRUD_asp.netMVC.ViewModels.Home
{
    public interface IBaseProfileViewModel
    {
        Users? Users { get; set; }
        List<Brand>? Brands { get; set; }
        List<Category>? Categories { get; set; }
    }
}
