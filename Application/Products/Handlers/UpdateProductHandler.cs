using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Products.Handlers;

public class UpdateProductHandler(
    IProductService productService
    ) : IRequestHandler<UpdateProductRequest, Guid>
{
    private readonly IProductService _productService = productService;

    public async Task<Guid> Handle(UpdateProductRequest request, CancellationToken cancellationToken)
    {
        var result = await _productService.UpdateAsync(request, cancellationToken);

        return result;
    }
}
