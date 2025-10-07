using CRUD_asp.netMVC.Models.Order;
using System.ComponentModel.DataAnnotations;

namespace CRUD_asp.netMVC.Models.Product
{
    public class Payment
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string OrderID { get; set; }
        public Orders? Order { get; set; }

        public string paymentMethod { get; set; } = "Transfer"; // or Cash, credit card

        [DataType(DataType.Currency)]
        public double paidAmount { get; set; }

        public DateTime PaymentDate { get; set; }
    }
}
