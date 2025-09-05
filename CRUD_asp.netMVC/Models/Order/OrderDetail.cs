using CRUD_asp.netMVC.Models.Product;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD_asp.netMVC.Models.Order
{
    public class OrderDetail
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string OrderID { get; set; }
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
