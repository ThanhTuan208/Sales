using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD_asp.netMVC.Models.Product
{
    public class Brand
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên. "), StringLength(50)]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mô tả. "), Column(TypeName = "nvarchar(max)")]
        public string? Description { get; set; }

        [NotMapped]
        public IFormFile? Picture { get; set; }

        public string? PicturePath { get; set; }

        public List<Products>? products { get; set; }
    }
}
