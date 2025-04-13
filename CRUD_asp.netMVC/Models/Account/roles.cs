using CRUD_asp.netMVC.Models.Product;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD_asp.netMVC.Models.Account
{
    public class Roles
    {
        [Key, Column(TypeName = "nvarchar(10)")]
        public string? ID { get; set; }

        [Required(ErrorMessage = "Loi {0}")]
        public string? Name { get; set; }

        public string? UserID { get; set; }
        public List<Users>? Users { get; set; }
    }
}
