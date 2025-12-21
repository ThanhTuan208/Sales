using CRUD_asp.netMVC.Models.Auth;
using CRUD_asp.netMVC.Models.Payments;
using CRUD_asp.netMVC.Models.Product;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace CRUD_asp.netMVC.ViewModels.Home
{
    public class SurplusMoneyViewModel : IBaseProfileViewModel
    {
        public List<Brand>? Brands { get; set; }
        public List<Category>? Categories { get; set; }
        public Users? Users { get; set; }
        public List<ExcessPayment>? ExcessPayments { get; set; }
        public List<UnderpaidOrder>? UnderpaidOrders { get; set; }

        public decimal TotalAmountInMonth { get; set; }
    }
}
