using CRUD_asp.netMVC.Models.Auth;
using CRUD_asp.netMVC.Models.Product;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Security.Permissions;

namespace CRUD_asp.netMVC.Models.Order
{
    public class Orders
    {
        [Key]
        public string ID { get; set; }

        [Required]
        public int UserID { get; set; }
        public Users? Users { get; set; }

        public int? AddressID { get; set; }
        public Address? Address { get; set; }

        [DataType(DataType.Currency)]
        public double Amount { get; set; }

        [Required]
        public string? Status { get; set; } = "Pending"; // Paid, failed, shipped, completed

        [Required]
        public string PaymentMethod { get; set; } // or Transfer, credit card

        [Required]
        public string TransactionId { get; set; }

        public string TrackingNumber { get; set; } // Ma van don

        public DateTime OrderDate { get; set; }

        public DateTime? PaidAt { get; set; }

        public DateTime? StatusTime { get; set; }

        [StringLength(50)]
        public string? ShipRecipientName { get; set; }

        [StringLength(50)]
        public string? ShipPhoneNumber { get; set; }

        [StringLength(50)]
        public string? ShipStreet { get; set; }

        [StringLength(50)]
        public string? ShipProvince { get; set; }

        [StringLength(50)]
        public string? ShipWard { get; set; }

        [StringLength(50)]
        public string? ShipPostalCode { get; set; }

        public Payment? Payment { get; set; }

        public List<OrderDetail>? OrderDetail { get; set; }
    }
}
