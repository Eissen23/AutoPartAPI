using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Application.Common.Interface;

public interface ICurrentUser
{
    string? Name { get; }

    Guid GetUserId();

    string? GetUserEmail();

    bool IsAuthenticated();

    IEnumerable<Claim>? GetUserClaims();
}
