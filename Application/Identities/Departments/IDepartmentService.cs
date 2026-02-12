using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Interface;
using Application.Identities.Departments.Command;

namespace Application.Identities.Departments;

public interface IDepartmentService : ITransientService
{
    // Crud actions for Department entity
    Task<List<DepartmentDto>> GetAllDepartmentsAsync();
    Task<DepartmentDto> GetDepartmentByIdAsync(Guid departmentId);
    Task<Guid> CreateDepartmentAsync(CreateDepartmentRequest request);
    Task<Guid> UpdateDepartmentAsync(UpdateDepartmentRequest request);
    Task DeleteDepartmentAsync(Guid departmentId);
}
