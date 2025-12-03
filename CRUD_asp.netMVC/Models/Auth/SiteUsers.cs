using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD_asp.netMVC.Models.Auth
{
    public class SiteUsers
    {
        public int ID { get; set; }
        public int TotalView { get; set; }

        [Column(TypeName = "date")] 
        public DateTime Date { get; set; }
    }
}
