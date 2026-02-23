using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Interface;
using Application.Common.Models;
using Application.Identities.Departments;

namespace Application.Warehouses;

public interface IWarehouseService : ITransientService
{
    //Pagination
    Task<PaginatedResponse<WarehouseLocationDto>> SearchAsync(PaginationFilter filter, CancellationToken ct);
    // Crud actions for Department entity
    Task<List<WarehouseLocationDto>> GetAllAsync(CancellationToken ct);
    Task<WarehouseLocationDto> GetByIdAsync(Guid departmentId, CancellationToken ct);
    Task<Guid> CreateAsync(CreateWarehouseLocationRequest request, CancellationToken ct);
    Task<Guid> UpdateAsync(UpdateWarehouseLocationRequest request, CancellationToken ct);
    Task<Guid> DeleteAsync(Guid departmentId, CancellationToken ct);
}
