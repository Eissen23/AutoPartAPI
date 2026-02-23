using Application.Common.Models;
using Application.Warehouses;
using Host.Extensions;
using Shared.Common;

namespace Host.Controllers.Warehouses;

public class WarehousesController : VersionedApiController
{
    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync(CreateWarehouseLocationRequest request)
    {
        var result = await Mediator.Send(request);
        return this.ApiOk(result, "Create Warehouse Location Success");
    }

    [Authorize]
    [HttpPost("search")]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResponse<WarehouseLocationDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchAsync(SearchWarehouseLocationRequest request)
    {
        var result = await Mediator.Send(request);
        return this.ApiOk(result, "Search Warehouse Location Success");
    }

    [Authorize]
    [HttpPut("{id:Guid}")]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAsync(UpdateWarehouseLocationRequest request, [FromRoute] Guid id)
    {
        if (request.Id != id)
        {
            throw new Shared.Common.Exceptions.ValidationException("Id mismatch", ["the route id not the same as the request id"]);
        }
        var result = await Mediator.Send(request);
        return this.ApiOk(result, "Update Warehouse Location Success");
    }

    [Authorize]
    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        var result = await Mediator.Send(new DeleteWarehouseLocationRequest(id));
        return this.ApiOk(result, "Delete Warehouse Location Success");
    }

    [Authorize]
    [HttpGet("{id:Guid}")]
    [ProducesResponseType(typeof(ApiResponse<WarehouseLocationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
    {
        var result = await Mediator.Send(new GetWarehouseLocationByIdRequest(id));
        return this.ApiOk(result, "Get Warehouse Location By Id Success");
    }
}
