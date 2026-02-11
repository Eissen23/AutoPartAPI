using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Application.Common.Interface;
using Infrastructure.Identities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure.Auth;

internal static class Startup
{
    public static IServiceCollection AddAuth(this IServiceCollection services)
    {

        services
            .AddCurrentUser()
            .AddIdentities()
            .AddJwtAuth()
            .AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
            });

        return services;
    }

    private static IServiceCollection AddJwtAuth(this IServiceCollection services)
    {
        // Works already
        services.AddOptions<JwtSettings>()
            .BindConfiguration($"SecuritySettings:{nameof(JwtSettings)}")
            .ValidateDataAnnotations()
            .ValidateOnStart();

        // Handle configure for JwtBearerOptions
        services.AddSingleton<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOption>();

        services
               .AddAuthentication(authentication =>
               {
                   authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                   authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
               })
                .AddJwtBearer();

        return services;
    }


    private static IServiceCollection AddCurrentUser(this IServiceCollection services) =>
        services
            .AddScoped<CurrentUserMiddleware>()
            .AddScoped<ICurrentUser, CurrentUser>()
            .AddScoped(sp => (ICurrentUserInitializer)sp.GetRequiredService<ICurrentUser>());

    internal static IApplicationBuilder UseCurrentUser(this IApplicationBuilder app) =>
        app.UseMiddleware<CurrentUserMiddleware>();

}
