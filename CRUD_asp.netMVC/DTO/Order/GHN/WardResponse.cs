namespace CRUD_asp.netMVC.DTO.Order.GHN
{
    public class WardResponse
    {
        public int code { get; set; }
        public string message { get; set; }
        public List<WardGHN> data { get; set; }
    }
}
