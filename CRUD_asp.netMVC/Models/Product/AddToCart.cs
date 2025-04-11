using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD_asp.netMVC.Models.Product
{
    public class AddToCart
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("Users")]
        [Required]
        public int? UserID { get; set; }
        public Users? Users { get; set; }

        [Required]
        public int? ProductID { get; set; }
        public Products? Product { get; set; }

        [Required]
        public int? Quantity { get; set; }

        public DateTime created_at { get; set; } = DateTime.Now;

    }
}
