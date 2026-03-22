using CRUD_asp.netMVC.Data;

namespace CRUD_asp.netMVC.Service.Scopes
{
    public interface IScopedExecutor
    {
        Task ExecuteWithScopeAsync(Func<AppDBContext, Task> action);
        Task<T> ExecuteWithScopeAsync<T>(Func<AppDBContext, Task<T>> action);
    }
}
