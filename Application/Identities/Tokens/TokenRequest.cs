using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Identities.Tokens;

public record TokenRequest(string LoginCredentials, string Password);

public record TokenResponse(string Token, string RefreshToken, DateTime RefreshTokenExpiryTime);