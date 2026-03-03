using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Categories.Handlers;

public class GetCategoryMapHandler(
        ICategoryService categoryService
    ) : IRequestHandler<GetCategoryMapRequest, List<CategoryNameDto>>
{
    private readonly ICategoryService _categoryService = categoryService;

    public async Task<List<CategoryNameDto>> Handle(GetCategoryMapRequest request, CancellationToken cancellationToken)
    {
        var result = await _categoryService.GetMappingCategory(cancellationToken);

        return result;
    }
}
