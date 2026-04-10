namespace CRUD_asp.netMVC.DTO.Payment
{
    public class SmsMessage
    {
        public string From { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string Sim { get; set; } = null!;
        public string Time { get; set; } = null!;
    }
}
