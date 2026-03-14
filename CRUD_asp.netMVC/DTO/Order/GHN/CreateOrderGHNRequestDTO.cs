using CRUD_asp.netMVC.Models.Order;
using Microsoft.AspNetCore.Http.Features;
using System.Text.Json.Serialization;

namespace CRUD_asp.netMVC.DTO.Order.GHN
{
    public class CreateOrderGHNRequestDTO // thong tin giao hang
    {
        [JsonPropertyName("to_name")]
        public string ToName { get; set; } = null!;

        [JsonPropertyName("to_phone")]
        public string ToPhone { get; set; } = null!;

        [JsonPropertyName("to_address")]
        public string ToAddress { get; set; } = null!;

        [JsonPropertyName("to_ward_name")]
        public string ToWardName { get; set; } = null!;
        [JsonPropertyName("to_ward_code")]
        public string ToWardCode { get; set; } = null!;

        [JsonPropertyName("to_district_name")]
        public string ToDistrictName { get; set; } = null!;
        [JsonPropertyName("to_district_id")]
        public string ToDistrictID{ get; set; } = null!;
        [JsonPropertyName("to_province_name")]
        public string ToProvinceName { get; set; } = null!;

        [JsonPropertyName("service_id")]
        public int ServiceID { get; set; }

        [JsonPropertyName("service_type_id")]
        public int ServiceTypeID { get; set; }

        [JsonPropertyName("weight")]
        public int? Weight { get; set; }

        [JsonPropertyName("cod_amount")]
        public int CodAmount { get; set; }

        [JsonPropertyName("cod_failed_amount")]
        public int CodFailedAmount { get; set; }

        [JsonPropertyName("payment_type_id")]
        public int PaymentTypeID { get; set; } // 1 nguoi nhan tra phi (COD), 2 nguoi gui tra

        [JsonPropertyName("note")]
        public string Note { get; set; } = null!;

        [JsonPropertyName("required_note")]
        public string RequiredNote { get; set; } = "KHONGCHOXEMHANG";

        [JsonPropertyName("shop_id")]
        public int ShopId { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; } = "Hàng hóa thông thường";

        //[JsonPropertyName("config_fee_id")]
        public int ConfigFeeID { get; set; }

        //[JsonPropertyName("extra_cost_id")]
        public int ExstraCodeID { get; set; }

        [JsonPropertyName("items")]
        public List<ProductItem> Items { get; set; } = null!;

        [JsonPropertyName("from_name")]
        public string FromName { get; set; } = null!;

        [JsonPropertyName("from_phone")]
        public string FromPhone { get; set; } = null!;

        [JsonPropertyName("from_address")]
        public string FromAddressUser { get; set; } = null!;

        [JsonPropertyName("from_district_name")]
        public string FromDistrictName { get; set; } = null!;
        [JsonPropertyName("from_district_id")]
        public int FromDistrictID { get; set; }
        [JsonPropertyName("from_province_name")]
        public string FromProvinceName { get; set; } = null!;

        [JsonPropertyName("from_ward_name")]
        public string FromWardName { get; set; } = null!;
        [JsonPropertyName("from_ward_code")]
        public string FromWardCode { get; set; } = null!;

        [JsonPropertyName("pick_station_id")]
        public string PickStationId { get; set; } = null!;

    }
}
