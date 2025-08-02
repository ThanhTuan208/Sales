using System.ComponentModel.DataAnnotations;

namespace CRUD_asp.netMVC.Models.Auth
{
    public class Customer : FieldGeneralRoles
    {      
        [Required, DataType(DataType.Date)]
        public DateTime JoinDate { get; set; }
    }
}
