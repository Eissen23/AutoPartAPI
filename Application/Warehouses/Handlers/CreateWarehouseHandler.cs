using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Warehouses.Handlers;

internal class CreateWarehouseHandler(
        IWarehouseService warehouseService
    ) : IRequestHandler<CreateWarehouseLocationRequest, Guid>
{
    private readonly IWarehouseService _warehouseService = warehouseService;

    public async Task<Guid> Handle(CreateWarehouseLocationRequest request, CancellationToken cancellationToken)
    {
        var result = await _warehouseService.CreateAsync(request, cancellationToken);

        return result;
    }
}
