using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Interface;

namespace Application.Warehouses;

public class WarehouseLocationDto : IDto
{
    public Guid Id { get; set; }
    public string ZoneCode { get; set; } = default!;
    public int Aisle { get; set; }
    public int Shelf { get; set; }
    public string? Bin { get; set; }
    public bool IsOverstocked { get; set; }
}