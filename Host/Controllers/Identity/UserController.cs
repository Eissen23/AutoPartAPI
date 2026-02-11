using Application.Identities.Users;
using Host.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Validations.Rules;

namespace Host.Controllers.Identity;

public class UserController(
        IUserService userService
    ) : VersionedApiController
{
    private readonly IUserService _userService = userService;


    /// <summary>
    /// Signup user with the create user request
    /// </summary>
    [AllowAnonymous, HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserRequest request)
    {
        var result = await _userService.CreateUserAsync(request);

        return this.ApiOk(result, "User created successfully");
    }

    /// <summary>
    /// Create user by admin
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [Authorize, HttpPost("create-by-admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUserByAdminAsync([FromBody] CreateUserByAdminRequest request)
    {
        var result = await _userService.CreateUserByAdminAsync(request);

        return this.ApiOk(result, "User created by admin successfully");
    }


    /// <summary>
    /// Update user
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [Authorize, HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserRequest request)
    {
        var result = await _userService.UpdateUserAsync(request);

        return this.ApiOk(result, "User updated successfully");
    }


    /// <summary>
    /// Update user by manager
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [Authorize, HttpPut("update-by-manager")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUserAdminAsync([FromBody] UpdateUserByManagerRequest request)
    {
        var result = await _userService.UpdateUserByManagerAsync(request);

        return this.ApiOk(result, "User updated successfully");
    }

    [Authorize]
    [HttpGet("profile")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUserProfileAsync(CancellationToken ct)
    {
        var result = await _userService.GetCurrentUser(ct);
        return this.ApiOk(result, "User profile retrieved successfully");
    }
}
