using CRUD_asp.netMVC.Models.Account;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD_asp.netMVC.Models.Product
{
    public class UsersProduct
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Loi {0}")]
        public string? UserName { get; set; }

        [EmailAddress(ErrorMessage = "Loi {0}")]
        [Required(ErrorMessage = "Loi {0}")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Loi {0}")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Loi {0}")]
        public int? roleID { get; set; }
        public Roles? Roles { get; set; }

        public DateTime created_at { get; set; } 

        //public List<AddToCart>? Carts { get; set; }
        //public List<Orders>? Orders { get; set; }
        //public List<Reviews>? Reviews { get; set; }
    }
}
