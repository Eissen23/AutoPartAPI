using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Models;
using Application.Common.Specifications;
using Application.Products;
using Domain.Entities.Warehouses;

namespace Application.PartLocations;

public class GetAllPartLocations : Specification<PartLocation, PartLocationDto>
{
    public GetAllPartLocations()
    {
        Query.Select(pl => new PartLocationDto
        {
            Id = pl.Id,
            PartId = pl.PartId,
            WarehouseLocationId = pl.WarehouseLocationId,
            QuantityAtLocation = pl.QuantityAtLocation
        });
    }
}

public class PartLocationPaginated(PaginationFilter filter) : PaginationSpecification<PartLocation, PartLocationDto>(filter)
{
}

public class GetPartLocationsByWarehouseLocationId : Specification<PartLocation>
{
    public GetPartLocationsByWarehouseLocationId(Guid warehouseLocationId)
    {
        Query.Where(pl => pl.WarehouseLocationId == warehouseLocationId)
            .Include(pl => pl.Part);
    }
}

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