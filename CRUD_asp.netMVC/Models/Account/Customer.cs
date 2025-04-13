using System.ComponentModel.DataAnnotations;

namespace CRUD_asp.netMVC.Models.Account
{
    public class Customer : FieldGeneral
    {      
        [Required, DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
}
