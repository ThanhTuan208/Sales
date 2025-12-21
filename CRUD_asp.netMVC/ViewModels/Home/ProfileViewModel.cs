using CRUD_asp.netMVC.Models.Auth;
using CRUD_asp.netMVC.Models.Product;

namespace CRUD_asp.netMVC.ViewModels.Home
{
    public class ProfileViewModel : IBaseProfileViewModel
    {
        public Users? Users { get; set; }
        public List<Brand>? Brands { get; set; }
        public List<Category>? Categories { get; set; }
    }
}
