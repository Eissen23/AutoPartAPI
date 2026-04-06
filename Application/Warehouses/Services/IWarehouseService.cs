using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Interface;
using Application.Common.Models;
using Application.Identities.Departments;
using Application.Warehouses.Models;

namespace Application.Warehouses.Services;

public interface IWarehouseService : ITransientService
{
    //Pagination
    Task<PaginatedResponse<WarehouseLocationDto>> SearchAsync(PaginationFilter filter, CancellationToken ct = default);
    // Crud actions for Department entity
    Task<List<WarehouseLocationDto>> GetAllAsync(CancellationToken ct = default);
    Task<WarehouseLocationDetailDto> GetByIdAsync(Guid departmentId, CancellationToken ct = default);
    Task<Guid> CreateAsync(CreateWarehouseLocationRequest request, CancellationToken ct = default);
    Task<Guid> UpdateAsync(Guid id, UpdateWarehouseLocationRequest request, CancellationToken ct = default);
    Task<Guid> DeleteAsync(Guid departmentId, CancellationToken ct = default);
}
