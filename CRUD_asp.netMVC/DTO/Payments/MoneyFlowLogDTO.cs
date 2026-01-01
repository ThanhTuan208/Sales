namespace CRUD_asp.netMVC.DTO.Payments
{
    public class MoneyFlowLogDTO
    {
        public string Id { get; set; } = null!;
        public string User { get; set; } = null!;
        public string OrderId { get; set; } = null!;
        public string RelatedId { get; set; } = null!;
        public string Type { get; set; } = null!;
        public decimal Amount { get; set; }
        public decimal? AmountOrder { get; set; }
        public decimal? PaidAmount { get; set; }
        public decimal? MissingAmount { get; set; }
        public decimal? ExcessAmount { get; set; }
        public decimal? BalanceSnapshot { get; set; }
        public decimal? WalletBalance { get; set; }
        public DateTime CreatedAtRaw { get; set; }
        public string CreatedAt { get; set; } = null!;
        public string? Description { get; set; } = null!;
        public bool AffectBalance { get; set; }
    }
}
