using CRUD_asp.netMVC.ViewModels.Admin;
using System.ComponentModel.DataAnnotations;

namespace CRUD_asp.netMVC.Models.Product
{
    public class Style : IProductItemGeneral
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Bạn cần nhập tên Style!")]
        public string? Name { get; set; }

        // Nếu có quan hệ nhiều-nhiều với GetProducts:
        public List<ProductStyle>? ProductStyles { get; set; }
    }

}
