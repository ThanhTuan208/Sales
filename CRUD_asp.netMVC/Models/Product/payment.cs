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
        public decimal? paidAmount { get; set; }

        public DateTime PaymentDate { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateOnly StartDay { get; private set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int StartMonth { get; private set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int StartYear { get; private set; }
    }
}
