using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Common.Exceptions;

namespace Infrastructure.Auth;

public class ConfigureJwtBearerOption(IOptions<JwtSettings> jwtSettings) : IConfigureNamedOptions<JwtBearerOptions>
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public void Configure(JwtBearerOptions options)
    {
        Configure(Options.DefaultName, options);
    }

    public void Configure(string? name, JwtBearerOptions options)
    {
        if (name != JwtBearerDefaults.AuthenticationScheme)
            return;

        byte[] key = Encoding.ASCII.GetBytes(_jwtSettings.Key);

        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateLifetime = true,
            ValidateAudience = false,
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.Zero
        };
        options.Events = new JwtBearerEvents
        {
            OnForbidden = _ => throw new ForbiddenException("You are not authorized to access this resource."),
            OnMessageReceived = context =>
            {
                Console.WriteLine($"[JWT] {context.Request.Method} {context.Request.Path}");
                Console.WriteLine("JWT HEADER: " + context.Request.Headers["Authorization"]);
                return Task.CompletedTask;
            },
        };
    }
}
