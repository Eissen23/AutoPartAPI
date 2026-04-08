using Application.Common.Models;
using Application.Identities.Departments.Models;
using Application.Identities.Departments.Services;
using Host.Extensions;
using Shared.Common;

namespace Host.Controllers.Identity;

public class DepartmentsController(
        IDepartmentService departmentService
    ) : VersionedApiController
{

    private readonly IDepartmentService _departmentService = departmentService;

    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<Guid>) ,StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateDepartmentRequest request, CancellationToken ct)
    {
        var result = await _departmentService.CreateAsync(request, ct);

        return this.ApiOk(result, "Create Departments success.");
    }

    [Authorize]
    [HttpPost("search")]
    [ProducesResponseType(typeof(ApiResponse<PaginatedResponse<DepartmentDto>>) ,StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchAsync([FromBody] SearchDepartmentRequest request, CancellationToken ct)
    {
        var result = await _departmentService.GetPaginated(request, ct);
        return this.ApiOk(result, "Search Departments success.");
    }


    [Authorize]
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<Guid>) ,StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateDepartmentRequest request, CancellationToken ct)
    {
        var result = await _departmentService.UpdateAsync(id, request, ct);
        return this.ApiOk(result, "Update Departments success.");
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<Guid>) ,StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id,  CancellationToken ct)
    {
        var result = await _departmentService.DeleteAsync(id, ct);
        return this.ApiOk(result, "Delete Departments success.");
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<DepartmentDto>) ,StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken ct )
    {
        var result = await _departmentService.GetByIdAsync(id, ct);
        return this.ApiOk(result, "Get Departments by Id success.");
    }

}
