using System;
using System.Collections.Generic;
using System.Text;
using Base.Application.Common.Interface;
using Base.Application.Common.Models;
using Base.Application.Identities.Departments;
using Base.Application.Identities.JobPosistions.Models;

namespace Base.Application.Identities.JobPosistions.Services;

public interface IJobPositionService : ITransientService
{
    //Pagination
    Task<PaginatedResponse<JobPositionDto>> SearchAsync(PaginationFilter filter, CancellationToken ct = default);

    // Crud actions for Department entity
    Task<List<JobPositionDto>> GetAllAsync(CancellationToken ct = default);
    Task<JobPositionDto> GetByIdAsync(Guid jobPositionId, CancellationToken ct = default);
    Task<Guid> CreateAsync(CreateJobPositionRequest request, CancellationToken ct = default);
    Task<Guid> UpdateAsync(Guid id, UpdateJobPositionRequest request, CancellationToken ct = default);
    Task<Guid> DeleteAsync(Guid jobPositionId, CancellationToken ct = default);
}
