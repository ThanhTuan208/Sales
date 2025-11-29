using CRUD_asp.netMVC.Models.Order;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD_asp.netMVC.Models.Product
{
    public class Payment
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [ForeignKey("Order")]
        public string OrderID { get; set; }
        public Orders? Order { get; set; }

        public string paymentMethod { get; set; }

        [DataType(DataType.Currency)]
        public double? paidAmount { get; set; }

        public DateTime PaymentDate { get; set; }
    }
}
