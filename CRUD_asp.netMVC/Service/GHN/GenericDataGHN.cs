using CRUD_asp.netMVC.Data;
using CRUD_asp.netMVC.DTO.Order.GHN;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CRUD_asp.netMVC.Service.GHN.Common
{
    public class GenericDataGHN : IGenenricDataGHN
    {
        private readonly HttpClient _httpClient;

        public GenericDataGHN(IHttpClientFactory factory) => _httpClient = factory.CreateClient("GHN");

        public IQueryable<TDto> GenericGetDataGHN<TEntity, TDto>(IQueryable<TEntity> query, Expression<Func<TEntity, TDto>> selector) where TEntity : class
        {
            return query.AsNoTracking().Select(selector);
        }

        // Convert para of type dictionary for Address api
        public async Task<Dictionary<TKey, TValue>> GetDictionaryAsync<TEntity, TKey, TValue>(AppDBContext db, Func<TEntity, TKey> keySelector, Func<TEntity, TValue> valueSelector)
            where TEntity : class
            where TKey : notnull
        {
            return await db.Set<TEntity>()
            .AsNoTracking()
            .ToDictionaryAsync(keySelector, valueSelector);
        }

        // Generic method get list data by api GHN
        public async Task<List<TEntity>> GetListByApiGHNAsync<TDto, TEntity>(string api, Func<TDto, TEntity> mapFunc)
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
