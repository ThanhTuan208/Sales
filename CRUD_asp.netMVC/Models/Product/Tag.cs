using CRUD_asp.netMVC.ViewModels.Admin;
using System.ComponentModel.DataAnnotations;

namespace CRUD_asp.netMVC.Models.Product
{
    public class Tag : IProductItemGeneral
    {
        [Key]
        public int ID { get; set; }

        [Required, StringLength(50, ErrorMessage = "Loi {0}")]
        public string? Name { get; set; }

        [Required, StringLength(50, ErrorMessage = "Loi {0}")]
        public string? Description { get; set; }

        public List<ProductTag>? ProductTag { get; set; }
    }
}
