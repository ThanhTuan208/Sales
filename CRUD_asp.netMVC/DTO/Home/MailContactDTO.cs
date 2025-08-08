using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CRUD_asp.netMVC.DTO.Home
{
    public class MailContactDTO
    {
        [Required(ErrorMessage = "tên")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "họ")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email"), EmailAddress(ErrorMessage = "địa chỉ email hợp lệ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "tiêu đề")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "nội dung tin nhắn")]
        [StringLength(3000, ErrorMessage = "nội dung tin nhắn không được vượt quá {1} ký tự")]
        public string Message { get; set; }
    }
}
