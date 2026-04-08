using Application.Common.Interface;
using Application.Common.Models;
using Application.Identities.Departments.Models;

namespace Application.Identities.Departments.Services;

public interface IDepartmentService : ITransientService
{
    //Pagination
    Task<PaginatedResponse<DepartmentDto>> GetPaginated(PaginationFilter filter, CancellationToken ct = default);

    // Crud actions for Department entity
    Task<List<DepartmentDto>> GetAllAsync(CancellationToken ct = default);
    Task<DepartmentDto> GetByIdAsync(Guid departmentId, CancellationToken ct = default);
    Task<Guid> CreateAsync(CreateDepartmentRequest request, CancellationToken ct = default);
    Task<Guid> UpdateAsync(Guid id, UpdateDepartmentRequest request, CancellationToken ct = default);
    Task<Guid> DeleteAsync(Guid departmentId, CancellationToken ct = default);
}
