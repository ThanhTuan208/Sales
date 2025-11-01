using CRUD_asp.netMVC.ViewModels.Admin;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace CRUD_asp.netMVC.Models.Product
{
    public class ProductQuantity : IProductQty
    {
        [Key]
        public int ProductID { get; set; }
        public Products? Product { get; set; }

        [Key]
        public int ColorID { get; set; }
        public Color? Color { get; set; }

        [Key]
        public int SizeID { get; set; }
        public Size? Size { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
