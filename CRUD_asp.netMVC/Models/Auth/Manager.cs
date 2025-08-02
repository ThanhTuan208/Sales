using CRUD_asp.netMVC.Models.Product;
using System.ComponentModel.DataAnnotations;

namespace CRUD_asp.netMVC.Models.Auth
{
    public class Manager : FieldGeneralRoles
    {
        [Required, DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
    }
}
