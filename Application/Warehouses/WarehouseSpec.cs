using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Models;
using Application.Common.Specifications;
using Domain.Entities.Warehouses;

namespace Application.Warehouses;

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
