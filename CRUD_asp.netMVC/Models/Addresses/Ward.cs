namespace CRUD_asp.netMVC.Models.Addresses
{
    public class Ward
    {
        public int Id { get; set; }
        public string WardName { get; set; } = null!;
        public string GHNWardCode { get; set; } = null!;
        public int DistrictId { get; set; }
        public District District { get; set; } = null!;
    }
}
