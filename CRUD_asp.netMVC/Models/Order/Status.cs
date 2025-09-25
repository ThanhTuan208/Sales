using System.ComponentModel.DataAnnotations;

namespace CRUD_asp.netMVC.Models.Order
{
    public class Status
    {
        public string? Name { get; set; }

        public List<Status> StatusList = new List<Status>()
        {
            new Status { Name = "Pending" },
            new Status { Name = "Paid" },
            new Status { Name = "Processing" },
            new Status { Name = "Shipped" },
            new Status { Name = "Deliveried" },
            new Status { Name = "Cancelled" },
            new Status { Name = "Returned" },
            new Status { Name = "Failled" },
        };
    }
}
