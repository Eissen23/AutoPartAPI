using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Models;

namespace Application.Identities.Departments;

public class CreateDepartmentRequest() : IRequest<Guid>
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public Guid? ParentId { get; set; }
}

public class DeleteDepartmentRequest(Guid id) : IRequest<Guid>
{
    public Guid Id { get; set; } = id;
}

public class GetDepartmentByIdRequest(Guid id) : IRequest<DepartmentDto?>
{
    public Guid Id { get; set; } = id;
}

public class SearchDepartmentRequest : PaginationFilter, IRequest<PaginatedResponse<DepartmentDto>>
{
}
public class UpdateDepartmentRequest : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public Guid? ParentId { get; set; }
}
