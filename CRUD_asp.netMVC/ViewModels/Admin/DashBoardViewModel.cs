using CRUD_asp.netMVC.DTO.Admin;
using CRUD_asp.netMVC.Models.Auth;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;

namespace CRUD_asp.netMVC.ViewModels.Admin
{
    public class DashBoardViewModel
    {
        public List<BaseDashboardDTO> DashBoards { get; set; } = new();
    }
}
