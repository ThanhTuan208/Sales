namespace CRUD_asp.netMVC.ViewModels.Admin
{
    public interface IProductQty
    {
        int ProductID { get; set; }
        int ColorID { get; set; }
        int SizeID { get; set; }
        int Quantity { get; set; }
    }
}
