using CRUD_asp.netMVC.Models.Order;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD_asp.netMVC.Models.Auth
{
    public class Address
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int UserID { get; set; }
        public Users? User { get; set; }

        [Required(ErrorMessage = "Tên người nhận không được để trống")]
        [StringLength(100)]
        public string RecipientName { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [Phone]
        [StringLength(15)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Địa chỉ chi tiết không được để trống")]
        [StringLength(255)]
        public string Street { get; set; }

        [Required]
        [StringLength(100)]
        public string FullLocation { get; set; } // chứa City + District + Ward

        [StringLength(50)]
        public string PostalCode { get; set; } // Ma buu chinh

        public bool IsDefault { get; set; } = false; // Địa chỉ mặc định

        public List<Orders>? Orders { get; set; }
    }
}
