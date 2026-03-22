using CRUD_asp.netMVC.Data;
using System.Linq.Expressions;

namespace CRUD_asp.netMVC.Service.GHN
{
    public interface IGenenricDataGHN
    {
        Task<Dictionary<TKey, TValue>> GetDictionaryAsync<TEntity, TKey, TValue>
                    (AppDBContext db, Func<TEntity, TKey> keySelector, Func<TEntity, TValue> valueSelector)
                    where TEntity : class where TKey : notnull;

        Task<List<TEntity>> GetListByApiGHNAsync<TDto, TEntity>(string api, Func<TDto, TEntity> mapFunc);

        IQueryable<TDto> GenericGetDataGHN<TEntity, TDto>(IQueryable<TEntity> query, Expression<Func<TEntity, TDto>> selector)
            where TEntity : class;
    }
}
