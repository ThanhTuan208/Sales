using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD_asp.netMVC.Models.Payments
{
    public class RefundRequest
    {
        [Key]
        public string Id { get; set; } = null!;

        [Required]
        public string ExcessPaymentId { get; set; } = null!;

        [Required]
        public int UserId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal RequestAmount { get; set; }

        [MaxLength(100)]
        public string? BankName { get; set; }

        [MaxLength(50)]
        public string? BankAccount { get; set; }

        [MaxLength(150)]
        public string? BankOwner { get; set; }

        [Required, MaxLength(30)]
        public string Status { get; set; } = "Pending";
        // Pending | Approved | Rejected | Paid

        public DateTime RequestedAt { get; set; }

        public DateTime? ApprovedAt { get; set; }

        public DateTime? PaidAt { get; set; }

        public long? AdminId { get; set; }

        [MaxLength(500)]
        public string? AdminNote { get; set; }

        // Navigation
        [ForeignKey(nameof(ExcessPaymentId))]
        public ExcessPayment ExcessPayment { get; set; } = null!;
    }
}
