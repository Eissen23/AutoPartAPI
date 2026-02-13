using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Interface;
using Application.Common.Models;
using Application.Identities.Departments;

namespace Application.Identities.JobPosistions;

public interface IJobPositionService : ITransientService
{
    //Pagination
    Task<PaginatedResponse<JobPositionDto>> SearchAsync(PaginationFilter filter, CancellationToken ct);

    // Crud actions for Department entity
    Task<List<JobPositionDto>> GetAllAsync(CancellationToken ct);
    Task<JobPositionDto> GetByIdAsync(Guid jobPositionId, CancellationToken ct);
    Task<Guid> CreateAsync(CreateJobPositionRequest request, CancellationToken ct);
    Task<Guid> UpdateAsync(UpdateJobPositionRequest request, CancellationToken ct);
    Task<Guid> DeleteAsync(Guid jobPositionId, CancellationToken ct);
}
