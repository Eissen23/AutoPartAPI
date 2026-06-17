using System;
using System.Collections.Generic;
using System.Text;
using Base.Application.Common.Interface;
using Base.Application.Common.Models;
using Base.Application.Identities.Roles.Models;
using Base.Application.Identities.Permissions.Models;

namespace Base.Application.Identities.Roles.Services;

public interface IRoleService : ITransientService
{
    Task<PaginatedResponse<RoleDto>> SearchAsync(PaginationFilter filter, CancellationToken ct = default);
    Task<List<RoleDto>> GetAllAsync(CancellationToken ct = default);
    Task<RoleDto> GetByIdAsync(Guid roleId, CancellationToken ct = default);
    Task<Guid> CreateAsync(CreateRoleRequest request, CancellationToken ct = default);
    Task<Guid> UpdateAsync(Guid id, UpdateRoleRequest request, CancellationToken ct = default);
    Task<Guid> DeleteAsync(Guid roleId, CancellationToken ct = default);

    Task AssignPermissionsAsync(Guid roleId, IEnumerable<Guid> permissionIds, CancellationToken ct = default);
    Task RemovePermissionsAsync(Guid roleId, IEnumerable<Guid> permissionIds, CancellationToken ct = default);
    Task<List<PermissionDto>> GetRolePermissionsAsync(Guid roleId, CancellationToken ct = default);
}
