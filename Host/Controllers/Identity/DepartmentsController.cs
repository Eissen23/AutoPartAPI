using Application.Common.Models;
using Application.Identities.Departments;
using Host.Extensions;
using Shared.Common;

namespace Host.Controllers.Identity;

public class DepartmentsController : VersionedApiController
{
    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<Guid>) ,StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateDepartmentRequest request, CancellationToken ct)
    {
        var result = await Mediator.Send(request, ct);

        return this.ApiOk(result, "Create Departments success.");
    }

    [Authorize]
    [HttpPost("search")]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResponse<DepartmentDto>>) ,StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchAsync([FromBody] SearchDepartmentRequest request, CancellationToken ct)
    {
        var result = await Mediator.Send(request, ct);
        return this.ApiOk(result, "Search Departments success.");
    }


    [Authorize]
    [HttpPut("{id: Guid}")]
    [ProducesResponseType(typeof(ApiResponse<Guid>) ,StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateDepartmentRequest request, [FromRoute] Guid id, CancellationToken ct)
    {
        if (request.Id != id)
        {
            throw new Shared.Common.Exceptions.ValidationException("Id mismatch", ["the route id not the same as the request id"]);
        }

        var result = await Mediator.Send(request, ct);
        return this.ApiOk(result, "Update Departments success.");
    }

    [Authorize]
    [HttpDelete("{id: Guid}")]
    [ProducesResponseType(typeof(ApiResponse<Guid>) ,StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id,  CancellationToken ct)
    {
        var request = new DeleteDepartmentRequest(id);
        var result = await Mediator.Send(request, ct);
        return this.ApiOk(result, "Delete Departments success.");
    }

    [Authorize]
    [HttpGet("{id: Guid}")]
    [ProducesResponseType(typeof(ApiResponse<DepartmentDto>) ,StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken ct)
    {
        var request = new GetDepartmentByIdRequest(id);
        var result = await Mediator.Send(request, ct);
        return this.ApiOk(result, "Get Departments by Id success.");
    }

}
