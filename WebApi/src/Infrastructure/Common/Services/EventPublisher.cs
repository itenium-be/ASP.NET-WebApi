using FSH.WebApi.Application.Common.Events;
using FSH.WebApi.Shared.Events;
using Microsoft.Extensions.Logging;

namespace FSH.WebApi.Infrastructure.Common.Services;

public class EventPublisher : IEventPublisher
{
    private readonly ILogger<EventPublisher> _logger;

    public EventPublisher(ILogger<EventPublisher> logger) =>
        _logger = logger;

    public Task PublishAsync(IEvent @event)
    {
        _logger.LogInformation("Publishing Event : {event}", @event.GetType().Name);
        return Task.CompletedTask;
    }
}