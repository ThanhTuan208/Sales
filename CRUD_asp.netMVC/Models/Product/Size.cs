using System.ComponentModel.DataAnnotations;

namespace CRUD_asp.netMVC.Models.Product
{
    public class Size
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string? Name { get; set; }

        public List<ProductSize>? ProductSize { get; set; }
    }
}
