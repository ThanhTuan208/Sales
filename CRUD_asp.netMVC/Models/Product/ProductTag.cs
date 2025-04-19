using System.ComponentModel.DataAnnotations;

namespace CRUD_asp.netMVC.Models.Product
{
    public class ProductTag
    {
        public int ProductID { get; set; }
        public Products? Product { get; set; }

        public int TagID { get; set; }
        public Tag? Tag { get; set; }
    }
}
