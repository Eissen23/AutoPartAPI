using System;
using System.Collections.Generic;
using System.Text;
using Base.Domain.Entities.Common.Contracts;

namespace Base.Infrastructure.Identities;

public class Permission : AuditableEntity
{
    public string Name { get; set; } = default!;

    public string? Description { get; set; }

    public ICollection<RolePermission> RolePermissions { get; set; }
        = [];
}


public class RolePermission : AuditableEntity
{
    public Guid RoleId { get; set; }
    public ApplicationRole Role { get; set; } = default!;

    public Guid PermissionId { get; set; }
    public Permission Permission { get; set; } = default!;
}
