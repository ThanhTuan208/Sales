namespace CRUD_asp.netMVC.DTO.Address
{
    public class AddressDTO
    {
        public int ID { get; set; }
        //public int UserID { get; set; }
        public string RecipientName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string Province { get; set; } = null!;
        public string Ward { get; set; } = null!;
        public string GovernmentCode { get; set; } = null!;
        //public string PostalCode { get; set; }
        public bool IsDefault { get; set; } = false;
        public bool IsDelete { get; set; } = false;
    }
}
