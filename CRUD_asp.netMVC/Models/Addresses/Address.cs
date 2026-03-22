using CRUD_asp.netMVC.Models.Auth;
using CRUD_asp.netMVC.Models.Order;
using System.ComponentModel.DataAnnotations;

namespace CRUD_asp.netMVC.Models.Addresses
{
    public class Address
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int UserID { get; set; }
        public Users User { get; set; } = null!;

        [StringLength(100)]
        [Required(ErrorMessage = "Tên người nhận không được để trống")]
        public string RecipientName { get; set; } = null!;

        [Phone]
        [StringLength(15)]
        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Địa chỉ chi tiết không được để trống")]
        [StringLength(255)]
        public string Street { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Province { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Ward { get; set; } = null!;

        [StringLength(50)]
        public string? PostalCode { get; set; }  // Ma buu chinh

        [StringLength(10)]
        public string GovernmentCode { get; set; } = null!;

        public bool IsDefault { get; set; }

        public bool IsDelete { get; set; }

        public List<Orders>? Orders { get; set; }
    }
}
