namespace CRUD_asp.netMVC.Models.Product
{
    public class ProductSize
    {
        public int ProductID { get; set; }
        public Products? products { get; set; }

        public int SizeID { get; set; }
        public Size? size { get; set; }
    }
}
