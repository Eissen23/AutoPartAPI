using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Interface;

namespace Application.Identities.Tokens;

public interface ITokenService : IScopedService
{
    Task<TokenResponse> CreateTokenAsync(TokenRequest request);
    Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request);

    Task<string> LogoutAsync();
}
