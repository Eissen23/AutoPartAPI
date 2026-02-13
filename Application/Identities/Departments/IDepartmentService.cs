using Application.Common.Interface;
using Application.Common.Models;

namespace Application.Identities.Departments;

public interface IDepartmentService : ITransientService
{
    //Pagination
    Task<PaginatedResponse<DepartmentDto>> GetPaginated(PaginationFilter filter, CancellationToken ct);

    // Crud actions for Department entity
    Task<List<DepartmentDto>> GetAllAsync(CancellationToken ct);
    Task<DepartmentDto> GetByIdAsync(Guid departmentId, CancellationToken ct);
    Task<Guid> CreateAsync(CreateDepartmentRequest request, CancellationToken ct);
    Task<Guid> UpdateAsync(UpdateDepartmentRequest request, CancellationToken ct);
    Task<Guid> DeleteAsync(Guid departmentId, CancellationToken ct);
}
