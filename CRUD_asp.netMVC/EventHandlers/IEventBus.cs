namespace CRUD_asp.netMVC.EventHandlers
{
    public interface IEventBus
    {
        Task PublishAsync<TEvent>(TEvent @event);
    }
}
