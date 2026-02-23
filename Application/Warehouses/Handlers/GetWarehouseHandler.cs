using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Warehouses.Handlers;

public class GetWarehouseHandler(
        IWarehouseService warehouseService
    ) : IRequestHandler<GetWarehouseLocationByIdRequest, WarehouseLocationDto?>
{
    private readonly IWarehouseService _warehouseService = warehouseService;

    public async Task<WarehouseLocationDto?> Handle(GetWarehouseLocationByIdRequest request, CancellationToken cancellationToken)
    {
        var result = await _warehouseService.GetByIdAsync(request.Id, cancellationToken);

        return result;
    }
}
