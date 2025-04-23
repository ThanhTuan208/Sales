using System.ComponentModel.DataAnnotations;

namespace CRUD_asp.netMVC.Models.Product
{
    public class Color
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string? Name { get; set; }

        public List<ProductColors>? ProductColor { get; set; }
    }
}
