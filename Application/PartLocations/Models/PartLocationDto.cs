using System;
using System.Collections.Generic;
using System.Text;
using Base.Application.Common.Interface;

namespace Base.Application.PartLocations.Models;

// Based on PartLocation attribute
public class PartLocationDto : IDto
{
    public Guid Id { get; set; }
    public Guid PartId { get; set; }
    public Guid WarehouseLocationId { get; set; }
    public int QuantityAtLocation { get; set; }

}
