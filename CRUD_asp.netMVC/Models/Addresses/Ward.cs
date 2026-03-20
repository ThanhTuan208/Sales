using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace CRUD_asp.netMVC.Models.Addresses
{
    public class Ward
    {
        [Key]
        public int Id { get; set; }

        [StringLength(10)]
        public string WardCode { get; set; } = null!;
        public int DistrictId { get; set; }

        [StringLength(50)]
        public string WardName { get; set; } = null!;
        public District District { get; set; } = null!;
    }
}
