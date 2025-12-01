namespace CRUD_asp.netMVC.DTO.Admin
{
    public abstract record BaseDashboardDTO
    (
        string Title,
        string Value,
        string ChangeText,
        string ChangeClass,
        string Icon,
        string Gradient
    );
}
