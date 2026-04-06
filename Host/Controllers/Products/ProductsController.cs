using Application.Common.Models;
using Application.Products.Models;
using Application.Products.Services;
using Host.Extensions;
using Shared.Common;

namespace Host.Controllers.Products;

public class ProductsController(
        IProductService productService
    ) : VersionedApiController
{
    private readonly IProductService _productService = productService;

    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync(CreateProductRequest request, CancellationToken ct)
    {
        var result = await _productService.CreateAsync(request, ct);
        return this.ApiOk(result, "Create Product Success");
    }

    [Authorize]
    [HttpPost]
    [Route("search")]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResponse<ProductDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchAsync(SearchProductRequest request, CancellationToken ct)
    {
        var result = await _productService.SearchAsync(request, ct);
        return this.ApiOk(result, "Search Product Success");
    }

    [Authorize]
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, UpdateProductRequest request, CancellationToken ct)
    {
        var result = await _productService.UpdateAsync(id, request, ct);
        return this.ApiOk(result, "Update Product Success");
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await _productService.DeleteAsync(id, ct);
        return this.ApiOk(result, "Delete Product Success");
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<ProductDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await _productService.GetByIdAsync(id, ct);
        return this.ApiOk(result, "Get Product Success");
    }
}
