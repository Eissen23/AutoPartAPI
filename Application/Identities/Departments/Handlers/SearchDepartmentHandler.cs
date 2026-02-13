using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Models;

namespace Application.Identities.Departments.Handlers;

public class SearchDepartmentHandler(
        IDepartmentService service
    ) : IRequestHandler<SearchDepartmentRequest, PaginatedResponse<DepartmentDto>>
{
    private readonly IDepartmentService _service = service;

    public async Task<PaginatedResponse<DepartmentDto>> Handle(SearchDepartmentRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.GetPaginated(request, cancellationToken);

        return result;
    }
}
