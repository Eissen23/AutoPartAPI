using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Domain.Entities.Identity;

public class JobPosition : AuditableEntity, IAggregateRoot
{
    public string Title { get; protected set; } = default!;
    public string? Description { get; protected set; }

    public decimal? Salary { get; protected set; }

    public AccessLevel AccessLevel { get; protected set; }

    public static JobPosition Create(string title, string? description, decimal? salary, AccessLevel accessLevel)
    {
        var entity = new JobPosition()
        {
            Title = title,
            Description = description,
            Salary = salary,
            AccessLevel = accessLevel
        };
        return entity;
    }

    public void Update(string? title, string? description, decimal? salary, AccessLevel? accessLevel)
    {
        if(title is not null && Title?.Equals(title) is not true)
        {
            Title = title;
        }
        if(description is not null && Description?.Equals(description) is not true)
        {
            Description = description;
        }

        if(salary.HasValue && Salary?.Equals(salary) is not true)
        {
            Salary = salary.Value;
        }
        if(accessLevel is not null && accessLevel != AccessLevel)
        {
            AccessLevel = accessLevel.Value;
        }
    }
}

public enum AccessLevel
{
    Associate = 1 ,
    Staff = 2,
    Lead = 3,
    Manager = 4,
    SystemAdmin = 5,
}