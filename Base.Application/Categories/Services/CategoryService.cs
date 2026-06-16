using Base.Application.Categories.Models;
using Base.Application.Common.Interface;
using Base.Application.Common.Models;
using Base.Application.Common.Services;
using Base.Domain.Entities.Categories;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Exceptions;

namespace Base.Application.Categories.Services;

public class CategoryService(IApplicationDbContext context)
    : BaseService<Category, CategoryDto>(context), ICategoryService
{
    public async Task<Guid> CreateAsync(CreateCategoryRequest request, CancellationToken ct = default)
    {
        var category = Category.Create(
            request.CategoryCode,
            request.Name,
            request.Description,
            request.Type,
            request.DefaultMarkupPercentage);

        await base.CreateAsync(category, ct);

        return category.Id;
    }

    public async Task<Guid> DeleteAsync(Guid categoryId, CancellationToken ct = default)
    {
        var category = await FindAsync(categoryId, ct);
        _ = category ?? throw new NotFoundException($"Category with id {categoryId} not found.");

        await base.DeleteAsync(category, ct);

        return category.Id;
    }

    public Task<List<CategoryDto>> GetAllAsync(CancellationToken ct = default)
        => ListAsync(ct);

    public async Task<CategoryDto> GetByIdAsync(Guid categoryId, CancellationToken ct = default)
    {
        var category = await FindAsync(categoryId, ct);
        _ = category ?? throw new NotFoundException($"Category with id {categoryId} not found.");

        return new CategoryDto
        {
            Id = category.Id,
            CategoryCode = category.CategoryCode,
            Name = category.Name,
            Description = category.Description,
            Type = category.Type,
            DefaultMarkupPercentage = category.DefaultMarkupPercentage
        };
    }

    public async Task<List<CategoryNameDto>> GetMappingCategory(CancellationToken ct = default)
    {
        var categories = await ListAsync(ct);

        return categories
            .Select(c => new CategoryNameDto { Id = c.Id, Name = c.Name })
            .ToList();
    }

    public Task<PaginatedResponse<CategoryDto>> SearchAsync(PaginationFilter filter, CancellationToken ct = default)
        => PaginatedSearchAsync(filter, ct);

    public async Task<Guid> UpdateAsync(Guid id, UpdateCategoryRequest request, CancellationToken ct = default)
    {
        var category = await FindAsync(id, ct);
        _ = category ?? throw new NotFoundException($"Category with id {id} not found.");

        category.Update(
            request.CategoryCode,
            request.Name,
            request.Description,
            request.Type,
            request.DefaultMarkupPercentage);

        await base.UpdateAsync(category, ct);

        return category.Id;
    }
}
