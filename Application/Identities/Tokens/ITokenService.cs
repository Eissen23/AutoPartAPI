using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Interface;
using Microsoft.AspNetCore.Http;

namespace Application.Identities.Tokens;

public interface ITokenService : IScopedService
{
    Task<TokenResponse> CreateTokenAsync(TokenRequest request, HttpContext httpContext);
    Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request);

    Task<string> LogoutAsync();
}
