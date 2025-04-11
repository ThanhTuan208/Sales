using System.ComponentModel.DataAnnotations;

namespace CRUD_asp.netMVC.Models.Product
{
    public class Roles
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Loi {0}")]
        public string? Name { get; set; }

        public List<Users>? Users { get; set; }
    }
}
