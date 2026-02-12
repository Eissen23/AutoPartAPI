using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Interface;
using Shared.Events;

namespace Application.Common.Event;

public interface IEventPublisher : ITransientService
{
    Task PublishAsync(IEvent @event);
}