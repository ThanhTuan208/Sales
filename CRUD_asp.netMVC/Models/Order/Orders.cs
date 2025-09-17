using CRUD_asp.netMVC.Models.Auth;
using CRUD_asp.netMVC.Models.Product;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;

namespace CRUD_asp.netMVC.Models.Order
{
    public class Orders
    {
        [Key]
        public string ID { get; set; }

        [Required]
        public int UserID { get; set; }
        public Users? Users { get; set; }

        [Required]
        public int AddressID { get; set; }
        public Address? Address { get; set; }

        [DataType(DataType.Currency)]
        public double Amount { get; set; }

        [Required]
        public string? Status { get; set; } = "Pending"; // Paid, failed, shipped, completed

        [Required]
        public string PaymentMethod { get; set; } = "Cash"; // or Transfer, credit card

        [Required]
        public string TransactionId { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime? PaidAt { get; set; }
        public Payment? Payment { get; set; }

        public List<OrderDetail>? OrderDetail { get; set; }
    }
}
