using System;
using System.Collections.Generic;
using System.Text;
using Shared.Events;

namespace Application.Common.Event;

public class EventNotification<TEvent>(TEvent @event) : INotification
    where TEvent : IEvent
{
    public TEvent Event { get; } = @event;
}
