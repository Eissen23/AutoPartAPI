using System;
using System.Collections.Generic;
using System.Text;
using Base.Application.Common.Models;

namespace Base.Application.Identities.Departments.Models;

public class CreateDepartmentRequest()
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public Guid? ParentId { get; set; }
}

public class SearchDepartmentRequest : PaginationFilter
{
}

public class UpdateDepartmentRequest 
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public Guid? ParentId { get; set; }
}
