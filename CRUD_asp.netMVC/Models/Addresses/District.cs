using System.ComponentModel.DataAnnotations;

namespace CRUD_asp.netMVC.Models.Addresses
{
    public class District
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string DistrictName { get; set; } = null!;
        public int DistrictID { get; set; }
        public int ProvinceId { get; set; }
        public Province Province { get; set; } = null!;
        public List<Ward> Wards { get; set; } = null!;
    }
}
