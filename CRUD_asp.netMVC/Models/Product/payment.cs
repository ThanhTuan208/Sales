using Microsoft.CodeAnalysis;
using Microsoft.Extensions.FileSystemGlobbing.Internal.PatternContexts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD_asp.netMVC.Models.Product
{
    public class Payment
    {
        [Key, Column(TypeName = "nvarchar(10)")]
        public string? ID { get; set; }

        [Required]
        public string? OrderID { get; set; }
        public Orders? Order { get; set; }

        public string paymentMethod { get; set; } = "Transfer"; // or Cash, credit card

        [DataType(DataType.Currency)]
        public double? paidAmount { get; set; }

        public DateTime PaymentDate { get; set; } 
    }
}
