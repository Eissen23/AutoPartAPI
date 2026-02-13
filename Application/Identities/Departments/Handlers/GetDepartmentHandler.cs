using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Identities.Departments.Handlers;

public class GetDepartmentHandler(
        IDepartmentService service  
    ) : IRequestHandler<GetDepartmentByIdRequest, DepartmentDto?>
{
    private readonly IDepartmentService _service = service;
    public async Task<DepartmentDto?> Handle(GetDepartmentByIdRequest request, CancellationToken cancellationToken)
    {
        var department =  await _service.GetByIdAsync(request.Id, cancellationToken);

        return department;
    }
}
