using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Identities.Departments.Handlers;

public class UpdateDeparmentHandler(
        IDepartmentService departmentService
    ) : IRequestHandler<UpdateDepartmentRequest, Guid>
{
    private readonly IDepartmentService _departmentService = departmentService; 
    public Task<Guid> Handle(UpdateDepartmentRequest request, CancellationToken cancellationToken)
    {
        var result = _departmentService.UpdateAsync( request, cancellationToken );

        return result;
    }
}
