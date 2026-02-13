using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Identities.Departments.Handlers;

public class DeleteDepartmentHandler(
        IDepartmentService service
    ) : IRequestHandler<DeleteDepartmentRequest, Guid>
{
    private readonly IDepartmentService _service = service;

    public async Task<Guid> Handle(DeleteDepartmentRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.DeleteAsync(request.Id, cancellationToken);

        return result;
    }
}
