using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CRUD_asp.netMVC.Models.Account;

namespace CRUD_asp.netMVC.Models.Product
{
    public class Reviews
    {
        [Key, Column(TypeName = "nvarchar(10)")]
        public int? ID { get; set; }

        [Required]
        public string? UserID { get; set; }
        public Users? Users { get; set; }

        [Required]
        public int? ProductID { get; set; }
        public Products? Product { get; set; }

        [Range(1, 5, ErrorMessage = "Loi {0}")]
        public int? Rating { get; set; }

        public string? Comment { get; set; }

        public DateTime ReviewDate { get; set; }
    }
}
