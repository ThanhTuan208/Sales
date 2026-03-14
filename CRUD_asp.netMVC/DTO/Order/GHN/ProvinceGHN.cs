namespace CRUD_asp.netMVC.DTO.Order.GHN
{
    public class ProvinceGHN
    {
        public int ProvinceID { get; set; }
        public string ProvinceName { get; set; } = null!;
        public string Code { get; set; } = null!;
        public List<string> NameExtension { get; set; } = null!;
    }
}
