using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Categories.Handlers;

public class GetCategoryHandler(
        ICategoryService categoryService
    ) : IRequestHandler<GetCategoryByIdRequest, CategoryDto?>
{
    private readonly ICategoryService _categoryService = categoryService;

    public async Task<CategoryDto?> Handle(GetCategoryByIdRequest request, CancellationToken cancellationToken)
    {
        var result = await _categoryService.GetByIdAsync(request.Id, cancellationToken);

        return result;
    }
}
