using CRUD_asp.netMVC.Models.Account;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Net;

namespace CRUD_asp.netMVC.Models.Product
{
    public class Orders
    {
        [Key, Column(TypeName = "nvarchar(10)")]
        public string? ID { get; set; }

        [Required]
        public string? UserID { get; set; }
        public Users? Users { get; set; }

        public DateTime OrderDate { get; set; }

        public string? Status { get; set; } = "Pending";

        [DataType(DataType.Currency)]
        public double totalPrice { get; set; }

        public List<OrderDetail>? OrderDetail { get; set; }
        public Payment? Payment { get; set; }
    }
}
