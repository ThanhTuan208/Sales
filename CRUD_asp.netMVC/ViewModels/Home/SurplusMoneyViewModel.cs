using CRUD_asp.netMVC.DTO.Payments;
using CRUD_asp.netMVC.Models.Auth;
using CRUD_asp.netMVC.Models.Payments;
using CRUD_asp.netMVC.Models.Product;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace CRUD_asp.netMVC.ViewModels.Home
{
    public class SurplusMoneyViewModel : IBaseProfileViewModel
    {
        public Users Users { get; set; } = null!;
        public List<Brand>? Brands { get; set; }
        public List<Category>? Categories { get; set; } 
        public List<MoneyFlowLogDTO>? MoneyFlowLogs { get; set; } = null!;

        public decimal TotalAmountInMonth { get; set; }
        public decimal PaidAmountInMonth { get; set; }
        public decimal ExcessMoneyNow { get; set; }
        public decimal PaidMoney { get; set; }
        public string UpdateAt { get; set; } = null!;
    }
}
