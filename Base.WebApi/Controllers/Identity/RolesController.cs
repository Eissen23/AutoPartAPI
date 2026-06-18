using Base.Application.Common.Models;
using Base.Application.Identities.Roles.Models;
using Base.Application.Identities.Roles.Services;
using Base.Application.Identities.Permissions.Models;
using Host.Extensions;
using Shared.Common;

namespace Host.Controllers.Identity;

public class RolesController(
        IRoleService roleService
    ) : VersionedApiController
{
    private readonly IRoleService _roleService = roleService;

    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateRoleRequest request, CancellationToken ct)
    {
        var result = await _roleService.CreateAsync(request, ct);

        return this.ApiCreated(result, "Create Role success.");
    }

    [Authorize]
    [HttpPost("search")]
    [ProducesResponseType(typeof(ApiResponse<List<RoleDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchAsync([FromBody] SearchRolesRequest request, CancellationToken ct)
    {
        var result = await _roleService.SearchAsync(request, ct);
        return this.ApiOk(result, "Search Roles success.");
    }

    [Authorize]
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateRoleRequest request, CancellationToken ct)
    {
        var result = await _roleService.UpdateAsync(id, request, ct);
        return this.ApiOk(result, "Update Role success.");
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await _roleService.DeleteAsync(id, ct);
        return this.ApiOk(result, "Delete Role success.");
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<RoleDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await _roleService.GetByIdAsync(id, ct);
        return this.ApiOk(result, "Get Role by Id success.");
    }

    [Authorize]
    [HttpPost("{id:guid}/permissions")]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AssignPermissionsAsync([FromRoute] Guid id, [FromBody] AssignPermissionsToRoleRequest request, CancellationToken ct)
    {
        await _roleService.AssignPermissionsAsync(id, request.PermissionIds, ct);
        return this.ApiOk(id, "Assign permissions to role success.");
    }

    [Authorize]
    [HttpDelete("{id:guid}/permissions")]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemovePermissionsAsync([FromRoute] Guid id, [FromBody] AssignPermissionsToRoleRequest request, CancellationToken ct)
    {
        await _roleService.RemovePermissionsAsync(id, request.PermissionIds, ct);
        return this.ApiOk(id, "Remove permissions from role success.");
    }

    [Authorize]
    [HttpGet("{id:guid}/permissions")]
    [ProducesResponseType(typeof(ApiResponse<List<PermissionDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPermissionsAsync([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await _roleService.GetRolePermissionsAsync(id, ct);
        return this.ApiOk(result, "Get Role permissions success.");
    }
}
