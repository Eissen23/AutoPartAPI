using System;
using System.Collections.Generic;
using System.Text;
using Base.Application.Common.Interface;
using Base.Application.Identities.Permissions.Models;

namespace Base.Application.Identities.Roles.Models;

public class RoleDto : IDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? AccessLevel { get; set; }
    public bool IsSystemRole { get; set; }
    public List<PermissionDto> Permissions { get; set; } = new();
}
