using Application.Common.Models;
using Application.PartLocations.Models;
using Application.PartLocations.Services;
using Host.Extensions;
using Shared.Common;

namespace Host.Controllers.Warehouses;

public class PartLocationsController(
        IPartLocationService partLocationService
    ) : VersionedApiController
{
    private readonly IPartLocationService _partLocationService = partLocationService; 

    [Authorize, HttpPost]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync(CreatePartLocationRequest request)
    {
        var result = await _partLocationService.CreateAsync(request);
        return this.ApiOk(result, "Create Part Location Success");
    }

    [Authorize, HttpPost("search")]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResponse<PartLocationDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchAsync(SearchPartLocationRequest request)
    {
        var result = await _partLocationService.SearchAsync(request);
        return this.ApiOk(result, "Search Part Location Success");
    }

    [Authorize, HttpPut("{id:Guid}")]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdatePartLocationRequest request)
    {
        var result = await _partLocationService.UpdateAsync(id, request);
        return this.ApiOk(result, "Update Part Location Success");
    }


    [Authorize, HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        var result = await _partLocationService.DeleteAsync(id);
        return this.ApiOk(result, "Delete Part Location Success");
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<PartLocationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
    {
        var result = await _partLocationService.GetByIdAsync(id);
        return this.ApiOk(result, "Get Part Location Success");
    }
}
