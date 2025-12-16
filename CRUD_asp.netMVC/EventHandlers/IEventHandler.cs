namespace CRUD_asp.netMVC.EventHandlers
{
    public interface IEventHandler<in TEvent>
    {
        Task HandleAsync(TEvent @event);
    }
}
