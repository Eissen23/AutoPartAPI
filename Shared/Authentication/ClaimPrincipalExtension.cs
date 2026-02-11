using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Shared.Authentication;

public static class ClaimPrincipalExtension
{
    public static string? GetEmail(this ClaimsPrincipal principal)
       => principal.FindFirstValue(ClaimTypes.Email);

    public static string? GetUserId(this ClaimsPrincipal principal)
       => principal.FindFirstValue(ClaimTypes.NameIdentifier);

    private static string? FindFirstValue(this ClaimsPrincipal principal, string claimType) =>
        principal is null
            ? throw new ArgumentNullException(nameof(principal))
            : principal.FindFirst(claimType)?.Value;
}
