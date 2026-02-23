using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Extension;
using Application.Common.Models;
using Application.Persistence.Repository;
using Domain.Entities.Categories;
using Shared.Common.Exceptions;

namespace Application.Categories;

public class CategoryService(
        IRepositoryWithEvents<Category> eventRepos,
        IReadRepository<Category> readRepos
    ) : ICategoryService
{
    private readonly IRepositoryWithEvents<Category> _eventRepos = eventRepos;
    private readonly IReadRepository<Category> _readRepos = readRepos;

    public async Task<Guid> CreateAsync(CreateCategoryRequest request, CancellationToken ct)
    {
        var category = new Category()
            .Update(
                request.Name,
                request.Description,
                request.Type,
                request.DefaultMarkupPercentage
            );

        var result = await _eventRepos.AddAsync(category, ct);

        return result.Id;
    }

    public async Task<Guid> DeleteAsync(Guid categoryId, CancellationToken ct)
    {
        var category = await _readRepos.GetByIdAsync(categoryId, ct);

        _ = category ?? throw new NotFoundException($"Category with id {categoryId} not found.");

        await _eventRepos.DeleteAsync(category, ct);

        return category.Id;
    }

    public async Task<List<CategoryDto>> GetAllAsync(CancellationToken ct)
    {
        var categories = await _readRepos.ListAsync(new GetAllCategories(), ct);

        return categories;
    }

    public async Task<CategoryDto> GetByIdAsync(Guid categoryId, CancellationToken ct)
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

    public async Task<PaginatedResponse<CategoryDto>> SearchAsync(PaginationFilter filter, CancellationToken ct)
    {
        var spec = new CategoryPaginated(filter);
        var result = await _readRepos.PaginatedListAsync(spec, filter.PageNumber, filter.PageSize, ct);

        return result;
    }

    public async Task<Guid> UpdateAsync(UpdateCategoryRequest request, CancellationToken ct)
    {
        var category = await _readRepos.GetByIdAsync(request.Id, ct);
        _ = category ?? throw new NotFoundException($"Category with id {request.Id} not found.");

        category.Update(
            request.Name,
            request.Description,
            request.Type,
            request.DefaultMarkupPercentage
        );

        await _eventRepos.UpdateAsync(category, ct);

        return category.Id;
    }
}
