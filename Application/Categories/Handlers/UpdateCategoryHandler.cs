using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Categories.Handlers;

public class UpdateCategoryHandler(
        ICategoryService categoryService
    ) : IRequestHandler<UpdateCategoryRequest, Guid>
{
    private readonly ICategoryService _categoryService = categoryService;

    public async Task<Guid> Handle(UpdateCategoryRequest request, CancellationToken cancellationToken)
    {
        var result = await _categoryService.UpdateAsync(request, cancellationToken);

        return result;
    }
}
