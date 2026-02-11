using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Middlewares;

internal static class Startup
{
    internal static IServiceCollection AddExceptionMiddleware(this IServiceCollection services) =>
        services.AddScoped<ExceptionMiddleware>();

    internal static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder) =>
        builder.UseMiddleware<ExceptionMiddleware>();
}
