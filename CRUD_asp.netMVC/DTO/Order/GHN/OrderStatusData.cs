namespace CRUD_asp.netMVC.DTO.Order.GHN
{
    public class OrderStatusData
    {
        public string OrderCode { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
        public DateTime? FinishedAt { get; set; }
    }
}
