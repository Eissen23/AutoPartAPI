using System;
using System.Collections.Generic;
using System.Text;
using Base.Application.Common.Interface;
using Base.Application.Common.Models;
using Base.Application.PartLocations.Models;

namespace Base.Application.PartLocations.Services;

public interface IPartLocationService : ITransientService
{
    //Pagination
    Task<PaginatedResponse<PartLocationDto>> SearchAsync(PaginationFilter filter, CancellationToken ct = default);
    // Crud actions for Department entity
    Task<List<PartLocationDto>> GetAllAsync(CancellationToken ct = default);
    Task<PartLocationDto> GetByIdAsync(Guid departmentId, CancellationToken ct = default);
    Task<Guid> CreateAsync(CreatePartLocationRequest request, CancellationToken ct = default);
    Task<Guid> UpdateAsync(Guid id, UpdatePartLocationRequest request, CancellationToken ct = default);
    Task<Guid> DeleteAsync(Guid departmentId, CancellationToken ct = default);
}
