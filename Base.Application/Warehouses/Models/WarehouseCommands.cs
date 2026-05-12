using System;
using System.Collections.Generic;
using System.Text;
using Base.Application.Common.Models;
using Base.Domain.Entities.Warehouses;

namespace Base.Application.Warehouses.Models;

public class CreateWarehouseLocationRequest
{
    public string ZoneCode { get; set; } = default!;
    public int Aisle { get; set; }
    public int Shelf { get; set; }
    public string? Bin { get; set; }
    public bool IsOverstocked { get; set; }
}

public class SearchWarehouseLocationRequest : PaginationFilter
{
}

public class UpdateWarehouseLocationRequest
{
    public string? ZoneCode { get; set; }
    public int? Aisle { get; set; }
    public int? Shelf { get; set; }
    public string? Bin { get; set; }
    public bool? IsOverstocked { get; set; }
}

