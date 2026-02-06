using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using MassTransit;
namespace Domain.Entities.Common.Contracts;


public abstract class BaseEntity : BaseEntity<DefaultIdType>
{
    protected BaseEntity() => Id = NewId.Next().ToGuid();
}

public abstract class BaseEntity<TId> : IEntity<TId>
{
    public TId Id { get; protected set; } = default!;

    [NotMapped]
    public List<DomainEvent> DomainEvents => throw new NotImplementedException();
}

