using System.ComponentModel.DataAnnotations;

namespace CRUD_asp.netMVC.Models.Payments
{
    public class UserWallet
    {
        [Key]
        public int UserId { get; set; }
        public decimal Balance { get; set; } // tien kha dung
        public decimal LockedBalance { get; set; } // tien dang bi giu/khoa khong dung duoc (pending/refund)
        public DateTime UpdatedAt { get; set; }
    }

}
