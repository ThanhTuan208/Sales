namespace CRUD_asp.netMVC.Service.Payment.SiteVisitService
{
    public interface ISiteUserVisitService
    {
        Task IncreaseSiteVisit(HttpContext context);
        Task<int> GetTodayVisitSiteAsysnc();
    }
}
