using CRUD_asp.netMVC.Models.Product.Order;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD_asp.netMVC.Models.Product
{
    public class OrderDetail
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int OrderID { get; set; }
        public Orders? Orders { get; set; }

        [Required]
        public int ProductID { get; set; }
        public Products? Product { get; set; }

        [Required]
        public int Quantity { get; set; }

        [DataType(DataType.Currency)]
        public double Price { get; set; }

    }
}
