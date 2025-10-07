namespace CRUD_asp.netMVC.Models.Product
{
    public class ProductColors
    {
        public int ProductID { get; set; }
        public Products? Product { get; set; }

        public int ColorID { get; set; }
        public Color? Color { get; set; }
    }
}
