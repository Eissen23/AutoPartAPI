using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Extension;
using Application.Common.Models;
using Application.Persistence.Repository;
using Domain.Entities.Identity;
using Shared.Common.Exceptions;

namespace Application.Identities.JobPosistions;

public class JobPositionService(
        IRepositoryWithEvents<JobPosition> eventRepos,
        IReadRepository<JobPosition> readRepos
    ) : IJobPositionService
{

    private readonly IRepositoryWithEvents<JobPosition> _eventRepos = eventRepos;
    private readonly IReadRepository<JobPosition> _readRepos = readRepos;

    public async Task<Guid> CreateAsync(CreateJobPositionRequest request, CancellationToken ct)
    {
        var jobPosition = new JobPosition(
                title: request.Title,
                description: request.Description,
                departmentId: request.DepartmentId,
                salary: request.Salary,
                accessLevel: request.AccessLevel
            );


        var result = await _eventRepos.AddAsync(jobPosition, ct);

        return result.Id;
    }

    public async Task<Guid> DeleteAsync(Guid jobPositionId, CancellationToken ct)
    {
        var jobPosition = await _readRepos.GetByIdAsync(jobPositionId, ct);

        _ = jobPosition ?? throw new NotFoundException($"Job position with id {jobPositionId} not found.");

        await _eventRepos.DeleteAsync(jobPosition, ct);

        return jobPosition.Id;
    }

    // Kinda useless method, but it is here for the sake of completeness
    public Task<List<JobPositionDto>> GetAllAsync(CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task<JobPositionDto> GetByIdAsync(Guid jobPositionId, CancellationToken ct)
    {
        var jobPosition = await _readRepos.GetByIdAsync(jobPositionId, ct);
        _ = jobPosition ?? throw new NotFoundException($"Job position with id {jobPositionId} not found.");

        return new JobPositionDto
        {
            Id = jobPosition.Id,
            Name = jobPosition.Title,
            Description = jobPosition.Description ?? string.Empty,
            DepartmentId = jobPosition.DepartmentId ?? Guid.Empty,
        };
    }

    public async Task<PaginatedResponse<JobPositionDto>> SearchAsync(PaginationFilter filter, CancellationToken ct)
    {
        var spec = new JobPositionPaginated(filter);
        var result = await _readRepos.PaginatedListAsync(spec, filter.PageNumber, filter.PageSize, ct);

        return result;
    }

    public async Task<Guid> UpdateAsync(UpdateJobPositionRequest request, CancellationToken ct)
    {
        var jobPosition = await _readRepos.GetByIdAsync(request.Id, ct);
        _ = jobPosition ?? throw new NotFoundException($"Job position with id {request.Id} not found.");

        jobPosition.Update(
                request.Title,
                request.Description,
                request.DepartmentId,
                request.Salary,
                request.AccessLevel
            );

        await _eventRepos.UpdateAsync(jobPosition, ct);

        return jobPosition.Id;
    }
}
