using CRUD_asp.netMVC.Models.Order;
using Microsoft.AspNetCore.Http.Features;
using System.Text.Json.Serialization;

namespace CRUD_asp.netMVC.DTO.Order.GHN
{
    public class CreateOrderGHNRequest // thong tin giao hang
    {
        [JsonPropertyName("to_name")]
        public string ToName { get; set; }

        [JsonPropertyName("to_phone")]
        public string ToPhone { get; set; }

        [JsonPropertyName("to_address")]
        public string ToAddress { get; set; }

        [JsonPropertyName("to_district_id")]
        public int ToDistrictID { get; set; }

        [JsonPropertyName("to_ward_code")]
        public string ToWardCode { get; set; }

        [JsonPropertyName("service_id")]
        public int ServiceID { get; set; }

        [JsonPropertyName("service_type_id")]
        public int ServiceTypeID { get; set; }

        [JsonPropertyName("weight")]
        public int? Weight { get; set; }

        [JsonPropertyName("cod_amount")]
        public int CodAmount { get; set; }

        [JsonPropertyName("payment_type_id")]
        public int PaymentTypeID { get; set; } // 1 nguoi nhan tra phi (COD), 2 nguoi gui tra

        [JsonPropertyName("note")]
        public string Note { get; set; }

        [JsonPropertyName("required_note")]
        public string RequiredNote { get; set; } = "KHONGCHOXEMHANG";

        [JsonPropertyName("shop_id")]
        public int ShopId { get; set; } // Thêm shop_id

        [JsonPropertyName("content")]
        public string Content { get; set; } = "Hàng hóa thông thường"; // Thêm mô tả

        //[JsonPropertyName("config_fee_id")]
        public int ConfigFeeID { get; set; }

        //[JsonPropertyName("extra_cost_id")]
        public int ExstraCodeID { get; set; }

        [JsonPropertyName("items")]
        public List<ProductItem> Items { get; set; }

        [JsonPropertyName("from_name")]
        public string FromName { get; set; }

        [JsonPropertyName("from_phone")]
        public string FromPhone { get; set; }

        [JsonPropertyName("from_address")]
        public string FromAddressUser { get; set; }

        [JsonPropertyName("from_district_id")]
        public int FromDistrictId { get; set; }

        [JsonPropertyName("from_ward_code")]
        public string FromWardCode { get; set; }

    }
}
