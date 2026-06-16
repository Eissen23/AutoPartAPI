using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Domain.Entities.Common.Contracts;

public interface IEntity
{
}

public interface IEntity<TId> : IEntity
{
    TId Id { get; }
}
