using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.Identity;

public class JobPosition : AuditableEntity, IAggregateRoot
{
    public string Title { get; protected set; } = default!;
    public string? Description { get; protected set; }

    public Guid? DepartmentId { get; protected set; }

    public decimal? Salary { get; protected set; }

    public AccessLevel AccessLevel { get; protected set; }

    // Navigation Properties
    public Department? Department { get; protected set; }

    public JobPosition()
    {
    }

    public JobPosition(string title, string? description, DefaultIdType? departmentId, decimal? salary, AccessLevel accessLevel)
    {
        Title = title;
        Description = description;
        DepartmentId = departmentId;
        Salary = salary;
        AccessLevel = accessLevel;
    }

    public void Update(string? title, string? description, Guid? departmentId, decimal? salary, AccessLevel? accessLevel)
    {
        if(title is not null && Title?.Equals(title) is not true)
        {
            Title = title;
        }
        if(description is not null && Description?.Equals(description) is not true)
        {
            Description = description;
        }

        if(departmentId is not null && DepartmentId?.Equals(departmentId) is not true)
        {
            DepartmentId = departmentId;
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