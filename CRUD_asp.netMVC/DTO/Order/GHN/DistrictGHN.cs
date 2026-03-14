using StackExchange.Redis;

namespace CRUD_asp.netMVC.DTO.Order.GHN
{
    public class DistrictGHN
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public List<WardGHN> Wards { get; set; } = null!;
    }
}
