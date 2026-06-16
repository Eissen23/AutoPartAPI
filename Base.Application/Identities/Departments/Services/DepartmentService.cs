using Base.Application.Common.Interface;
using Base.Application.Common.Models;
using Base.Application.Common.Services;
using Base.Application.Identities.Departments.Models;
using Base.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Exceptions;

namespace Base.Application.Identities.Departments.Services;

internal class DepartmentService(IApplicationDbContext context)
    : BaseService<Department, DepartmentDto>(context), IDepartmentService
{
    public async Task<Guid> CreateAsync(CreateDepartmentRequest request, CancellationToken ct = default)
    {
        var department = Department.Create(
            request.Name,
            request.Description ?? "",
            request.ParentId);

        await base.CreateAsync(department, ct);

        return department.Id;
    }

    public async Task<Guid> DeleteAsync(Guid departmentId, CancellationToken ct = default)
    {
        var department = await FindAsync(departmentId, ct);
        _ = department ?? throw new NotFoundException("Department not found.");

        var hasChildren = await Entities.AnyAsync(d => d.ParentId == department.Id, ct);
        if (hasChildren)
        {
            throw new ConflicException("Cannot delete Department with children");
        }

        await base.DeleteAsync(department, ct);

        return department.Id;
    }

    public Task<List<DepartmentDto>> GetAllAsync(CancellationToken ct = default)
        => ListAsync(ct);

    public async Task<DepartmentDto> GetByIdAsync(Guid departmentId, CancellationToken ct = default)
    {
        var department = await FindAsync(departmentId, ct);
        _ = department ?? throw new NotFoundException("Department not found.");

        return new DepartmentDto
        {
            Id = department.Id,
            Name = department.Name,
            Description = department.Description,
            ParentId = department.ParentId
        };
    }

    public Task<PaginatedResponse<DepartmentDto>> GetPaginated(PaginationFilter filter, CancellationToken ct = default)
        => PaginatedSearchAsync(filter, ct);

    public async Task<Guid> UpdateAsync(Guid id, UpdateDepartmentRequest request, CancellationToken ct = default)
    {
        var department = await FindAsync(id, ct);
        _ = department ?? throw new NotFoundException("Department not found.");

        department.Update(
            request.Name,
            request.Description,
            request.ParentId);

        await base.UpdateAsync(department, ct);

        return department.Id;
    }
}
