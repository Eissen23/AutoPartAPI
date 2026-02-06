using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.Categories;

public class Category : AuditableEntity, IAggregateRoot
{
    public string CategoryCode { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public SystemType? Type { get; private set; }
    public decimal DefaultMarkupPercentage { get; private set; } = 5.2m;


    public Category Update(
        string? name,
        string? description,
        SystemType? type,
        decimal? defaultMarkupPercentage)
    {
        if (name is not null && Name?.Equals(name) is not true)
            Name = name;
        if (description is not null && Description?.Equals(description) is not true)
            Description = description;
        if (type is not null && Type != type)
            Type = type;
        if (defaultMarkupPercentage.HasValue && DefaultMarkupPercentage != defaultMarkupPercentage)
            DefaultMarkupPercentage = defaultMarkupPercentage.Value;

        return this;
    }
}

