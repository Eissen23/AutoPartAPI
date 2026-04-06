using System;
using System.Collections.Generic;
using System.Text;
using Application.Categories.Models;
using Application.Common.Interface;
using Application.Common.Models;

namespace Application.Categories.Services;

public interface ICategoryService : ITransientService
{
    Task<PaginatedResponse<CategoryDto>> SearchAsync(PaginationFilter filter, CancellationToken ct = default);
    Task<List<CategoryDto>> GetAllAsync(CancellationToken ct = default);
    Task<CategoryDto> GetByIdAsync(Guid categoryId, CancellationToken ct = default);
    Task<Guid> CreateAsync(CreateCategoryRequest request, CancellationToken ct = default);
    Task<Guid> UpdateAsync(Guid id, UpdateCategoryRequest request, CancellationToken ct = default);
    Task<Guid> DeleteAsync(Guid categoryId, CancellationToken ct = default);

    //Optimize later
    Task<List<CategoryNameDto>> GetMappingCategory(CancellationToken ct = default);
}
