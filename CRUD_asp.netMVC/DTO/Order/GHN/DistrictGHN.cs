using StackExchange.Redis;

namespace CRUD_asp.netMVC.DTO.Order.GHN
{
    public class DistrictGHN
    {
        public int Id { get; set; }
        public int DistrictID { get; set; }
        public int ProvinceID { get; set; }
        public string DistrictName { get; set; } = null!;
        public List<WardGHN> Wards { get; set; } = null!;
    }
}
