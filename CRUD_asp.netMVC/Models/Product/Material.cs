using System.ComponentModel.DataAnnotations;

namespace CRUD_asp.netMVC.Models.Product
{
    public class Material
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string? Name { get; set; }

        public List<ProductMaterial>? ProductMaterial { get; set; }
    }
}
