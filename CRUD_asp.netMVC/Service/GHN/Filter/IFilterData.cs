using CRUD_asp.netMVC.DTO.Order.GHN;

namespace CRUD_asp.netMVC.Service.GHN.Fil
{
    public interface IFilterData
    {
        WardGHN GetWardGHNByGovernmentPara(string government);
        string? GetDistrictNameByDistrictGHN(string government);
        int? GetDistrictIdByDistrictGHN(string government);
    }
}
