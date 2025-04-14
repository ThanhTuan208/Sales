using System.ComponentModel.DataAnnotations;

namespace CRUD_asp.netMVC.Models.Account
{
    public class Customer : FieldGeneralRoles
    {      
        [Required, DataType(DataType.Date)]
        public DateTime JoinDate { get; set; }
    }
}
