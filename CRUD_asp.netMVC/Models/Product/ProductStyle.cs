namespace CRUD_asp.netMVC.Models.Product
{
    public class ProductStyle
    {
        public int ProductID { get; set; }
        public Products? Product { get; set; }

        public int StyleID { get; set; }
        public Style? Style { get; set; }
    }
}
