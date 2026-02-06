using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.Warehouses;

public class WarehouseLocation : AuditableEntity, IAggregateRoot
{
    public string ZoneCode { get; private set; } = default!;

    public int Aisle { get; private set; }
    public int Shelf { get; private set; }
    public string? Bin { get; private set; }


    public bool IsOverstocked { get; private set; }

    public WarehouseLocation Update(
        string? zoneCode,
        int? aisle,
        int? shelf,
        string? bin,
        bool? isOverstocked)
    {
        if (zoneCode is not null && ZoneCode?.Equals(zoneCode) is not true)
            ZoneCode = zoneCode;
        if (aisle.HasValue && Aisle != aisle)
            Aisle = aisle.Value;
        if (shelf.HasValue && Shelf != shelf)
            Shelf = shelf.Value;
        if (bin is not null && Bin?.Equals(bin) is not true)
            Bin = bin;
        if (isOverstocked.HasValue && IsOverstocked != isOverstocked)
            IsOverstocked = isOverstocked.Value;

        return this;
    }
}
