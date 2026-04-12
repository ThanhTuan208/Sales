using CRUD_asp.netMVC.Models.Product;
using MimeKit.Tnef;

namespace CRUD_asp.netMVC.DTO.Cart
{
    public sealed record CartValidationResult
    {
        public int? ProductId { get; set; }
        public string? Color { get; set; } = null!;
        public string? Size { get; set; } = null!;
        public int Quantity{ get; set; }
        public double? Price { get; set; }
        public Products? Product { get; set; } = null!;
    }
}
