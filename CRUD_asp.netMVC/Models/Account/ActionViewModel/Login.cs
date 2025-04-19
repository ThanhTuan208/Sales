using System.ComponentModel.DataAnnotations;

namespace CRUD_asp.netMVC.Models.Account.ActionViewModel
{
    public class Login
    {
        [Required,EmailAddress(ErrorMessage = "Loi {0}")]
        public string? Email { get; set; }
        
        [Required, DataType(DataType.Password)]
        public string? Password { get; set; }

        public bool RememberMe { get; set; } = false; 
    }
}
