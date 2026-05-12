using System;
using System.Collections.Generic;
using System.Text;
using Base.Application.Common.Models;
using Base.Application.Common.Specifications;
using Base.Application.PartLocations.Models;
using Base.Application.Products;
using Base.Domain.Entities.Warehouses;

namespace Base.Application.PartLocations.Specs;

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

