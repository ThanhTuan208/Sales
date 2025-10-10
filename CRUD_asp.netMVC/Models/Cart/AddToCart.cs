using CRUD_asp.netMVC.Models.Auth;
using CRUD_asp.netMVC.Models.Product;
using System.ComponentModel.DataAnnotations;

namespace CRUD_asp.netMVC.Models.Cart
{
    public class AddToCart
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int UserID { get; set; }
        public Users? Users { get; set; }

        [Required]
        public int ProductID { get; set; }
        public Products? Product { get; set; }

        [Required]
        public string? SelectColor { get; set; }

        [Required]
        public string? SelectSize { get; set; }

        [Required]
        public int Quantity { get; set; }

        public bool IsDelete { get; set; }

        public DateTime created_at { get; set; }
    }
}
