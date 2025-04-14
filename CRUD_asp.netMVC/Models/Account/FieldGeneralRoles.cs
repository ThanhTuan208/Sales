using System.ComponentModel.DataAnnotations;

namespace CRUD_asp.netMVC.Models.Account
{
    public abstract class FieldGeneralRoles
    {
        [Key] public int ID { get; set; }

        [Required, StringLength(50, ErrorMessage = "Loi {0}")]
        public string? FirstName { get; set; }

        [Required, StringLength(50, ErrorMessage = "Loi {0}")]
        public string? LastName { get; set; }

        [Required, StringLength(50, ErrorMessage = "Loi {0}")]
        public string? UserName { get; set; }

        [Required, Phone]
        public string? PhoneNumber { get; set; }

        [Required, EmailAddress]
        public string? Email { get; set; }

        [StringLength(50, ErrorMessage = "Loi {0}")]
        public string? Address { get; set; }

        //[Required, StringLength(50, ErrorMessage = "Loi {0}")]
        //public string? Position { get; set; }

        public int? UserID { get; set; }
        public Users? Users { get; set; }
    }
}
