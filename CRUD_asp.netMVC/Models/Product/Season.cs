using CRUD_asp.netMVC.ViewModels.Admin;
using System.ComponentModel.DataAnnotations;

namespace CRUD_asp.netMVC.Models.Product
{
    public class Season : IProductItemGeneral
    {
        [Key]
        public int ID { get; set; }

        [Required, StringLength(50, ErrorMessage = "Loi {0}")]
        public string? Name { get; set; }

        public List<ProductSeason>? ProductSeason { get; set; }
    }
}
