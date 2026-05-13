using System;
using System.Collections.Generic;
using System.Text;
using Base.Application.Categories.Models;
using Base.Application.Categories.Specs;
using Base.Application.Common.Extension;
using Base.Application.Common.Models;
using Base.Application.Persistence.Repository;
using Base.Domain.Entities.Categories;
using Shared.Common.Exceptions;

namespace Base.Application.Categories.Services;

public class CategoryService(
        IRepositoryWithEvents<Category> eventRepos,
        IReadRepository<Category> readRepos
    ) : ICategoryService
{
    private readonly IRepositoryWithEvents<Category> _eventRepos = eventRepos;
    private readonly IReadRepository<Category> _readRepos = readRepos;

    public async Task<Guid> CreateAsync(CreateCategoryRequest request, CancellationToken ct = default)
    {
        var category = Category.Create(
                request.CategoryCode,
                request.Name,
                request.Description,
                request.Type,
                request.DefaultMarkupPercentage
            );

        var result = await _eventRepos.AddAsync(category, ct);

        return result.Id;
    }

    public async Task<Guid> DeleteAsync(Guid categoryId, CancellationToken ct = default)
    {
        var category = await _readRepos.GetByIdAsync(categoryId, ct);

        _ = category ?? throw new NotFoundException($"Category with id {categoryId} not found.");

        await _eventRepos.DeleteAsync(category, ct);

        return category.Id;
    }

    public async Task<List<CategoryDto>> GetAllAsync(CancellationToken ct = default)
    {
        var categories = await _readRepos.ListAsync(new GetAllCategories(), ct);

        return categories;
    }

    public async Task<CategoryDto> GetByIdAsync(Guid categoryId, CancellationToken ct = default)
    {
        var category = await _readRepos.GetByIdAsync(categoryId, ct);
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

    public Task<List<CategoryNameDto>> GetMappingCategory(CancellationToken ct = default)
    {
        return _readRepos.ListAsync(new GetAllCategories(), ct)
            .ContinueWith(task => task.Result
                .Select(c => new CategoryNameDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList(), ct);
    }

    public async Task<PaginatedResponse<CategoryDto>> SearchAsync(PaginationFilter filter, CancellationToken ct = default)
    {
        var spec = new CategoryPaginated(filter);
        var result = await _readRepos.PaginatedListAsync(spec, filter.PageNumber, filter.PageSize, ct);

        return result;
    }

    public async Task<Guid> UpdateAsync(Guid id, UpdateCategoryRequest request, CancellationToken ct = default)
    {
        var category = await _readRepos.GetByIdAsync(id, ct);
        _ = category ?? throw new NotFoundException($"Category with id {id} not found.");

        category.Update(
            request.CategoryCode,
            request.Name,
            request.Description,
            request.Type,
            request.DefaultMarkupPercentage
        );

        await _eventRepos.UpdateAsync(category, ct);

        return category.Id;
    }
}
