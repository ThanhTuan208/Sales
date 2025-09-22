namespace CRUD_asp.netMVC.DTO.Home
{
    public class UserProfileDTO
    {
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public IFormFile? ProfileImage { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
