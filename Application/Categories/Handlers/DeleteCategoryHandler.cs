using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Categories.Handlers;

public class DeleteCategoryHandler(
        ICategoryService categoryService
    ) : IRequestHandler<DeleteCategoryRequest, Guid>
{
    private readonly ICategoryService _categoryService = categoryService;

    public async Task<Guid> Handle(DeleteCategoryRequest request, CancellationToken cancellationToken)
    {
        var result = await _categoryService.DeleteAsync(request.Id, cancellationToken);

        return result;
    }
}
