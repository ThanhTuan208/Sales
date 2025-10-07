using Microsoft.AspNetCore.Mvc.Rendering;

namespace CRUD_asp.netMVC.ViewModels.Product
{
    public interface IProductGeneralViewModel
    {
        public int? GenderID { get; set; }
        public int? BrandID { get; set; }
        public int? CateID { get; set; }
        public int? FeaturedID { get; set; }

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
    }
}
