using CRUD_asp.netMVC.Models.Product;
using CRUD_asp.netMVC.ViewModels.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD_asp.netMVC.ViewModels.Product
{
    public class ProductGeneralViewModel : IProductGeneralViewModel
    {
        [Required]
        public int ID { get; set; }

        [Required(ErrorMessage = "Nhập tên sản phẩm. "),
            StringLength(60, MinimumLength = 5, ErrorMessage = "Do dai ten tu {2} den {1} ky tu")]
        public string Name { get; set; }

        //[Required(ErrorMessage = "Chọn hình ânh. ")]
        public IFormFile[]? Picture { get; set; }

        public string? PicturePath { get; set; }

        [Required(ErrorMessage = "Nhập mô tả sản phẩm. "), Column(TypeName = "nvarchar(max)")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Nhập giá mới. "), DataType(DataType.Currency)]
        public double? NewPrice { get; set; }

        [Required(ErrorMessage = "Nhập giá cũ. "), DataType(DataType.Currency)]
        public double OldPrice { get; set; }

        [Required(ErrorMessage = "Nhập số lượng. ")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Chọn nổi bật. ")]
        public int? FeaturedID { get; set; }

        [Required(ErrorMessage = "Chọn giới tính. ")]
        public int? GenderID { get; set; }

        [Required(ErrorMessage = "Chọn thương hiệu. ")]
        public int? BrandID { get; set; }

        [Required(ErrorMessage = "Chọn phân loại. ")]
        public int? CateID { get; set; }

        [DataType(DataType.Date)]
        public DateTime Created { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Chọn chất liệu. ")]
        public int[] SelectedMaterialID { get; set; }

        //[Required(ErrorMessage = "Chọn màu sắc. ")]
        public int[]? SelectedColorID { get; set; }

        //[Required(ErrorMessage = "Chọn kích thước. ")]
        public int[]? SelectedSizeID { get; set; }

        [Required(ErrorMessage = "Chọn mùa. ")]
        public int[] SelectedSeasonID { get; set; }

        [Required(ErrorMessage = "Chọn kiểu dáng. ")]
        public int[] SelectedStyleID { get; set; }

        [Required(ErrorMessage = "Chọn nhãn dán. ")]
        public int[] SelectedTagID { get; set; }

        public SelectList? FeaturedList { get; set; }
        public SelectList? BrandList { get; set; }
        public SelectList? CategoryList { get; set; }
        public SelectList? GenderList { get; set; }
        public SelectList? MaterialList { get; set; }
        public SelectList? ColorList { get; set; }
        public SelectList? SizeList { get; set; }
        public SelectList? StyleList { get; set; }
        public SelectList? TagList { get; set; }
        public SelectList? SeasonList { get; set; }

        public List<TempProductQty> TempProductQty { get; set; } = new List<TempProductQty>();

        public List<ProductQuantity> ProductQty { get; set; } = new List<ProductQuantity>();
    }
}
