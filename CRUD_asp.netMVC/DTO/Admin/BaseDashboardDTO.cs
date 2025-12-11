namespace CRUD_asp.netMVC.DTO.Admin
{
    public abstract record BaseDashboardDTO
    (
        string Title,
        string IdValue,
        string Value,
        string IdChangetext,
        string ChangeText,
        string ChangeClass,
        string Icon,
        string Gradient
    );
}
