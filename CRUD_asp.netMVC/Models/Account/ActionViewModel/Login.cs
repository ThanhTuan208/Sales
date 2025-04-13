using System.ComponentModel.DataAnnotations;

namespace CRUD_asp.netMVC.Models.Account.ActionViewModel
{
    public class Login
    {
        [Required, StringLength(50, ErrorMessage = "Loi {0}")]
        public string? UserName { get; set; }
        
        [Required, DataType(DataType.Password)]
        public string? Password { get; set; }

    }
}
