using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Models;

namespace Application.Categories.Handlers;

public class SearchCategoryHandler(
        ICategoryService categoryService
    ) : IRequestHandler<SearchCategoryRequest, PaginatedResponse<CategoryDto>>
{
    private readonly ICategoryService _categoryService = categoryService;

    public async Task<PaginatedResponse<CategoryDto>> Handle(SearchCategoryRequest request, CancellationToken cancellationToken)
    {
        var result = await _categoryService.SearchAsync(request, cancellationToken);

        return result;
    }
}
