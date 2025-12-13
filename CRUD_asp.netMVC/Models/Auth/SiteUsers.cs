using CRUD_asp.netMVC.DTO.Admin;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD_asp.netMVC.Models.Auth
{
    public class SiteUsers
    {
        public int ID { get; set; }
        public long UniqueVisitors { get; set; }
        public long DailyActiveUsers { get; set; }

        public decimal TodayAmounts { get; set; }    
        public decimal MonthAmounts { get; set; }    

        [Column(TypeName = "date")] 

        public DateTime Date { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
