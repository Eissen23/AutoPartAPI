using System;
using System.Collections.Generic;
using System.Text;
using Host.Events;

namespace Domain.Entities.Common.Contracts;

public abstract class DomainEvent : IEvent
{
    public DateTime TriggeredOn { get; protected set; } = DateTime.UtcNow;
}
