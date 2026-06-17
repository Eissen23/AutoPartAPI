using System;
using System.Collections.Generic;
using System.Text;
using Base.Application.Common.Models;

namespace Base.Application.Identities.Permissions.Models;

public class CreatePermissionRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class UpdatePermissionRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}

public class SearchPermissionsRequest : PaginationFilter
{
}
