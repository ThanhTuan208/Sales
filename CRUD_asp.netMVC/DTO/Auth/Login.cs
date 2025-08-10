using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD_asp.netMVC.DTO.Auth
{
    public class Login
    {
        [Required(ErrorMessage = "Cần nhập Email"), EmailAddress(ErrorMessage = "Loi {0}")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Cần nhập Mật khẩu"), DataType(DataType.Password)]
        public string? Password { get; set; }

        public bool RememberMe { get; set; } = false;

        [NotMapped]
        public string? InfoGeneral { get; set; }
    }
}
