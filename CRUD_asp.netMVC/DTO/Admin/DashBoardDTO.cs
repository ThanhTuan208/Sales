namespace CRUD_asp.netMVC.DTO.Admin
{
    public record AmountInTodayDTO(decimal Amount, decimal changePercent) : BaseDashboardDTO(
        Title: $"Doanh thu hôm nay",
        IdValue: "today-revenues",
        Value: $"{Amount:N0} VNĐ",
        IdChangetext: "revenue-percents",
        ChangeText: changePercent == 0
                    ? "0%\nkhông có thay đổi"
                    : $"{(changePercent > 0 ? "+" : "")}{changePercent:F1}% so với hôm trước",
        ChangeClass: changePercent >= 0 ? "text-success" : "text-danger",
        Icon: "ni ni-money-coins",
        Gradient: "bg-gradient-primary shadow-primary"
    );

    public record TodayUserVisitorDTO(long uvCount, decimal changePercent) : BaseDashboardDTO(
        Title: "Truy cập hôm nay",
        IdValue: "uv",
        Value: uvCount.ToString("N0"),
        IdChangetext: "uv-percents",
        ChangeText: changePercent == 0
                    ? "0%\nkhông có thay đổi"
                    : $"{(changePercent > 0 ? "+" : "")}{changePercent:F1}% so với hôm qua",
        ChangeClass: changePercent >= 0 ? "text-success" : "text-danger",
        Icon: "ni ni-world",
        Gradient: "bg-gradient-danger shadow-danger"
    );

    public record TodayUserLoginDTO(long dauCount, decimal changePercent) : BaseDashboardDTO(
         Title: $"Đăng nhập hôm nay",
         IdValue: "dau",
         Value: dauCount.ToString("N0"),
         IdChangetext: "dau-percents",
         ChangeText: changePercent == 0
                      ? "0%\nkhông có thay đổi"
                      : $"{(changePercent > 0 ? "+" : "")}{changePercent:F1}% so với hôm qua",
         ChangeClass: changePercent >= 0 ? "text-success" : "text-danger",
         Icon: "ni ni-paper-diploma",
         Gradient: "bg-gradient-success shadow-success"
     );

    public record AmountInMonthDTO(decimal Amount, decimal changePercent) : BaseDashboardDTO(
         Title: $"Doanh thu tháng {DateTime.UtcNow.Month}",
         IdValue: "month-revenues",
         Value: $"{Amount:N0} VNĐ",
         IdChangetext: "month-revenue-percents",
         ChangeText: changePercent == 0
                     ? "0%\nkhông có thay đổi"
                     : $"{(changePercent > 0 ? "+" : "")}{changePercent:F1}% so với tháng trước",
         ChangeClass: changePercent >= 0 ? "text-success" : "text-danger",
         Icon: "ni ni-cart",
         Gradient: "bg-gradient-warning shadow-warning"
     );

}
