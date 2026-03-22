using CRUD_asp.netMVC.Data;

namespace CRUD_asp.netMVC.Service.Scopes
{
    public class ScopedExecutor : IScopedExecutor
    {
        private readonly IServiceProvider _serviceProvider;

        public ScopedExecutor(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

        // Create scope
        public async Task ExecuteWithScopeAsync(Func<AppDBContext, Task> action)
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDBContext>();

            await action(db);
        }
        public async Task<T> ExecuteWithScopeAsync<T>(Func<AppDBContext, Task<T>> action)
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDBContext>();

            return await action(db);
        }
    }
}
