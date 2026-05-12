using System;
using System.Collections.Generic;
using System.Text;
using Base.Application.Common.Interface;
using Base.Application.PartLocations;

namespace Base.Application.Warehouses.Models;

public class WarehouseLocationDto : IDto
{
    public Guid Id { get; set; }
    public string ZoneCode { get; set; } = default!;
    public int Aisle { get; set; }
    public int Shelf { get; set; }
    public string? Bin { get; set; }
    public bool IsOverstocked { get; set; }
}


public class WarehouseLocationDetailDto
{
    public Guid Id { get; set; }
    public string ZoneCode { get; set; } = default!;
    public int Aisle { get; set; }
    public int Shelf { get; set; }
    public string? Bin { get; set; }
    public bool IsOverstocked { get; set; }

    public IEnumerable<ExistingPart>? ExistingPart { get; set; }
}

public class ExistingPart
{
    public Guid Id { get; set; }
    public string? PartName { get; set; }
    public string? PartNumber { get; set; }
    public int? Quantity { get; set; }
}