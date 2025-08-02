using CRUD_asp.netMVC.Models.Product;
using System.ComponentModel.DataAnnotations;

namespace CRUD_asp.netMVC.Models.Auth
{
    public class Staff : FieldGeneralRoles
    {
        [Required, DataType(DataType.Date)]
        public DateTime DayOfBirth { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        public enum WorkStatus
        {
            Working,
            OnLeave,
            Resigned
        }

        public WorkStatus Status { get; set; } = WorkStatus.Working;
    }
}
    