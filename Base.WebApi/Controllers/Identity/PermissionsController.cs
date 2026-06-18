using Base.Application.Common.Models;
using Base.Application.Identities.Permissions.Models;
using Base.Application.Identities.Permissions.Services;
using Host.Extensions;
using Shared.Common;

namespace Host.Controllers.Identity;

public class PermissionsController(
        IPermissionService permissionService
    ) : VersionedApiController
{
    private readonly IPermissionService _permissionService = permissionService;

    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody] CreatePermissionRequest request, CancellationToken ct)
    {
        var result = await _permissionService.CreateAsync(request, ct);

        return this.ApiCreated(result, "Create Permission success.");
    }

    [Authorize]
    [HttpPost("search")]
    [ProducesResponseType(typeof(ApiResponse<List<PermissionDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchAsync([FromBody] SearchPermissionsRequest request, CancellationToken ct)
    {
        var result = await _permissionService.SearchAsync(request, ct);
        return this.ApiOk(result, "Search Permissions success.");
    }

    [Authorize]
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdatePermissionRequest request, CancellationToken ct)
    {
        var result = await _permissionService.UpdateAsync(id, request, ct);
        return this.ApiOk(result, "Update Permission success.");
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await _permissionService.DeleteAsync(id, ct);
        return this.ApiOk(result, "Delete Permission success.");
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<PermissionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await _permissionService.GetByIdAsync(id, ct);
        return this.ApiOk(result, "Get Permission by Id success.");
    }
}
