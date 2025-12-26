using CRUD_asp.netMVC.Models.Order;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CRUD_asp.netMVC.Models.Payments
{
    public class UnderpaidOrder
    {
        [Key]
        public string Id { get; set; } = null!;

        [Required]
        public int UserId { get; set; }

        [Required]
        public string OrderId { get; set; } = null!;

        [Column(TypeName = "decimal(18,2)")]
        public decimal? OrderAmount { get; set; } // So tien don hang

        [Column(TypeName = "decimal(18,2)")] 
        public decimal PaidAmount { get; set; } // So tien da tra

        [Column(TypeName = "decimal(18,2)")]
        public decimal MissingAmount { get; set; } // So tien thieu

        [Required, MaxLength(30)]
        public string Status { get; set; } = "Pending";
        // Pending | Paid | Cancelled | Expired

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public Orders Order { get; set; } = null!;
    }
}
