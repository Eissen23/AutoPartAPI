using System;
using System.Collections.Generic;
using System.Text;
using Base.Application.Products.Models;
using Base.Domain.Entities.Warehouses;

namespace Base.Application.PartLocations.Specs;

public class GetPartLocationByPartId : Specification<PartLocation, WarehouseStockDto>
{
    public GetPartLocationByPartId(Guid partId)
    {
        Query.Where(pl => pl.PartId == partId)
            .Select(pl => new WarehouseStockDto
            {
                Id = pl.Id,
                ZoneCode = pl.WarehouseLocation!.ZoneCode,
                Aisle = pl.WarehouseLocation.Aisle,
                Shelf = pl.WarehouseLocation.Shelf,
                Bin = pl.WarehouseLocation.Bin,
                Quantity = pl.QuantityAtLocation
            });
    }
}