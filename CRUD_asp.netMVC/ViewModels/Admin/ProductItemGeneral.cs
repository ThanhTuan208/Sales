using CRUD_asp.netMVC.Models.Product;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CRUD_asp.netMVC.ViewModels.Admin
{
    public class ProductItemGeneral
    {
        public string? ValueType { get; set; }
        public int Qty { get; set;  }

        public List<Material>? Material { get; set; }
        public List<Color>? Color { get; set; }
        public List<Size>? Size { get; set; }
        public List<Season>? Season { get; set; }
        public List<Style>? Style { get; set; }
        public List<Tag>? Tag { get; set; }

        public List<ProductQuantity> ProductQty { get; set; }
        public List<TempProductQty> TempProductQty { get; set; }
        public List<IProductItemGeneral> Items { get; set; }

        public SelectList? SelectListColor { get; set; }
        public SelectList? SelectListSize { get; set; }
    }
}
