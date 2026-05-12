using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Application.Identities.Tokens;

public record RefreshTokenRequest (string Token, string RefreshToken);
