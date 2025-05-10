using CRUD_asp.netMVC.Models.Product;
using System.ComponentModel.DataAnnotations;

namespace CRUD_asp.netMVC.Models.Cart
{
    public class UpdateToCart
    {
        [Required]
        public int productID { get; set; }
        public Products? product { get; set; }

        [Required]
        public required int qty { get; set; }
    }
}
