using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Products.Handlers;

internal class CreateProductHandler(
        IProductService productService
    ) : IRequestHandler<CreateProductRequest, Guid>
{
    private readonly IProductService _productService = productService;

    public async Task<Guid> Handle(CreateProductRequest request, CancellationToken cancellationToken)
    {
        var result = await _productService.CreateAsync(request, cancellationToken);

        return result;
    }
}
