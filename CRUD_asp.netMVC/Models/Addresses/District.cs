namespace CRUD_asp.netMVC.Models.Addresses
{
    public class District
    {
        public int Id { get; set; }
        public string DistrictName { get; set; } = null!;
        public int GHNDistrictID { get; set; }
        public int ProvinceMappingId { get; set; }
        public Province Province { get; set; } = null!;
        public List<Ward> Wards { get; set; } = null!;
    }
}
