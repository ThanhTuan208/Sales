using System.ComponentModel.DataAnnotations;

namespace CRUD_asp.netMVC.Models.Account.ActionViewModel
{
    public class Register
    {
        [Required, StringLength(50, ErrorMessage = "Loi {0}")]
        public string? UserName { get; set; }

        [Required, StringLength(50, ErrorMessage = "Loi {0}")]
        public string? FullName { get; set; }

        [Required, StringLength(50, ErrorMessage = "Loi {0}")]
        public string? RoleID { get; set; }

        [Required, EmailAddress]
        public string? Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string? Password { get; set; } 

        [Compare("Password",ErrorMessage = "Password doesn't match")]
        public string? RePassword { get; set; } 
    }
}
