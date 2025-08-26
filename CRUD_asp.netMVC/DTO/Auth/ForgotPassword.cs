using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CRUD_asp.netMVC.DTO.Auth
{
    public class ForgotPassword
    {
        [Required(ErrorMessage = "Vui lòng nhập Email để mã xác thực được gửi về cho bạn. ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mã Email. ")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới. ")]
        public string NewPass { get; set; }

        public string? InfoGeneral { get; set; }
    }
}
