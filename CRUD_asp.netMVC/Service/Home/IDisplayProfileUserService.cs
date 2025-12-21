using CRUD_asp.netMVC.Common;
using CRUD_asp.netMVC.DTO.Generic;

namespace CRUD_asp.netMVC.Service.Home
{
    public interface IDisplayProfileUserService
    {
        Task<string> HandleProfileDisplayAsync(string options);
    }
}
