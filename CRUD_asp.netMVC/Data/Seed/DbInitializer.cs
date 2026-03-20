using AspNetCoreGeneratedDocument;
using Azure;
using CRUD_asp.netMVC.Controllers;
using CRUD_asp.netMVC.DTO.Order.GHN;
using CRUD_asp.netMVC.Migrations;
using CRUD_asp.netMVC.Models.Addresses;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Identity.Client;
using Org.BouncyCastle.Asn1.TeleTrust;
using System;
using System.Net.Http;

namespace CRUD_asp.netMVC.Data.Seed
{
    public class DbInitializer
    {
        private readonly ILogger<DbInitializer> _logger;
        private readonly HttpClient _httpClient;
        private readonly IServiceProvider _serviceProvider;

        public DbInitializer(IHttpClientFactory factory, IServiceProvider serviceProvider, ILogger<DbInitializer> logger)
        {
            _logger = logger;
            _httpClient = factory.CreateClient("GHN");
            _serviceProvider = serviceProvider;
        }

        public async Task SeedAddressesAsync(AppDBContext _dbContext)
        {
            using var transactions = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                await SeedProvinceAsync(_dbContext);
                await SeedDistrictAsync(_dbContext);

                await transactions.CommitAsync();
            }
            catch (Exception ex)
            {
                await transactions.RollbackAsync();
                _logger.LogError(ex, "Error transaction database");
            }
        }
        private async Task SeedWardAsync(AppDBContext _dbContext, int districtId)
        {
            try
            {
                List<Ward> wards = await GetWardListByApiGHN($"master-data/ward?district_id={districtId}");

                if (!wards.Any()) return;

                var districtDict = await GetDictionaryAsync<District, int, int>(
                    _dbContext,
                    p => p.DistrictID,
                    p => p.Id);

                var wardCodes = await _dbContext.WardGHN.Where(p => p.DistrictId == districtDict[districtId])
                                                        .Select(p => p.WardCode)
                                                        .ToHashSetAsync();

                var newWardsByDistricts = wards.Where(p => !wardCodes.Contains(p.WardCode)).ToList();

                if (!newWardsByDistricts.Any()) return;

                await _dbContext.WardGHN.AddRangeAsync(newWardsByDistricts);
                await _dbContext.SaveChangesAsync();
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

                var setHashdistrictIds = await _dbContext.DistrictGHN.Select(p => p.DistrictID).ToHashSetAsync();

                var newDistricts = districts.Where(p => !setHashdistrictIds.Contains(p.DistrictID)).ToList();

                if (newDistricts.Any())
                {
                    await _dbContext.DistrictGHN.AddRangeAsync(newDistricts);
                    await _dbContext.SaveChangesAsync();
                }

                var semaphore = new SemaphoreSlim(5);
                var districtIdsNeedSeed = await _dbContext.DistrictGHN
                    .Where(d => !_dbContext.WardGHN.Any(w => w.DistrictId == d.Id))
                    .Select(d => d.DistrictID)
                    .ToListAsync();

                var tasks = districtIdsNeedSeed.Select(async districtId =>
                {
                    await semaphore.WaitAsync();
                    try
                    {
                        await ExecuteWithScopeAsync(async db =>
                        {
                            await SeedWardAsync(db, districtId);
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

                await _dbContext.ProvinceGHN.AddRangeAsync(newProvinces);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error when seeding province");
            }
        }

        private async Task<List<Ward>> GetWardListByApiGHN(string api)
        {
            return await ExecuteWithScopeAsync(async db =>
            {
                var districtDict = await GetDictionaryAsync<District, int, int>(
                    db,
                    p => p.DistrictID,
                    p => p.Id,
                    true);

                var wards = await GetListByApiGHN<WardGHN, Ward>(api, (dto) =>
                {
                    if (!districtDict.ContainsKey(dto.DistrictId))
                        return default!;

                    return new Ward()
                    {
                        WardName = dto.WardName,
                        WardCode = dto.WardCode,
                        DistrictId = districtDict[dto.DistrictId]
                    };
                });

                return wards.Where(p => p != null).ToList();
            });
        }

        private async Task<List<District>> GetDistrictListByApiGHN(string api)
        {
            return await ExecuteWithScopeAsync(async db =>
             {
                 var provinceDict = await GetDictionaryAsync<Province, int, int>(
                     db,
                     p => p.ProvinceID,
                     p => p.Id);

                 var districts = await GetListByApiGHN<DistrictGHN, District>(api, (dto) =>
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
            return await GetListByApiGHN<ProvinceGHN, Province>(api, (dto) => new Province()
            {
                ProvinceName = dto.ProvinceName,
                ProvinceID = dto.ProvinceID,
            });
        }

        // Create scope appDbContext
        private async Task ExecuteWithScopeAsync(Func<AppDBContext, Task> action)
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDBContext>();

            await action(db);
        }
        private async Task<T> ExecuteWithScopeAsync<T>(Func<AppDBContext, Task<T>> action)
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDBContext>();

            return await action(db);
        }

        // Convert para of type dictionary
        // neu su dung 1 key 1 value Dictionary<TKey, TValue>
        private async Task<Dictionary<TKey, TValue>> GetDictionaryAsync<TEntity, TKey, TValue>
            (AppDBContext db, Func<TEntity, TKey> keySelector, Func<TEntity, TValue> valueSelector, bool allowMultipleValues = false)
            where TEntity : class where TKey : notnull
        {
            return await db.Set<TEntity>()
                        .AsNoTracking()
                        .ToDictionaryAsync(keySelector, valueSelector);
        }

        // Generic method
        private async Task<List<TEntity>> GetListByApiGHN<TDto, TEntity>(string api, Func<TDto, TEntity> mapFunc)
        {
            var res = await _httpClient.GetAsync(api);
            res.EnsureSuccessStatusCode();

            var result = await res.Content.ReadFromJsonAsync<GHNApiResponse<List<TDto>>>();

            if (result == null || result.Data == null || !result.Data.Any())
                return new List<TEntity>();

            return result.Data.Select(dto => mapFunc(dto)).ToList();
        }
    }
}
