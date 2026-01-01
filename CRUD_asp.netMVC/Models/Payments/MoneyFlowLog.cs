using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD_asp.netMVC.Models.Payments
{
    public class MoneyFlowLog
    {
        [Key]
        public string Id { get; set; } = null!;

        [Required]
        public int UserId { get; set; }

        [Required]
        public string OrderId { get; set; } = null!;

        public string RelatedId { get; set; } = null!;
        // ExcessPaymentId | RefundRequestId | UnderpaidOrderId

        [Required, MaxLength(50)]
        public string Type { get; set; } = null!;
        // ExcessCreated | RefundRequested | RefundApproved | Refunded | AutoRefund | UnderpaidCreated

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Column(TypeName = "decimal(18,2)")] 
        public decimal? PaidAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? BalanceSnapshot { get; set; }
        public bool AffectBalance { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(500)]
        public string? Description { get; set; }
    }
}
