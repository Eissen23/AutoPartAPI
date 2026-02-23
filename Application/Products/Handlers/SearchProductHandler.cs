using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Models;

namespace Application.Products.Handlers;

public class SearchProductHandler(
        IProductService productService
    ) : IRequestHandler<SearchProductRequest, PaginatedResponse<ProductDto>>
{
    private readonly IProductService _productService = productService;

    public async Task<PaginatedResponse<ProductDto>> Handle(SearchProductRequest request, CancellationToken cancellationToken)
    {
        var result = await _productService.SearchAsync(request, cancellationToken);

        return result;
    }
}
