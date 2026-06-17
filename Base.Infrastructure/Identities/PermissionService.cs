using Base.Application.Common.Interface;
using Base.Application.Common.Models;
using Base.Application.Common.Services;
using Base.Application.Identities.Permissions.Models;
using Base.Application.Identities.Permissions.Services;
using Shared.Common.Exceptions;

namespace Base.Infrastructure.Identities;

public class PermissionService(IApplicationDbContext context)
    : BaseService<Permission, PermissionDto>(context), IPermissionService
{
    public async Task<Guid> CreateAsync(CreatePermissionRequest request, CancellationToken ct = default)
    {
        var permission = new Permission
        {
            Name = request.Name,
            Description = request.Description
        };

        await base.CreateAsync(permission, ct);

        return permission.Id;
    }

    public async Task<Guid> DeleteAsync(Guid permissionId, CancellationToken ct = default)
    {
        var permission = await FindAsync(permissionId, ct);
        _ = permission ?? throw new NotFoundException($"Permission with id {permissionId} not found.");

        await base.DeleteAsync(permission, ct);

        return permission.Id;
    }

    public Task<List<PermissionDto>> GetAllAsync(CancellationToken ct = default)
        => ListAsync(ct);

    public async Task<PermissionDto> GetByIdAsync(Guid permissionId, CancellationToken ct = default)
    {
        var permission = await FindAsync(permissionId, ct);
        _ = permission ?? throw new NotFoundException($"Permission with id {permissionId} not found.");

        return new PermissionDto
        {
            Id = permission.Id,
            Name = permission.Name,
            Description = permission.Description
        };
    }

    public Task<PaginatedResponse<PermissionDto>> SearchAsync(PaginationFilter filter, CancellationToken ct = default)
        => PaginatedSearchAsync(filter, ct);

    public async Task<Guid> UpdateAsync(Guid id, UpdatePermissionRequest request, CancellationToken ct = default)
    {
        var permission = await FindAsync(id, ct);
        _ = permission ?? throw new NotFoundException($"Permission with id {id} not found.");

        if (request.Name is not null && permission.Name?.Equals(request.Name) is not true)
            permission.Name = request.Name;

        if (request.Description is not null && permission.Description?.Equals(request.Description) is not true)
            permission.Description = request.Description;

        await base.UpdateAsync(permission, ct);

        return permission.Id;
    }
}
