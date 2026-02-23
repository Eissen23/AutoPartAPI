using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Warehouses.Handlers;

public class DeleteWarehouseHandler(
        IWarehouseService warehouseService
    ) : IRequestHandler<DeleteWarehouseLocationRequest, Guid>
{
    private readonly IWarehouseService _warehouseService = warehouseService;

    public async Task<Guid> Handle(DeleteWarehouseLocationRequest request, CancellationToken cancellationToken)
    {
        var result = await _warehouseService.DeleteAsync(request.Id, cancellationToken);

        return result;
    }
}
