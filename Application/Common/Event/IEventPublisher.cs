using System;
using System.Collections.Generic;
using System.Text;
using Base.Application.Common.Interface;
using Shared.Events;

namespace Base.Application.Common.Event;

public interface IEventPublisher : ITransientService
{
    Task PublishAsync(IEvent @event);
}