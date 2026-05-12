using System;
using System.Collections.Generic;
using System.Text;
using Base.Application.Common.Models;
using Base.Application.Common.Specifications;
using Base.Application.Warehouses.Models;
using Base.Domain.Entities.Warehouses;

namespace Base.Application.Warehouses.Spec;

public class GetAllWarehouseLocations : Specification<WarehouseLocation, WarehouseLocationDto>
{
    public GetAllWarehouseLocations()
    {
        Query.Select(wl => new WarehouseLocationDto
        {
            Id = wl.Id,
            ZoneCode = wl.ZoneCode,
            Aisle = wl.Aisle,
            Shelf = wl.Shelf,
            Bin = wl.Bin,
            IsOverstocked = wl.IsOverstocked
        });
    }
}

public class WarehouseLocationPaginated(PaginationFilter filter) : PaginationSpecification<WarehouseLocation, WarehouseLocationDto>(filter)
{
}
