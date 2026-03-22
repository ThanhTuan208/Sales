using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.DTO.Order.GHN;
using Microsoft.EntityFrameworkCore;

namespace CRUD_asp.netMVC.Service.GHN.Fil
{
    public class FilterData : IFilterData
    {
        private readonly AppDBContext _dbContext;
        private readonly IGenenricDataGHN _ghn;

        public FilterData(AppDBContext dbContext, IGenenricDataGHN ghn)
        {
            _dbContext = dbContext;
            _ghn = ghn;
        }

        public int? GetDistrictIdByDistrictGHN(string government)
        {
            var ward = GetWardGHNByGovernmentPara(government);
            if (ward.WardCode == null) return null;

            return _ghn.GenericGetDataGHN(_dbContext.DistrictGHN, p => new DistrictGHN()
            {
                Id = p.Id,
                DistrictID = p.DistrictID

            }).FirstOrDefault(p => p.Id == ward.DistrictId)?.DistrictID ?? null;
        }

        public string? GetDistrictNameByDistrictGHN(string government)
        {
            var ward = GetWardGHNByGovernmentPara(government);
            if (ward.WardCode == null) return null;

            return _ghn.GenericGetDataGHN(_dbContext.DistrictGHN, p => new DistrictGHN()
            {
                Id = p.Id,
                DistrictName = p.DistrictName

            }).FirstOrDefault(p => p.Id == ward.DistrictId)?.DistrictName ?? null;
        }

        public WardGHN GetWardGHNByGovernmentPara(string government)
        {
            return _ghn.GenericGetDataGHN(_dbContext.WardGHN, p => new WardGHN()
            {
                WardCode = p.WardCode,
                WardName = p.WardName,
                GovernmentCode = p.GovernmentCode,
                DistrictId = p.DistrictId,

            }).FirstOrDefault(p => p.GovernmentCode == government) ?? new WardGHN();
        }
    }
}
