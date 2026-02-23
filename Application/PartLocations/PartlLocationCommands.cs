using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Models;

namespace Application.PartLocations;

public class CreatePartLocationRequest : IRequest<Guid>
{
    public Guid PartId { get; set; }
    public Guid WarehouseLocationId { get; set; }
    public int QuantityAtLocation { get; set; }
}

public class DeletePartLocationRequest(Guid id) : IRequest<Guid>
{
    public Guid Id { get; set; } = id;
}

public class GetPartLocationByIdRequest(Guid id) : IRequest<PartLocationDto?>
{
    public Guid Id { get; set; } = id;
}

public class SearchPartLocationRequest : PaginationFilter, IRequest<PaginatedResponse<PartLocationDto>>
{
}

public class UpdatePartLocationRequest : IRequest<Guid>
{
    public Guid Id { get; set; }
    public Guid? PartId { get; set; }
    public Guid? WarehouseLocationId { get; set; }
    public int? QuantityAtLocation { get; set; }
}
