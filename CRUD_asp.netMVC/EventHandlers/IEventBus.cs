namespace CRUD_asp.netMVC.EventHandlers
{
    public interface IEventBus
    {
        Task PublishAsync<TEvent>(TEvent @event);
        Task RequestGHNAsync<TEvent>(TEvent @event);
        Task PaymentVerificationAsync<TEvent>(TEvent @event);
    }
}
