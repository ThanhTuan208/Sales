using CRUD_asp.netMVC.Models.Product;
using System.ComponentModel.DataAnnotations;

namespace CRUD_asp.netMVC.Models.Account
{
    public class Staff : FieldGeneral
    {
        [Required, DataType(DataType.Date)]
        public DateTime Date { get; set; }

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
    