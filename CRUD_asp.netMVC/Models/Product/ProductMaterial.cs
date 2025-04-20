namespace CRUD_asp.netMVC.Models.Product
{
    public class ProductMaterial
    {
        public int ProductID { get; set; }
        public Products? Product { get; set; }

        public int MaterialID { get; set; }
        public Material? Material { get; set; }
    }
}
