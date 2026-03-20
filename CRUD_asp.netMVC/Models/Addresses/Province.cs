namespace CRUD_asp.netMVC.Models.Addresses
{
    public class Province
    {
        public int Id { get; set; }
        public string ProvinceName { get; set; } = null!;
        public int ProvinceID { get; set; }
        public List<District> Districts { get; set; } = null!;
    }
}
