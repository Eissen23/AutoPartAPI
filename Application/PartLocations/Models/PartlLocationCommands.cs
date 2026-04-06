using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Models;

namespace Application.PartLocations.Models;

public class CreatePartLocationRequest 
{
    public Guid PartId { get; set; }
    public Guid WarehouseLocationId { get; set; }
    public int QuantityAtLocation { get; set; }
}

public class SearchPartLocationRequest : PaginationFilter
{
}

public class UpdatePartLocationRequest
{
    public Guid? PartId { get; set; }
    public Guid? WarehouseLocationId { get; set; }
    public int? QuantityAtLocation { get; set; }
}
