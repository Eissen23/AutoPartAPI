using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Models;
using Application.Common.Specifications;
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
