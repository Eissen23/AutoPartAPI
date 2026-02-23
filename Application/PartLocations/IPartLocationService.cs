using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Models;

namespace Application.PartLocations;

public interface IPartLocationService
{
    //Pagination
    Task<PaginatedResponse<PartLocationDto>> SearchAsync(PaginationFilter filter, CancellationToken ct);
    // Crud actions for Department entity
    Task<List<PartLocationDto>> GetAllAsync(CancellationToken ct);
    Task<PartLocationDto> GetByIdAsync(Guid departmentId, CancellationToken ct);
    Task<Guid> CreateAsync(CreatePartLocationRequest request, CancellationToken ct);
    Task<Guid> UpdateAsync(UpdatePartLocationRequest request, CancellationToken ct);
    Task<Guid> DeleteAsync(Guid departmentId, CancellationToken ct);
}
