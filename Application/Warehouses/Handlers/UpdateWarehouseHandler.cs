using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Warehouses.Handlers;

public class UpdateWarehouseHandler(
    IWarehouseService warehouseService
    ) : IRequestHandler<UpdateWarehouseLocationRequest, Guid>
{
    private readonly IWarehouseService _warehouseService = warehouseService;

    public async Task<Guid> Handle(UpdateWarehouseLocationRequest request, CancellationToken cancellationToken)
    {
        var result = await _warehouseService.UpdateAsync(request, cancellationToken);

        return result;
    }
}
