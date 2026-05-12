using Base.Application.Identities.Tokens;
using Host.Extensions;
using Shared.Common;

namespace Host.Controllers.Identity;

public class TokenController(
        ITokenService tokenService
    ) : VersionedApiController
{
    private readonly ITokenService _tokenService = tokenService;

    [AllowAnonymous, HttpPost]
    [ProducesResponseType(typeof(ApiResponse<TokenResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateToken([FromBody] TokenRequest request)
    {
        var result = await _tokenService.CreateTokenAsync(request, HttpContext);
        return this.ApiOk(result, "Successfully create token");
    }

    [Authorize, HttpPost("logout")]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Logout()
    {
        var result = await _tokenService.LogoutAsync();

        return this.ApiOk(result, "Successfully logged out");
    }

    [Authorize, HttpPost("refresh")]
    [ProducesResponseType(typeof(ApiResponse<TokenResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var result = await _tokenService.RefreshTokenAsync(request);
        return this.ApiOk(result, "Successfully refresh token");
    }
}
