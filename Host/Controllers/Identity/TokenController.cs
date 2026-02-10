using Application.Identities.Tokens;
using Host.Extensions;

namespace Host.Controllers.Identity;

public class TokenController(
        ITokenService tokenService
    ) : VersionedApiController
{
    private readonly ITokenService _tokenService = tokenService;

    [AllowAnonymous, HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateToken([FromBody] TokenRequest request)
    {
        var result = await _tokenService.CreateTokenAsync(request);
        return this.ApiOk<TokenResponse>(result, "Successfully create token");
    }

    [Authorize, HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Logout()
    {
        var result = await _tokenService.LogoutAsync();

        return this.ApiOk(result ,"Successfully logged out");
    }
}
