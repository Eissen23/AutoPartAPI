using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Event;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Events;

namespace Infrastructure.Common.Services;

public class EventPublisher (
        ILogger<EventPublisher> logger,
        IPublisher mediator
    ) : IEventPublisher
{
    private readonly ILogger<EventPublisher> _logger = logger;
    private readonly IPublisher _publisher = mediator;

    public Task PublishAsync(IEvent @event)
    {
        _logger.LogInformation("Publishing event of type {EventType}", @event.GetType().Name);
        return _publisher.Publish(CreateEventNotification(@event));
    }

    private static INotification CreateEventNotification(IEvent @event) =>
       (INotification)Activator.CreateInstance(
           typeof(EventNotification<>).MakeGenericType(@event.GetType()), @event)!;
}
