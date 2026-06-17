using Base.Application.Common.Interface;
using Base.Application.Common.Models;
using Base.Application.Common.Extension;
using Base.Application.Identities.Permissions.Models;
using Base.Application.Identities.Roles.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Exceptions;
using Base.Application.Identities.Roles.Services;

namespace Base.Infrastructure.Identities;

public class RoleService(RoleManager<ApplicationRole> roleManager, IApplicationDbContext context) : IRoleService
{
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
    private readonly IApplicationDbContext _context = context;

    public async Task<Guid> CreateAsync(CreateRoleRequest request, CancellationToken ct = default)
    {
        var role = new ApplicationRole
        {
            Name = request.Name,
            Description = request.Description,
            AccessLevel = request.AccessLevel,
            IsSystemRole = request.IsSystemRole ?? false
        };

        var result = await _roleManager.CreateAsync(role);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Failed to create role: {errors}");
        }

        if (request.PermissionIds != null && request.PermissionIds.Count != 0)
        {
            var permissions = await _context.Set<Permission>()
                .Where(p => request.PermissionIds.Contains(p.Id))
                .Select(p => p.Id)
                .ToListAsync(ct);

            var missing = request.PermissionIds.Except(permissions).ToList();
            if (missing.Count != 0)
                throw new NotFoundException($"Permission(s) {string.Join(',', missing)} not found.");

            var rolePermissions = permissions.Select(pid => new RolePermission
            {
                RoleId = role.Id,
                PermissionId = pid
            }).ToList();

            await _context.Set<RolePermission>().AddRangeAsync(rolePermissions, ct);
            await _context.SaveChangesAsync(ct);
        }

        return role.Id;
    }

    public async Task<Guid> DeleteAsync(Guid roleId, CancellationToken ct = default)
    {
        var role = await _roleManager.FindByIdAsync(roleId.ToString());
        _ = role ?? throw new NotFoundException($"Role with id {roleId} not found.");

        var result = await _roleManager.DeleteAsync(role);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Failed to delete role: {errors}");
        }

        return role.Id;
    }

    public async Task<List<RoleDto>> GetAllAsync(CancellationToken ct = default)
    {
        var roles = await _roleManager.Roles
            .AsNoTracking()
            .ToListAsync(ct);

        return [.. roles.Select(r => new RoleDto
        {
            Id = r.Id,
            Name = r.Name ?? string.Empty,
            Description = r.Description,
            AccessLevel = r.AccessLevel,
            IsSystemRole = r.IsSystemRole,
            Permissions = []
        })];
    }

    public async Task<RoleDto> GetByIdAsync(Guid roleId, CancellationToken ct = default)
    {
        var role = await _roleManager.FindByIdAsync(roleId.ToString());
        _ = role ?? throw new NotFoundException($"Role with id {roleId} not found.");

        var permissionIds = await _context.Set<RolePermission>()
            .Where(rp => rp.RoleId == roleId)
            .Select(rp => rp.PermissionId)
            .ToListAsync(ct);

        var permissions = await _context.Set<Permission>()
            .AsNoTracking()
            .Where(p => permissionIds.Contains(p.Id))
            .ProjectToType<PermissionDto>()
            .ToListAsync(ct);

        return new RoleDto
        {
            Id = role.Id,
            Name = role.Name ?? string.Empty,
            Description = role.Description,
            AccessLevel = role.AccessLevel,
            IsSystemRole = role.IsSystemRole,
            Permissions = permissions
        };
    }

    public async Task<PaginatedResponse<RoleDto>> SearchAsync(PaginationFilter filter, CancellationToken ct = default)
    {
        var query = _roleManager.Roles
            .AsNoTracking()
            .ApplyBaseFilter(filter);

        var count = await query.CountAsync(ct);

        var roles = await query
            .Paginate(filter)
            .ToListAsync(ct);

        var roleDtos = roles.Select(r => new RoleDto
        {
            Id = r.Id,
            Name = r.Name ?? string.Empty,
            Description = r.Description,
            AccessLevel = r.AccessLevel,
            IsSystemRole = r.IsSystemRole,
            Permissions = []
        }).ToList();

        return new PaginatedResponse<RoleDto>(roleDtos, filter.PageNumber, count, filter.PageSize);
    }

    public async Task<Guid> UpdateAsync(Guid id, UpdateRoleRequest request, CancellationToken ct = default)
    {
        var role = await _roleManager.FindByIdAsync(id.ToString());
        _ = role ?? throw new NotFoundException($"Role with id {id} not found.");

        if (request.Name is not null && role.Name?.Equals(request.Name) is not true)
            role.Name = request.Name;

        if (request.Description is not null && role.Description?.Equals(request.Description) is not true)
            role.Description = request.Description;

        if (request.AccessLevel.HasValue && role.AccessLevel != request.AccessLevel)
            role.AccessLevel = request.AccessLevel;

        if (request.IsSystemRole.HasValue && role.IsSystemRole != request.IsSystemRole.Value)
            role.IsSystemRole = request.IsSystemRole.Value;

        role.UpdatedAt = DateTime.UtcNow;

        var result = await _roleManager.UpdateAsync(role);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Failed to update role: {errors}");
        }

        return role.Id;
    }

    public async Task AssignPermissionsAsync(Guid roleId, IEnumerable<Guid> permissionIds, CancellationToken ct = default)
    {
        var role = await _roleManager.FindByIdAsync(roleId.ToString());
        _ = role ?? throw new NotFoundException($"Role with id {roleId} not found.");

        var existing = await _context.Set<RolePermission>()
            .Where(rp => rp.RoleId == roleId)
            .Select(rp => rp.PermissionId)
            .ToListAsync(ct);

        var toAdd = permissionIds.Except(existing).ToList();
        if (toAdd.Count == 0)
            return;

        var validPermissions = await _context.Set<Permission>()
            .Where(p => toAdd.Contains(p.Id))
            .Select(p => p.Id)
            .ToListAsync(ct);

        var missing = toAdd.Except(validPermissions).ToList();
        if (missing.Count != 0)
            throw new NotFoundException($"Permission(s) {string.Join(',', missing)} not found.");

        var rolePermissions = validPermissions.Select(pid => new RolePermission
        {
            RoleId = roleId,
            PermissionId = pid
        });

        await _context.Set<RolePermission>().AddRangeAsync(rolePermissions, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task RemovePermissionsAsync(Guid roleId, IEnumerable<Guid> permissionIds, CancellationToken ct = default)
    {
        var role = await _roleManager.FindByIdAsync(roleId.ToString());
        _ = role ?? throw new NotFoundException($"Role with id {roleId} not found.");

        var toRemove = await _context.Set<RolePermission>()
            .Where(rp => rp.RoleId == roleId && permissionIds.Contains(rp.PermissionId))
            .ToListAsync(ct);

        if (toRemove.Count == 0)
            return;

        _context.Set<RolePermission>().RemoveRange(toRemove);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<List<PermissionDto>> GetRolePermissionsAsync(Guid roleId, CancellationToken ct = default)
    {
        var role = await _roleManager.FindByIdAsync(roleId.ToString());
        _ = role ?? throw new NotFoundException($"Role with id {roleId} not found.");

        var permissionIds = await _context.Set<RolePermission>()
            .Where(rp => rp.RoleId == roleId)
            .Select(rp => rp.PermissionId)
            .ToListAsync(ct);

        var permissions = await _context.Set<Permission>()
            .AsNoTracking()
            .Where(p => permissionIds.Contains(p.Id))
            .ProjectToType<PermissionDto>()
            .ToListAsync(ct);

        return permissions;
    }
}
