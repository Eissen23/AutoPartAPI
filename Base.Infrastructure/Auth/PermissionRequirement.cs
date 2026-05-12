using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Base.Infrastructure.Auth;

public class PermissionRequirement : IAuthorizationRequirement 
{
}

[AttributeUsage(AttributeTargets.All)]
public class RequiredPermissionAttribute(string permission) : Attribute
{
    public string Permission { get; } = permission;
}

public class PermissionHandler
    : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        var endpoint = context.Resource as HttpContext;
        var permission = endpoint?
            .GetEndpoint()?
            .Metadata?
            .GetMetadata<RequiredPermissionAttribute>()?
            .Permission;

        if (permission != null &&
            context.User.HasClaim("Permission", permission))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}