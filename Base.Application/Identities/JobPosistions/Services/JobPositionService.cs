using Base.Application.Common.Interface;
using Base.Application.Common.Models;
using Base.Application.Common.Services;
using Base.Application.Identities.JobPosistions.Models;
using Base.Domain.Entities.Identity;
using Shared.Common.Exceptions;

namespace Base.Application.Identities.JobPosistions.Services;

public class JobPositionService(IApplicationDbContext context)
    : BaseService<JobPosition, JobPositionDto>(context), IJobPositionService
{
    public async Task<Guid> CreateAsync(CreateJobPositionRequest request, CancellationToken ct = default)
    {
        var jobPosition = JobPosition.Create(
            title: request.Title,
            description: request.Description,
            salary: request.Salary,
            accessLevel: request.AccessLevel);

        await base.CreateAsync(jobPosition, ct);

        return jobPosition.Id;
    }

    public async Task<Guid> DeleteAsync(Guid jobPositionId, CancellationToken ct = default)
    {
        var jobPosition = await FindAsync(jobPositionId, ct);
        _ = jobPosition ?? throw new NotFoundException($"Job position with id {jobPositionId} not found.");

        await base.DeleteAsync(jobPosition, ct);

        return jobPosition.Id;
    }

    // Kinda useless method, but it is here for the sake of completeness
    public Task<List<JobPositionDto>> GetAllAsync(CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task<JobPositionDto> GetByIdAsync(Guid jobPositionId, CancellationToken ct = default)
    {
        var jobPosition = await FindAsync(jobPositionId, ct);
        _ = jobPosition ?? throw new NotFoundException($"Job position with id {jobPositionId} not found.");

        return new JobPositionDto
        {
            Id = jobPosition.Id,
            Name = jobPosition.Title,
            Description = jobPosition.Description ?? string.Empty,
        };
    }

    public Task<PaginatedResponse<JobPositionDto>> SearchAsync(PaginationFilter filter, CancellationToken ct = default)
        => PaginatedSearchAsync(filter, ct);

    public async Task<Guid> UpdateAsync(Guid id, UpdateJobPositionRequest request, CancellationToken ct = default)
    {
        var jobPosition = await FindAsync(id, ct);
        _ = jobPosition ?? throw new NotFoundException($"Job position with id {id} not found.");

        jobPosition.Update(
            request.Title,
            request.Description,
            request.Salary,
            request.AccessLevel);

        await base.UpdateAsync(jobPosition, ct);

        return jobPosition.Id;
    }
}
