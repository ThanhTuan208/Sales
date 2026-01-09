using CRUD_asp.netMVC.Common;
using CRUD_asp.netMVC.DTO.Generic;
using CRUD_asp.netMVC.ViewModels.Home;

namespace CRUD_asp.netMVC.Service.Home
{
    public interface IDisplayProfileUserService
    {
        Task<string> HandleProfileDisplayAsync(string options);
        Task<IBaseProfileViewModel> DisplayViewModel(string? option);
        Task<T> BuildBaseViewModelAsync<T>() where T : class, IBaseProfileViewModel, new();
    }
}
