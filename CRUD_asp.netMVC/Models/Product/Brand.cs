using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD_asp.netMVC.Models.Product
{
    public class Brand
    {
        [Key]
        public int ID { get; set; }

        [Required, StringLength(50)]
        public string? Name { get; set; }

        [Required, Column(TypeName = "nvarchar(max)")]
        public string? Description { get; set; }

        public List<Products>? products { get; set; }
    }
}
