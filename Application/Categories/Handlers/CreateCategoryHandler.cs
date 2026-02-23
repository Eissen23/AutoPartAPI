using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Categories.Handlers;

internal class CreateCategoryHandler(
        ICategoryService categoryService
    ) : IRequestHandler<CreateCategoryRequest, Guid>
{
    private readonly ICategoryService _categoryService = categoryService;

    public async Task<Guid> Handle(CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        var result = await _categoryService.CreateAsync(request, cancellationToken);

        return result;
    }
}
