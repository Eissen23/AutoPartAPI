using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities.Products;

namespace Domain.Entities.Warehouses;

public class PartLocation : AuditableEntity, IAggregateRoot
{
    public Guid PartId { get; private set; } = default!;
    public Guid WarehouseLocationId { get; private set; } = default!;

    public int QuantityAtLocation { get; private set; }

    // Navigation properties
    public virtual Product Part { get; private set; } = default!;
    public virtual WarehouseLocation WarehouseLocation { get; private set; } = default!;

    public PartLocation Update(Guid? partId, Guid? warehouseLocationId, int? quantityAtLocation)
    {
        if (partId.HasValue && PartId != partId)
            PartId = partId.Value;
        if (warehouseLocationId.HasValue && WarehouseLocationId != warehouseLocationId)
            WarehouseLocationId = warehouseLocationId.Value;
        if (quantityAtLocation.HasValue && QuantityAtLocation != quantityAtLocation)
            QuantityAtLocation = quantityAtLocation.Value;

        return this;
    }
}
