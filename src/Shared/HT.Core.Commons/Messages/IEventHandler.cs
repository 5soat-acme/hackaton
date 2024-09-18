namespace HT.Core.Commons.Messages;

public interface IEventHandler<T> where T : Event
{
    Task Handle(T @event);
}