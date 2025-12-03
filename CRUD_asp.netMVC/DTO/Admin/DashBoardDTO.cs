namespace CRUD_asp.netMVC.DTO.Admin
{
    public record AmountInTodayDTO(decimal Amount, decimal ChangePercent) : BaseDashboardDTO(
        Title: $"Doanh thu hôm nay",
        Value: $"{Amount:N0} VND",
        ChangeText: ChangePercent == 0
                    ? "0%\nkhông có thay đổi"
                    : $"{(ChangePercent > 0 ? "+" : "")}{ChangePercent:F1}% so với hôm qua",
        ChangeClass: ChangePercent >= 0 ? "text-success" : "text-danger",
        Icon: "ni ni-money-coins",
        Gradient: "bg-gradient-primary shadow-primary"
    );
    public record TodayUsersDTO(int Count, decimal ChangePercent) : BaseDashboardDTO(
        Title: "Người dùng hôm nay",
        Value: Count.ToString("N0"),
        ChangeText: ChangePercent == 0
                    ? "0%\nkhông có thay đổi"
                    : $"{(ChangePercent > 0 ? "+" : "")}{ChangePercent:F1}% so với tuần trước",
        ChangeClass: ChangePercent >= 0 ? "text-success" : "text-danger",
        Icon: "ni ni-world",
        Gradient: "bg-gradient-danger shadow-danger"
    );

    public record NewUserByQuarterDTO(int Quarter, int UserCount) : BaseDashboardDTO(
         Title: $"Khách hàng mới - {GetQuarterName(Quarter)}",
         Value: UserCount >= 0 ? $"+{UserCount:N0}" : UserCount.ToString("N0"),
         ChangeText: UserCount == 0
                      ? "0%\nkhông có thay đổi"
                      : $"{(UserCount > 0 ? "+" : "")}{(UserCount * 0.1m):F1}% so với quý trước",
         ChangeClass: UserCount >= 0 ? "text-success" : "text-danger",
         Icon: "ni ni-paper-diploma",
         Gradient: "bg-gradient-success shadow-success"
     )
    {
        private static string GetQuarterName(int Quarter)
        {
            return Quarter switch
            {
                1 or 2 or 3 => "quý 1",
                4 or 5 or 6 => "quý 2",
                7 or 8 or 9 => "quý 3",
                10 or 11 or 12 => "quý 4",
                _ => "?"
            };
        }
    }
    public record AmountInMonthDTO(decimal Amount, decimal ChangePercent) : BaseDashboardDTO(
         Title: $"Doanh thu tháng {DateTime.UtcNow.Month}",
         Value: $"{Amount:N0} VND",
         ChangeText: ChangePercent == 0
                     ? "0%\nkhông có thay đổi"
                     : $"{(ChangePercent > 0 ? "+" : "")}{ChangePercent:F1}% so với tháng trước",
         ChangeClass: ChangePercent >= 0 ? "text-success" : "text-danger",
         Icon: "ni ni-cart",
         Gradient: "bg-gradient-warning shadow-warning"
     );

}
