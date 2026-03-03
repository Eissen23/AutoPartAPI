using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Products.Handlers;

public class GetProductHandler(
        IProductService productService
    ) : IRequestHandler<GetProductByIdRequest, ProductDetailDto?>
{
    private readonly IProductService _productService = productService;

    public async Task<ProductDetailDto?> Handle(GetProductByIdRequest request, CancellationToken cancellationToken)
    {
        var result = await _productService.GetByIdAsync(request.Id, cancellationToken);

        return result;
    }
}
