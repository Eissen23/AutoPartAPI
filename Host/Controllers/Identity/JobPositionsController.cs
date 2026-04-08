using Application.Common.Models;
using Application.Identities.JobPosistions.Models;
using Application.Identities.JobPosistions.Services;
using Host.Extensions;
using Shared.Common;

namespace Host.Controllers.Identity;

public class JobPositionsController(
        IJobPositionService jobPositionService
    ) : VersionedApiController
{
    private readonly IJobPositionService _jobPositionService =  jobPositionService;

    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<Guid>) ,StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateJobPositionRequest request, CancellationToken ct)
    {
        var result = await _jobPositionService.CreateAsync(request, ct);

        return this.ApiOk(result, "Create Job Positions success.");
    }

    [Authorize]
    [HttpPost("search")]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResponse<JobPositionDto>>) ,StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchAsync([FromBody] SearchJobPositionsRequest request, CancellationToken ct)
    {
        var result = await _jobPositionService.SearchAsync(request, ct);
        return this.ApiOk(result, "Search Job Positions success.");
    }

    [Authorize]
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<Guid>) ,StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAsync([FromRoute]Guid id, [FromBody] UpdateJobPositionRequest request, CancellationToken ct)
    {
  
        var result = await _jobPositionService.UpdateAsync(id, request, ct);
        return this.ApiOk(result, "Update Job Positions success.");
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<Guid>) ,StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id,  CancellationToken ct)
    {
        var result = await _jobPositionService.DeleteAsync(id, ct);
        return this.ApiOk(result, "Delete Job Positions success.");
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<JobPositionDto>) ,StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await _jobPositionService.GetByIdAsync(id, ct);
        return this.ApiOk(result, "Get Job Positions by Id success.");
    }
}
