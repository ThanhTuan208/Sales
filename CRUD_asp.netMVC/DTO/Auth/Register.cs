using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD_asp.netMVC.DTO.Auth
{
    public class Register
    {
        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
        [StringLength(50, ErrorMessage = "Tên đăng nhập không được vượt quá 50 ký tự")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên")]
        [StringLength(50, ErrorMessage = "Họ không được vượt quá 50 ký tự")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ")]
        [StringLength(50, ErrorMessage = "Tên không được vượt quá 50 ký tự")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại không đúng định dạng")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Vui lòng thêm vai trò")]
        public int RoleID { get; set; }    

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Mật khẩu phải từ {1} đến {2} ký tự")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[~!@#$%^&*()_+=?])[A-Za-z0-9~!@#$%^&*()_+=?]{8,20}$",
            ErrorMessage = "Mật khẩu phải từ 8-20 ký tự, bao gồm ít nhất 1 chữ hoa, 1 chữ thường, 1 số và 1 ký tự đặc biệt (~!@#$%^&*()_+=?).")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Nhập lại mật khẩu của bạn"), Compare("Password", ErrorMessage = "Mật khẩu không giống nhau !!!")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Ngày bắt đầu là bắt buộc")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; } = DateTime.Now;

        //[NotMapped]
        //public string InfoGeneral { get; set; }
    }
}
