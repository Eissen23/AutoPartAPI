using Application.Common.Models;
using Application.PartLocations;
using Host.Extensions;
using Shared.Common;

namespace Host.Controllers.Warehouses;

public class PartLocationsController : VersionedApiController
{
    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync(CreatePartLocationRequest request)
    {
        var result = await Mediator.Send(request);
        return this.ApiOk(result, "Create Part Location Success");
    }

    [Authorize]
    [HttpPost("search")]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResponse<PartLocationDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchAsync(SearchPartLocationRequest request)
    {
        var result = await Mediator.Send(request);
        return this.ApiOk(result, "Search Part Location Success");
    }

    [Authorize]
    [HttpPut("{id:Guid}")]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAsync(UpdatePartLocationRequest request, [FromRoute] Guid id)
    {
        if (request.Id != id)
        {
            throw new Shared.Common.Exceptions.ValidationException("Id mismatch", ["the route id not the same as the request id"]);
        }
        var result = await Mediator.Send(request);
        return this.ApiOk(result, "Update Part Location Success");
    }


    [Authorize]
    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        var result = await Mediator.Send(new DeletePartLocationRequest(id));
        return this.ApiOk(result, "Delete Part Location Success");
    }

    [Authorize]
    [HttpGet("{id:Guid}")]
    [ProducesResponseType(typeof(ApiResponse<PartLocationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
    {
        var result = await Mediator.Send(new GetPartLocationByIdRequest(id));
        return this.ApiOk(result, "Get Part Location Success");
    }
}
