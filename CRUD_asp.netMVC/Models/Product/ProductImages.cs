using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD_asp.netMVC.Models.Product
{
    public class ProductImages
    {
        [Key]
        public int ID { get; set; }
        public string? PathNameImage { get; set; }

        public int ProductID { get; set; }
        public Products? Products { get; set; }
    }
}
