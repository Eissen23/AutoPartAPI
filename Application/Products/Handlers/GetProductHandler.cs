using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Products.Handlers;

public class GetProductHandler(
        IProductService productService
    ) : IRequestHandler<GetProductByIdRequest, ProductDto?>
{
    private readonly IProductService _productService = productService;

    public async Task<ProductDto?> Handle(GetProductByIdRequest request, CancellationToken cancellationToken)
    {
        var result = await _productService.GetByIdAsync(request.Id, cancellationToken);

        return result;
    }
}
