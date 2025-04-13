using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD_asp.netMVC.Models.Product
{
    public class Manufacturer
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string? Name { get; set; }

        public List<Products>? products { get; set; }
    }
}
