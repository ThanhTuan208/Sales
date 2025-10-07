using System.ComponentModel.DataAnnotations;

namespace CRUD_asp.netMVC.Models.Product
{
    public class Category
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Loi {0}")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Loi {0}")]
        public string? PicturePath { get; set; }

        public List<Products>? products { get; set; }
    }
}
