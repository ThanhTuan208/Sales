using System.Text.Json.Serialization;

namespace CRUD_asp.netMVC.DTO.Order.GHN
{
    public class CreateOrderData
    {
        [JsonPropertyName("order_code")]
        public string OrderCode { get; set; }
    }
}
