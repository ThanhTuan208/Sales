using CRUD_asp.netMVC.Models.Product;

namespace CRUD_asp.netMVC.DTO.Cart
{
    public sealed record BuyNowData
    {
        public int? ProductId { get; init; }
        public string? Color { get; init; } = null!;
        public string? Size { get; init; } = null!;
        public int Quantity{ get; init; }
        public double? Price { get; init; }
        public Products? Product { get; init; } = null!;
        public DateTime Expired { get; init; }
    }
}
