using System.ComponentModel.DataAnnotations;

namespace CRUD_asp.netMVC.Models.Account.ActionViewModel
{
    public class Register
    {
        [Required, StringLength(50, ErrorMessage = "Loi {0}")]
        public string? UserName { get; set; }

        [Required, StringLength(50, ErrorMessage = "Loi {0}")]
        public string? FirstName { get; set; }

        [Required, StringLength(50, ErrorMessage = "Loi {0}")]
        public string? LastName { get; set; }

        [Required, EmailAddress]
        public string? Email { get; set; }

        [Required, Phone]
        public string? Phone { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime StartDate { get; set; } = DateTime.Now;

        [Required, DataType(DataType.Password, ErrorMessage = "Loi {0}")]
        public string? Password { get; set; }

        [Compare("Password", ErrorMessage = "Password doesn't match")]
        public string? ConfirmPassword { get; set; }

        public int RoleID { get; set; }
        public Roles? Roles { get; set; }

       
    }
}
