using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Domain.Entities.Categories;

public class Category : AuditableEntity, IAggregateRoot
{
    public string CategoryCode { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public SystemType? Type { get; private set; }
    public decimal DefaultMarkupPercentage { get; private set; } = 5.2m;

    public static Category Create(
        string categoryCode,
        string name,
        string? description,
        SystemType? type,
        decimal defaultMarkupPercentage)
    {
        var entity = new Category()
        {
            CategoryCode = categoryCode,
            Name = name,
            Description = description,
            Type = type,
            DefaultMarkupPercentage = defaultMarkupPercentage
        };
        return entity;
    }

    public Category Update(
        string? categoryCode,
        string? name,
        string? description,
        SystemType? type,
        decimal? defaultMarkupPercentage)
    {
        if (categoryCode is not null && CategoryCode?.Equals(categoryCode) is not true)
            CategoryCode = categoryCode;

        if (name is not null && Name?.Equals(name) is not true)
            Name = name;
        if (Description?.Equals(description) is not true)
            Description = description;
        if (type is not null && Type != type)
            Type = type;
        if (defaultMarkupPercentage.HasValue && DefaultMarkupPercentage != defaultMarkupPercentage)
            DefaultMarkupPercentage = defaultMarkupPercentage.Value;

        return this;
    }
}

