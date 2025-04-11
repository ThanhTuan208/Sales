using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Net;

namespace CRUD_asp.netMVC.Models.Product
{
    public class Orders
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int? UserID { get; set; }
        public Users? Users { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public string? Status { get; set; } = "Pending";

        [DataType(DataType.Currency)]
        public double totalPrice { get; set; }

        public List<OrderDetail>? OrderDetail { get; set; }
        public Payment? Payment { get; set; }
    }
}
