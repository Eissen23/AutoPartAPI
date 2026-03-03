using Application.Categories;
using Application.Common.Models;
using Host.Extensions;
using Shared.Common;

namespace Host.Controllers.Categories;

public class CategoriesController : VersionedApiController
{
    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync(CreateCategoryRequest request)
    {
        var result = await Mediator.Send(request);
        return this.ApiOk(result, "Create Category Success");
    }

    [Authorize]
    [HttpPost("search")]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResponse<CategoryDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchAsync(SearchCategoryRequest request)
    {
        var result = await Mediator.Send(request);
        return this.ApiOk(result, "Search Category Success");
    }

    [Authorize]
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAsync(UpdateCategoryRequest request, [FromRoute] Guid id)
    {
        if (request.Id != id)
        {
            throw new Shared.Common.Exceptions.ValidationException("Id mismatch", ["the route id not the same as the request id"]);
        }
        var result = await Mediator.Send(request);
        return this.ApiOk(result, "Update Category Success");
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        var result = await Mediator.Send(new DeleteCategoryRequest(id));
        return this.ApiOk(result, "Delete Category Success");
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<CategoryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
    {
        var result = await Mediator.Send(new GetCategoryByIdRequest(id));
        return this.ApiOk(result, "Get Category By Id Success");
    }

    [Authorize]
    [HttpGet("category-map")]
    [ProducesResponseType(typeof(ApiResponse<List<CategoryNameDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCategoryMapAsync()
    {
        var result = await Mediator.Send(new GetCategoryMapRequest());
        return this.ApiOk(result, "Get Category Map Success");
    }
}
