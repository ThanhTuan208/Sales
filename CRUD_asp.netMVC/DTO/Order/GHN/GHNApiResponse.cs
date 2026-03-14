namespace CRUD_asp.netMVC.DTO.Order.GHN
{
    public class GHNApiResponse<T>
    {
        public int Code { get; set; }
        public string Message { get; set; } = null!;
        public T Data { get; set; }
    }
}
