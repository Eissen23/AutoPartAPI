using System;
using System.Collections.Generic;
using System.Text;
using Base.Application.Common.Interface;
using Base.Application.Common.Models;
using Base.Application.Identities.Permissions.Models;

namespace Base.Application.Identities.Permissions.Services;

public interface IPermissionService : ITransientService
{
    Task<PaginatedResponse<PermissionDto>> SearchAsync(PaginationFilter filter, CancellationToken ct = default);
    Task<List<PermissionDto>> GetAllAsync(CancellationToken ct = default);
    Task<PermissionDto> GetByIdAsync(Guid permissionId, CancellationToken ct = default);
    Task<Guid> CreateAsync(CreatePermissionRequest request, CancellationToken ct = default);
    Task<Guid> UpdateAsync(Guid id, UpdatePermissionRequest request, CancellationToken ct = default);
    Task<Guid> DeleteAsync(Guid permissionId, CancellationToken ct = default);
}
