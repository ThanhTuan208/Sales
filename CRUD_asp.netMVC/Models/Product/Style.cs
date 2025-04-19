using NuGet.ProjectModel;
using System.ComponentModel.DataAnnotations;

namespace CRUD_asp.netMVC.Models.Product
{
    public class Style
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Bạn cần nhập tên Style!")]
        public string? Name { get; set; }

        // Nếu có quan hệ nhiều-nhiều với Products:
        public List<ProductStyle>? ProductStyles { get; set; }
    }

}
