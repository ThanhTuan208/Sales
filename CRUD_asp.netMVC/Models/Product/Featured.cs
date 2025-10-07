using System.ComponentModel.DataAnnotations;

namespace CRUD_asp.netMVC.Models.Product
{
    public class Featured
    {
        [Key]
        public int ID { get; set; }

        [Required, StringLength(50)]
        public string? Name { get; set; }

        public List<Products>? products { get; set; }
    }
}
