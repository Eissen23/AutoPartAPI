using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Models;

namespace Application.Categories;

public interface ICategoryService
{
    Task<PaginatedResponse<CategoryDto>> SearchAsync(PaginationFilter filter, CancellationToken ct);
    Task<List<CategoryDto>> GetAllAsync(CancellationToken ct);
    Task<CategoryDto> GetByIdAsync(Guid categoryId, CancellationToken ct);
    Task<Guid> CreateAsync(CreateCategoryRequest request, CancellationToken ct);
    Task<Guid> UpdateAsync(UpdateCategoryRequest request, CancellationToken ct);
    Task<Guid> DeleteAsync(Guid categoryId, CancellationToken ct);
}
