using Base.Application.Common.Models;
using Base.Application.Warehouses.Models;
using Base.Application.Warehouses.Services;
using Azure.Core;
using Host.Extensions;
using Shared.Common;

namespace Host.Controllers.Warehouses;

public class WarehousesController(
        IWarehouseService warehouseService
    ) : VersionedApiController
{
    private readonly IWarehouseService _warehouseService = warehouseService;

    [Authorize, HttpPost]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync(CreateWarehouseLocationRequest request)
    {
        var result = await _warehouseService.CreateAsync(request);
        return this.ApiOk(result, "Create Warehouse Location Success");
    }

    [Authorize, HttpPost("search")]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResponse<WarehouseLocationDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchAsync(SearchWarehouseLocationRequest request)
    {
        var result = await _warehouseService.SearchAsync(request);
        return this.ApiOk(result, "Search Warehouse Location Success");
    }

    [Authorize, HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateWarehouseLocationRequest request)
    {

        var result = await _warehouseService.UpdateAsync(id, request);
        return this.ApiOk(result, "Update Warehouse Location Success");
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        var result = await _warehouseService.DeleteAsync(id);
        return this.ApiOk(result, "Delete Warehouse Location Success");
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<WarehouseLocationDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
    {
        var result = await _warehouseService.GetByIdAsync(id);
        return this.ApiOk(result, "Get Warehouse Location By Id Success");
    }
}
