using FSH.WebApi.Shared.Events;

namespace FSH.WebApi.Application.Common.Events;

public class EventNotification<TEvent>
    where TEvent : IEvent
{
    public EventNotification(TEvent @event) => Event = @event;

    public TEvent Event { get; }
}