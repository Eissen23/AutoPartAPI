using System;
using System.Collections.Generic;
using System.Text;
using Base.Application.Common.Extension;
using Base.Application.Common.Models;
using Base.Application.Identities.JobPosistions.Models;
using Base.Application.Persistence.Repository;
using Base.Domain.Entities.Identity;
using Shared.Common.Exceptions;

namespace Base.Application.Identities.JobPosistions.Services;

public class JobPositionService(
        IRepositoryWithEvents<JobPosition> eventRepos,
        IReadRepository<JobPosition> readRepos
    ) : IJobPositionService
{

    private readonly IRepositoryWithEvents<JobPosition> _eventRepos = eventRepos;
    private readonly IReadRepository<JobPosition> _readRepos = readRepos;

    public async Task<Guid> CreateAsync(CreateJobPositionRequest request, CancellationToken ct = default)
    {
        var jobPosition = new JobPosition(
                title: request.Title,
                description: request.Description,
                salary: request.Salary,
                accessLevel: request.AccessLevel
            );


        var result = await _eventRepos.AddAsync(jobPosition, ct);

        return result.Id;
    }

    public async Task<Guid> DeleteAsync(Guid jobPositionId, CancellationToken ct = default)
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

    public async Task<JobPositionDto> GetByIdAsync(Guid jobPositionId, CancellationToken ct = default)
    {
        var jobPosition = await _readRepos.GetByIdAsync(jobPositionId, ct);
        _ = jobPosition ?? throw new NotFoundException($"Job position with id {jobPositionId} not found.");

        return new JobPositionDto
        {
            Id = jobPosition.Id,
            Name = jobPosition.Title,
            Description = jobPosition.Description ?? string.Empty,
        };
    }

    public async Task<PaginatedResponse<JobPositionDto>> SearchAsync(PaginationFilter filter, CancellationToken ct = default)
    {
        var spec = new JobPositionPaginated(filter);
        var result = await _readRepos.PaginatedListAsync(spec, filter.PageNumber, filter.PageSize, ct);

        return result;
    }

    public async Task<Guid> UpdateAsync(Guid id, UpdateJobPositionRequest request, CancellationToken ct = default)
    {
        var jobPosition = await _readRepos.GetByIdAsync(id, ct);
        _ = jobPosition ?? throw new NotFoundException($"Job position with id {id} not found.");

        jobPosition.Update(
                request.Title,
                request.Description,
                request.Salary,
                request.AccessLevel
            );

        await _eventRepos.UpdateAsync(jobPosition, ct);

        return jobPosition.Id;
    }
}
