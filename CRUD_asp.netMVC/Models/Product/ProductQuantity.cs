using CRUD_asp.netMVC.ViewModels.Admin;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace CRUD_asp.netMVC.Models.Product
{
    [PrimaryKey(nameof(ProductID), nameof(ColorID), nameof(SizeID))]
    public class ProductQuantity : IProductQty
    {
        public int ProductID { get; set; }
        public Products? Product { get; set; }

        public int ColorID { get; set; }
        public Color? Color { get; set; }

        public int SizeID { get; set; }
        public Size? Size { get; set; }

        [Required]
        public int Quantity { get; set; }
    }

}
