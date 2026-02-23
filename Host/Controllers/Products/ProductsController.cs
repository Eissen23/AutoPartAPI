using Application.Common.Models;
using Application.Products;
using Host.Extensions;
using Shared.Common;

namespace Host.Controllers.Products;

public class ProductsController : VersionedApiController
{
    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync(CreateProductRequest request, CancellationToken ct)
    {
        var result = await Mediator.Send(request, ct);
        return this.ApiOk(result, "Create Product Success");
    }

    [Authorize]
    [HttpPost]
    [Route("search")]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResponse<ProductDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchAsync(SearchProductRequest request, CancellationToken ct)
    {
        var result = await Mediator.Send(request, ct);
        return this.ApiOk(result, "Search Product Success");
    }

    [Authorize]
    [HttpPut("{id:Guid}")]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAsync(UpdateProductRequest request, [FromRoute] Guid id, CancellationToken ct)
    {
        if (request.Id != id)
        {
            throw new Shared.Common.Exceptions.ValidationException("Id mismatch", ["the route id not the same as the request id"]);
        }
        var result = await Mediator.Send(request, ct);
        return this.ApiOk(result, "Update Product Success");
    }

    [Authorize]
    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await Mediator.Send(new DeleteProductRequest(id), ct);
        return this.ApiOk(result, "Delete Product Success");
    }

    [Authorize]
    [HttpGet("{id:Guid}")]
    [ProducesResponseType(typeof(ApiResponse<ProductDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await Mediator.Send(new GetProductByIdRequest(id), ct);
        return this.ApiOk(result, "Get Product Success");
    }
}
