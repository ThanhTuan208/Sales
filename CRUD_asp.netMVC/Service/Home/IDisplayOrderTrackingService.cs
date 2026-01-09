using CRUD_asp.netMVC.Common;
using CRUD_asp.netMVC.DTO.Generic;
using CRUD_asp.netMVC.Models.Order;
using CRUD_asp.netMVC.ViewModels.Home;

namespace CRUD_asp.netMVC.Service.Home
{
    public interface IDisplayOrderTrackingService
    {
         Task<HomeViewModel> DisplayOrderPaidItemsAsync(HomeViewModel model, int userId);
    }
}
