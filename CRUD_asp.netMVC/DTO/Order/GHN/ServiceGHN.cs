using System.Text.Json.Serialization;

namespace CRUD_asp.netMVC.DTO.Order.GHN
{
    public class ServiceGHN
    {
        [JsonPropertyName("service_id")]
        public int Id { get; set; }

        [JsonPropertyName("service_type_id")]
        public int TypeId { get; set; }

        [JsonPropertyName("short_name")]
        public string ShortName { get; set; } = string.Empty;
    }
}
