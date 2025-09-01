using CRUD_asp.netMVC.Models.Auth;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Net;

namespace CRUD_asp.netMVC.Models.Product.Order
{
    public class Orders
    {
        [Key]
        public string ID { get; set; }

        [Required]
        public int UserID { get; set; }
        public Users? Users { get; set; }

        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Required]
        public string? Status { get; set; } = "Pending"; // Paid, failed, shipped, completed

        [Required]
        public string PaymentMethod { get; set; } = "Cash"; // or Transfer, credit card

        [Required]
        public string TransactionId { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime? PaidAt { get; set; }

        public List<OrderDetail>? OrderDetail { get; set; }
        public Payment? Payment { get; set; }
    }
}
