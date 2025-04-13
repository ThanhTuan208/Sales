using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD_asp.netMVC.Models.Product
{
    public class Prototype

    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Loi {0}")]
        public string? Name { get; set; }

        public List<Products>? products { get; set; }
    }
}
