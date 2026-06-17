using System;
using System.Collections.Generic;
using System.Text;
using Base.Application.Common.Models;

namespace Base.Application.Identities.Roles.Models;

public class CreateRoleRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? AccessLevel { get; set; }
    public bool? IsSystemRole { get; set; }
    public List<Guid>? PermissionIds { get; set; }
}

public class UpdateRoleRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? AccessLevel { get; set; }
    public bool? IsSystemRole { get; set; }
}

public class AssignPermissionsToRoleRequest
{
    public List<Guid> PermissionIds { get; set; } = new();
}

public class SearchRolesRequest : PaginationFilter
{
}
