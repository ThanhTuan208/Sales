namespace CRUD_asp.netMVC.Models.Product
{
    public class ProductSeason
    {
        public int ProductID { get; set; }
        public Products? Product { get; set; }

        public int SeasonID { get; set; }
        public Season? Season { get; set; }
    }
}
