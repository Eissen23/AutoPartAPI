using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Auth;

public class CurrentUserMiddleware(
        ICurrentUserInitializer currentUserInitializer
    ) : IMiddleware
{
    private readonly ICurrentUserInitializer _currentUserInitializer = currentUserInitializer;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        _currentUserInitializer.SetCurrentUser(context.User);
        await next(context);
    }
}
