using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Products.Handlers;

public class DeleteProductHandler(
        IProductService productService
    ) : IRequestHandler<DeleteProductRequest, Guid>
{
    private readonly IProductService _productService = productService;

    public async Task<Guid> Handle(DeleteProductRequest request, CancellationToken cancellationToken)
    {
        var result = await _productService.DeleteAsync(request.Id, cancellationToken);

        return result;
    }
}
