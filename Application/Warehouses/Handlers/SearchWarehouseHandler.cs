using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Models;

namespace Application.Warehouses.Handlers;

public class SearchWarehouseHandler(
        IWarehouseService warehouseService
    ) : IRequestHandler<SearchWarehouseLocationRequest, PaginatedResponse<WarehouseLocationDto>>
{
    private readonly IWarehouseService _warehouseService = warehouseService;

    public async Task<PaginatedResponse<WarehouseLocationDto>> Handle(SearchWarehouseLocationRequest request, CancellationToken cancellationToken)
    {
        var result = await _warehouseService.SearchAsync(request, cancellationToken);

        return result;
    }
}
