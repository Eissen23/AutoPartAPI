using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.Common.Contracts;

public interface IEntity
{
    List<DomainEvent> DomainEvents { get; }
}

public interface IEntity<TId> : IEntity 
{
    TId Id { get; }
}

