using CRUD_asp.netMVC.Models.Order;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD_asp.netMVC.Models.Payments
{
    /* Status properties
         Pending: Chưa xác nhận giao dịch
         Available: Có tiền dư, có thể hoàn
         RefundRequested: User yêu cầu hoàn
         Refunded: Admin đã hoàn
         Expired: Quá hạn xử lý
    */
    public class ExcessPayment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public string OrderId { get; set; } = null!;

        [Column(TypeName = "decimal(18,2)")]
        public decimal OriginalAmount { get; set; }   // Giá trị đơn hàng

        [Column(TypeName = "decimal(18,2)")]
        public decimal PaidAmount { get; set; }       // Số tiền user chuyển

        [Column(TypeName = "decimal(18,2)")]
        public decimal ExcessAmount { get; set; }     // Số tiền dư

        [Required, MaxLength(30)]
        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        [MaxLength(500)]
        public string? Note { get; set; }

        public Orders Order { get; set; } = null!;
        public ICollection<RefundRequest>? RefundRequests { get; set; }
    }

}
