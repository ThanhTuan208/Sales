using CRUD_asp.netMVC.DTO.Order.GHN;
using CRUD_asp.netMVC.Models.Addresses;
using CRUD_asp.netMVC.Service.GHN;
using CRUD_asp.netMVC.Service.Scopes;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace CRUD_asp.netMVC.Data.Seed
{
    public class DbInitializer
    {
        private readonly AppDBContext _dbContext;
        private readonly IScopedExecutor _scoped;
        private readonly IGenenricDataGHN _seedGHN;
        private readonly ILogger<DbInitializer> _logger;
            
        public DbInitializer(ILogger<DbInitializer> logger, IGenenricDataGHN seedGHN, AppDBContext dbContext, IScopedExecutor scoped)
        {
            _logger = logger;
            _seedGHN = seedGHN;
            _dbContext = dbContext;
            _scoped = scoped;
        }

        public async Task SeedAddressesAsync()
        {
            await SeedProvinceAsync(_dbContext);
            await SeedDistrictAsync(_dbContext);
        }
        private async Task SeedWardAsync(AppDBContext _dbContext, int districtId)
        {
            try
            {
                List<Ward> wards = await GetWardListByApiGHN($"master-data/ward?district_id={districtId}");

                if (!wards.Any()) return;

                var districtDict = await _seedGHN.GetDictionaryAsync<District, int, int>(
                    _dbContext,
                    p => p.DistrictID,
                    p => p.Id);

                var wardCodes = await _dbContext.WardGHN.AsNoTracking()
                                                        .Where(p => p.DistrictId == districtDict[districtId])
                                                        .Select(p => p.WardCode)
                                                        .ToHashSetAsync();

                var newWardsByDistricts = wards.Where(p => !wardCodes.Contains(p.WardCode)).ToList();

                if (!newWardsByDistricts.Any()) return;

                await _dbContext.BulkInsertAsync(newWardsByDistricts, new BulkConfig
                {
                    BatchSize = 5000,
                    BulkCopyTimeout = 0,
                    TrackingEntities = false,
                    PreserveInsertOrder = false
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when seeding ward for districtId {districtId}", districtId);
            }
        }

        private async Task SeedDistrictAsync(AppDBContext _dbContext)
        {
            try
            {
                List<District> districts = await GetDistrictListByApiGHN("master-data/district");

                if (!districts.Any()) return;

                var setHashdistrictIds = await _dbContext.DistrictGHN.AsNoTracking()
                                                                    .Select(p => p.DistrictID)
                                                                    .ToHashSetAsync();

                var newDistricts = districts.Where(p => !setHashdistrictIds.Contains(p.DistrictID)).ToList();

                if (!newDistricts.Any()) return;

                await _dbContext.BulkInsertAsync(newDistricts, new BulkConfig
                {
                    BatchSize = 5000,
                    BulkCopyTimeout = 0,
                    TrackingEntities = false,
                    PreserveInsertOrder = false
                });

                int batchSize = 20;
                var semaphore = new SemaphoreSlim(5);
                var districtIdsNeedSeed = await _dbContext.DistrictGHN
                    .Where(d => !_dbContext.WardGHN.Any(w => w.DistrictId == d.Id))
                    .Select(d => d.DistrictID)
                    .ToListAsync();

                var batches = districtIdsNeedSeed
                    .Select((id, index) => new { id, index })
                    .GroupBy(x => x.index / batchSize)
                    .Select(g => g.Select(x => x.id).ToList())
                    .ToList();

                var tasks = batches.Select(async batch =>
                {
                    await semaphore.WaitAsync();
                    try
                    {
                        await _scoped.ExecuteWithScopeAsync(async db =>
                        {
                            foreach (var districtId in batch)
                            {
                                await SeedWardAsync(db, districtId);
                            }
                        });
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                });

                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when seeding districtId");
            }
        }

        private async Task SeedProvinceAsync(AppDBContext _dbContext)
        {
            try
            {
                List<Province> provinces = await GetProvinceListByApiGHN("master-data/province");

                if (!provinces.Any()) return;

                var provinceIds = await _dbContext.ProvinceGHN.Select(p => p.ProvinceID).ToHashSetAsync();

                var newProvinces = provinces.Where(p => !provinceIds.Contains(p.ProvinceID)).ToList();

                if (!newProvinces.Any()) return;

                await _dbContext.BulkInsertAsync(newProvinces, new BulkConfig
                {
                    BatchSize = 5000,
                    BulkCopyTimeout = 0,
                    TrackingEntities = false,
                    PreserveInsertOrder = false
                });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error when seeding province");
            }
        }

        private async Task<List<Ward>> GetWardListByApiGHN(string api)
        {
            return await _scoped.ExecuteWithScopeAsync(async db =>
            {
                var districtDict = await _seedGHN.GetDictionaryAsync<District, int, int>(
                    db,
                    p => p.DistrictID,
                    p => p.Id);

                var wards = await _seedGHN.GetListByApiGHNAsync<WardGHN, Ward>(api, (dto) =>
                {
                    if (!districtDict.ContainsKey(dto.DistrictId))
                        return default!;

                    return new Ward()
                    {
                        WardName = dto.WardName,
                        WardCode = dto.WardCode,
                        GovernmentCode = dto.GovernmentCode,
                        DistrictId = districtDict[dto.DistrictId]
                    };
                });

                return wards.Where(p => p != null).ToList();
            });
        }

        private async Task<List<District>> GetDistrictListByApiGHN(string api)
        {
            return await _scoped.ExecuteWithScopeAsync(async db =>
            {
                var provinceDict = await _seedGHN.GetDictionaryAsync<Province, int, int>(
                    db,
                    p => p.ProvinceID,
                    p => p.Id);

                var districts = await _seedGHN.GetListByApiGHNAsync<DistrictGHN, District>(api, (dto) =>
                {
                    if (!provinceDict.ContainsKey(dto.ProvinceID))
                        return null!;

                    return new District()
                    {
                        DistrictName = dto.DistrictName,
                        DistrictID = dto.DistrictID,
                        ProvinceId = provinceDict[dto.ProvinceID],
                    };
                });

                return districts.Where(x => x != null).ToList();
            });
        }

        private async Task<List<Province>> GetProvinceListByApiGHN(string api)
        {
            return await _seedGHN.GetListByApiGHNAsync<ProvinceGHN, Province>(api, (dto) => new Province()
            {
                ProvinceName = dto.ProvinceName,
                ProvinceID = dto.ProvinceID,
            });
        }
    }
}
