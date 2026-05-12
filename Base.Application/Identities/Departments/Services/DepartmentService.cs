using System;
using System.Collections.Generic;
using System.Text;
using Base.Application.Common.Extension;
using Base.Application.Common.Models;
using Base.Application.Identities.Departments.Models;
using Base.Application.Identities.Departments.Specs;
using Base.Application.Identities.JobPosistions;
using Base.Application.Persistence.Repository;
using Base.Domain.Entities.Identity;
using Shared.Common.Exceptions;

namespace Base.Application.Identities.Departments.Services;

internal class DepartmentService(
        IRepositoryWithEvents<Department> eventRepos,
        IReadRepository<Department> readRepos
    ) : IDepartmentService
{
    private readonly IRepositoryWithEvents<Department> _eventRepos = eventRepos;
    private readonly IReadRepository<Department> _readRepos = readRepos;

    public async Task<Guid> CreateAsync(CreateDepartmentRequest request, CancellationToken ct = default)
    {
        var department = new Department(
            request.Name,
            request.Description ?? "",
            request.ParentId
        );

        var createdDepartment = await _eventRepos.AddAsync(department, ct);
        await _eventRepos.SaveChangesAsync(ct);

        return createdDepartment.Id;
    }

    public async Task<Guid> DeleteAsync(Guid departmentId, CancellationToken ct = default)
    {
        var department = await _readRepos.GetByIdAsync(departmentId, ct);
        _ = department ?? throw new NotFoundException("Department not found.");

        var hasChildren = await _readRepos.AnyAsync(new DepartmentByParentId(department.Id), ct);
        if( hasChildren )
        {
            throw new ConflicException("Cannot delete Department with children");
        }

        await _eventRepos.DeleteAsync(department, ct);

        return department.Id;
    }

    public async Task<List<DepartmentDto>> GetAllAsync(CancellationToken ct = default)
    {
        var departments = await _readRepos.ListAsync(new GetAllDepartment(), ct);

        return departments;
    }

    public async Task<DepartmentDto> GetByIdAsync(Guid departmentId, CancellationToken ct = default)
    {
        var department = await _readRepos.GetByIdAsync(departmentId, ct);

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
    {

        var spec = new DepartmentPaginated(filter);
        var departments = _readRepos.PaginatedListAsync(spec, filter.PageNumber, filter.PageSize, ct);

        return departments;
    }

    public async Task<Guid> UpdateAsync(Guid id, UpdateDepartmentRequest request, CancellationToken ct = default)
    {
        var department = await _readRepos.GetByIdAsync(id, ct);

        _ = department ?? throw new NotFoundException("Department not found.");

        department.Update(
            request.Name,
            request.Description,
            request.ParentId
        );

        await _eventRepos.UpdateAsync(department, ct);

        return department.Id;
    }
}

