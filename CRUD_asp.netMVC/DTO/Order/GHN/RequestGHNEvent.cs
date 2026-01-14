namespace CRUD_asp.netMVC.DTO.Order.GHN
{
    public class RequestGHNEvent
    {
        public string OrderId { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string Ward { get; set; } = null!;
        public string Province { get; set; } = null!;
        public string ToWardCode { get; set; } = null!;
        public string RequiredNote { get; set; } = null!;
        public string FromName { get; set; } = null!;
        public string FromPhone { get; set; } = null!;
        public string FromAddressUser { get; set; } = null!;
        public string FromDistrictId { get; set; } = null!;
        public string FromWardCode { get; set; } = null!;

        public int ServiceTypeId { get; set; }
        public int ToDistrictId { get; set; }
        public int PaymentTypeId { get; set; } 
        public int ConfigFeeId { get; set; } 
        public int ExstraCodeId { get; set; } 
        public double Weight  { get; set; }
        public double CodAount { get; set; }

        
    }
}
