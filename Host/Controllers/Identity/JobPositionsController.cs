using Application.Common.Models;
using Application.Identities.JobPosistions;
using Host.Extensions;
using Shared.Common;

namespace Host.Controllers.Identity;

public class JobPositionsController : VersionedApiController
{
    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<Guid>) ,StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateJobPositionRequest request, CancellationToken ct)
    {
        var result = await Mediator.Send(request, ct);

        return this.ApiOk(result, "Create Job Positions success.");
    }

    [Authorize]
    [HttpPost("search")]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResponse<JobPositionDto>>) ,StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchAsync([FromBody] SearchJobPositionsRequest request, CancellationToken ct)
    {
        var result = await Mediator.Send(request, ct);
        return this.ApiOk(result, "Search Job Positions success.");
    }

    [Authorize]
    [HttpPut("{id: Guid}")]
    [ProducesResponseType(typeof(ApiResponse<Guid>) ,StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateJobPositionRequest request, Guid id, CancellationToken ct)
    {
        if (request.Id != id)
        {
            throw new Shared.Common.Exceptions.ValidationException("Id mismatch", ["the route id not the same as the request id"]);
        }

        var result = await Mediator.Send(request, ct);
        return this.ApiOk(result, "Update Job Positions success.");
    }

    [Authorize]
    [HttpDelete("{id: Guid}")]
    [ProducesResponseType(typeof(ApiResponse<Guid>) ,StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id,  CancellationToken ct)
    {
        var result = await Mediator.Send(new DeleteJobPositionRequest(id), ct);
        return this.ApiOk(result, "Delete Job Positions success.");
    }

    [Authorize]
    [HttpGet("{id: Guid}")]
    [ProducesResponseType(typeof(ApiResponse<JobPositionDto>) ,StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await Mediator.Send(new GetJobPositionByIdRequest(id), ct);
        return this.ApiOk(result, "Get Job Positions by Id success.");
    }
}
