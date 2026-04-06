using Application.Categories.Models;
using Application.Categories.Services;
using Application.Common.Models;
using Host.Extensions;
using Shared.Common;

namespace Host.Controllers.Categories;

public class CategoriesController(
        ICategoryService categoryService
    ) : VersionedApiController
{
    private readonly ICategoryService _categoryService = categoryService;

    [Authorize, HttpPost]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync(CreateCategoryRequest request)
    {
        var result = await _categoryService.CreateAsync(request);
        return this.ApiOk(result, "Create Category Success");
    }

    [Authorize, HttpPost("search")]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResponse<CategoryDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchAsync(SearchCategoryRequest request)
    {
        var result = await _categoryService.SearchAsync(request);
        return this.ApiOk(result, "Search Category Success");
    }

    [Authorize, HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateCategoryRequest request)
    {
        var result = await _categoryService.UpdateAsync(id, request);
        return this.ApiOk(result, "Update Category Success");
    }

    [Authorize, HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        var result = await _categoryService.DeleteAsync(id);
        return this.ApiOk(result, "Delete Category Success");
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<CategoryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
    {
        var result = await _categoryService.GetByIdAsync(id);
        return this.ApiOk(result, "Get Category By Id Success");
    }

    [Authorize]
    [HttpGet("category-map")]
    [ProducesResponseType(typeof(ApiResponse<List<CategoryNameDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetMappingCategory()
    {
        var result = await _categoryService.GetMappingCategory();
        return this.ApiOk(result, "Get Category Map Success");
    }
}
