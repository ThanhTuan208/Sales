using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CRUD_asp.netMVC.DTO.Home
{
    public class MailContactDTO
    {
        [Required(ErrorMessage = "Vui lòng nhập tên")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Email"), EmailAddress(ErrorMessage = "Vui lòng nhập địa chỉ email hợp lệ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tiêu đề")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập nội dung tin nhắn")]
        [StringLength(1000, ErrorMessage = "Nội dung tin nhắn không được vượt quá 1000 ký tự")]
        public string Message { get; set; }
    }
}
