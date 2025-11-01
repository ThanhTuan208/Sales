namespace CRUD_asp.netMVC.ViewModels.Admin
{
    public class TempProductQty : IProductQty
    {
        public int ProductID { get; set; }
        public int ColorID { get; set; }
        public int SizeID { get; set; }
        public int Quantity { get; set; }
    }
}
