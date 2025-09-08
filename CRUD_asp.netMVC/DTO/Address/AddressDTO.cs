namespace CRUD_asp.netMVC.DTO.Address
{
    public class AddressDTO
    {
        public int ID { get; set; }
        //public int UserID { get; set; }
        public string RecipientName { get; set; }
        public string PhoneNumber { get; set; }
        public string Street { get; set; }
        public string Province { get; set; }
        public string Ward { get; set; }
        //public string PostalCode { get; set; }
        public bool IsDefault { get; set; } = false;
    }
}
