using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CRUD_asp.netMVC.Models.ViewModels.Product
{
    public class ProductEditViewModel : IProductGeneralViewModel
    {
        [Required]
        public int ID { get; set; }

        [Required(ErrorMessage = "Ban can phai nhap Ten !!!"), StringLength(50, MinimumLength = 5, ErrorMessage = "Do dai ten tu {2} den {1} ky tu")]
        public string? Name { get; set; }

        public IFormFile[]? Picture { get; set; }

        public string? PicturePath { get; set; }

        [Required, Column(TypeName = "nvarchar(max)")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Ban can phai nhap {0} sp !!!")]
        [DataType(DataType.Currency)]
        public double Price { get; set; }

        [Required(ErrorMessage = "Ban can phai nhap {0} !!!")]
        public int Quantity { get; set; }

        [DataType(DataType.Date)]
        public DateTime Created { get; set; } = DateTime.Now;

        [Required]
        public int FeaturedID { get; set; }

        [Required]
        public int GenderID { get; set; }

        [Required]
        public int BrandID { get; set; }

        [Required]
        public int CateID { get; set; }

        public int[] SelectedMaterialID { get; set; }
        public int[] SelectedColorID { get; set; }
        public int[] SelectedSizeID { get; set; }
        public int[] SelectedStyleID { get; set; }
        public int[] SelectedSeasonID { get; set; }
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

        public List<string> ImagePaths { get; set; } = new List<string>();
    }
}
