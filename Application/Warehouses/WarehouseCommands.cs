using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Models;
using Domain.Entities.Warehouses;

namespace Application.Warehouses;

public class CreateWarehouseLocationRequest : IRequest<Guid>
{
    public string ZoneCode { get; set; } = default!;
    public int Aisle { get; set; }
    public int Shelf { get; set; }
    public string? Bin { get; set; }
    public bool IsOverstocked { get; set; }
}

public class DeleteWarehouseLocationRequest(Guid id) : IRequest<Guid>
{
    public Guid Id { get; set; } = id;
}

public class GetWarehouseLocationByIdRequest(Guid id) : IRequest<WarehouseLocationDto?>
{
    public Guid Id { get; set; } = id;
}

public class SearchWarehouseLocationRequest : PaginationFilter, IRequest<PaginatedResponse<WarehouseLocationDto>>
{
}

public class UpdateWarehouseLocationRequest : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string? ZoneCode { get; set; }
    public int? Aisle { get; set; }
    public int? Shelf { get; set; }
    public string? Bin { get; set; }
    public bool? IsOverstocked { get; set; }
}

