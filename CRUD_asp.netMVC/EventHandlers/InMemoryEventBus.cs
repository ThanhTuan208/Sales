
using Microsoft.AspNetCore.Components.Forms.Mapping;
using Microsoft.Extensions.Logging;

namespace CRUD_asp.netMVC.EventHandlers
{
    public class InMemoryEventBus : IEventBus
    {
        private readonly IServiceProvider _provide;

        public InMemoryEventBus(IServiceProvider provide) => _provide = provide;

        public async Task PaymentVerificationAsync<TEvent>(TEvent @event) => await GeneralMethod(@event);

        public async Task PublishAsync<TEvent>(TEvent @event) => await GeneralMethod(@event);

        public async Task GeneralMethod<TEvent>(TEvent @event)
        {
            var scope = _provide.CreateScope();
            var handlers = scope.ServiceProvider.GetServices<IEventHandler<TEvent>>();

            foreach (var handler in handlers)
            {
                await handler.HandleAsync(@event);
            }
        }
    }
}
