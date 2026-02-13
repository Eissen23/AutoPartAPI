using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.Identity;

public class Department : AuditableEntity, IAggregateRoot
{

    public string Name { get; protected set; } = default!;
    public string Description { get; protected set; } = default!;

    public DefaultIdType? ParentId { get; protected set; }

    // Navigation Properties
    public Department? Parent { get; protected set; }

    public Department()
    {
    }

    public Department(string name, string description, DefaultIdType? parentId)
    {
        Name = name;
        Description = description;
        ParentId = parentId;
    }

    public void Update(string? name, string? description, Guid? parentId)
    {
        if(name is not null && Name?.Equals(name) is not true)
        {
            Name = name;
        }
        if(description is not null && Description?.Equals(description) is not true)
        {
            Description = description;
        }
        if(parentId is not null && ParentId?.Equals(parentId) is not true)
        {
            ParentId = parentId;
        }
    }   
}
