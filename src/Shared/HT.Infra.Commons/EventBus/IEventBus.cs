using HT.Core.Commons.Messages;

namespace HT.Infra.Commons.EventBus;

public interface IEventBus
{
    void Subscribe<T>(IEventHandler<T> handler) where T : Event;
    Task Publish<T>(T @event) where T : Event;
}