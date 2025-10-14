using CRUD_asp.netMVC.ViewModels.Admin;
using System.ComponentModel.DataAnnotations;

namespace CRUD_asp.netMVC.Models.Product
{
    public class Size : IProductItemGeneral
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string? Name { get; set; }

        public List<ProductSize>? ProductSize { get; set; }
        public List<ProductQuantity>? ProductQty { get; set; }
    }
}
