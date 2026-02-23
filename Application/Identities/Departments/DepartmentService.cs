using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Extension;
using Application.Common.Models;
using Application.Identities.JobPosistions;
using Application.Persistence.Repository;
using Domain.Entities.Identity;
using Shared.Common.Exceptions;

namespace Application.Identities.Departments;

internal class DepartmentService(
        IRepositoryWithEvents<Department> eventRepos,
        IReadRepository<Department> readRepos
    ) : IDepartmentService
{
    private readonly IRepositoryWithEvents<Department> _eventRepos = eventRepos;
    private readonly IReadRepository<Department> _readRepos = readRepos;

    public async Task<Guid> CreateAsync(CreateDepartmentRequest request, CancellationToken ct)
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

    public async Task<Guid> DeleteAsync(Guid departmentId, CancellationToken ct)
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

    public async Task<List<DepartmentDto>> GetAllAsync(CancellationToken ct)
    {
        var departments = await _readRepos.ListAsync(new GetAllDepartment(), ct);

        return departments;
    }

    public async Task<DepartmentDto> GetByIdAsync(Guid departmentId, CancellationToken ct)
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

    public Task<PaginatedResponse<DepartmentDto>> GetPaginated(PaginationFilter filter, CancellationToken ct)
    {

        var spec = new DepartmentPaginated(filter);
        var departments = _readRepos.PaginatedListAsync(spec, filter.PageNumber, filter.PageSize, ct);

        return departments;
    }

    public async Task<Guid> UpdateAsync(UpdateDepartmentRequest request, CancellationToken ct)
    {
        var department = await _readRepos.GetByIdAsync(request.Id, ct);

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

