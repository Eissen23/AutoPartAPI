using Domain.Entities.Categories;
using Domain.ValueObjects;

namespace Domain.Entities.Products;

public class Product : AuditableEntity, IAggregateRoot
{
    public string PartNumber { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }

    public decimal UnitCost { get; private set; }
    public decimal? RetailPrice { get; private set; }

    public Guid CategoryId { get; private set; }

    // Navigation property
    public virtual Category Category { get; private set; } = default!;

    public Product Update(
        string? partNumber,
        string? name,
        string? description,
        decimal? unitCost,
        decimal? retailPrice,
        Guid? categoryId)
    {
        if (partNumber is not null && PartNumber?.Equals(partNumber) is not true)
            PartNumber = partNumber;
        if (name is not null && Name?.Equals(name) is not true)
            Name = name;
        if (description is not null && Description?.Equals(description) is not true)
            Description = description;
        if (unitCost.HasValue && UnitCost != unitCost)
            UnitCost = unitCost.Value;
        if (retailPrice.HasValue && RetailPrice != retailPrice)
            RetailPrice = retailPrice;
        if (categoryId.HasValue && CategoryId != categoryId)
            CategoryId = categoryId.Value;

        return this;
    }
}