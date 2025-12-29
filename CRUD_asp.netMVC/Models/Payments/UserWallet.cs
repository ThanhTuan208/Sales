using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD_asp.netMVC.Models.Payments
{
    public class UserWallet
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; } // tien kha dung

        [Column(TypeName = "decimal(18,2)")]
        public decimal LockedBalance { get; set; } // tien dang bi giu/khoa khong dung duoc (pending/refund)
        public DateTime UpdatedAt { get; set; }
    }
}
