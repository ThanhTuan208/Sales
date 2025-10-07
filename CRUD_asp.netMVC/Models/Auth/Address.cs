using CRUD_asp.netMVC.Models.Order;
using System.ComponentModel.DataAnnotations;

namespace CRUD_asp.netMVC.Models.Auth
{
    public class Address
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int UserID { get; set; }
        public Users? User { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Tên người nhận không được để trống")]
        public string RecipientName { get; set; }

        [Phone]
        [StringLength(15)]
        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Địa chỉ chi tiết không được để trống")]
        [StringLength(255)]
        public string Street { get; set; }

        [Required]
        [StringLength(100)]
        public string Province { get; set; }

        [Required]
        [StringLength(100)]
        public string Ward { get; set; }

        [StringLength(50)]
        public string? PostalCode { get; set; }  // Ma buu chinh

        public bool IsDefault { get; set; }

        public bool IsDelete { get; set; } 

        public List<Orders>? Orders { get; set; }
    }
}
