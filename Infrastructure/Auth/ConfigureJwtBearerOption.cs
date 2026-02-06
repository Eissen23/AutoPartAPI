using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Common.Exception;
using Shared.Common.Exceptions;

namespace Infrastructure.Auth;

public class ConfigureJwtBearerOption(IOptions<JwtSettings> jwtSettings) : IConfigureOptions<JwtBearerOptions>
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public void Configure(JwtBearerOptions options)
    {
        throw new NotImplementedException();
    }

    public void Configure(string name, JwtBearerOptions options)
    {
        if (name != JwtBearerDefaults.AuthenticationScheme)
        {
            return;
        }

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
            OnChallenge = context =>
            {
                context.HandleResponse();
                if (!context.Response.HasStarted)
                {
                    throw new UnauthorizedException("Authentication Failed.");
                }

                return Task.CompletedTask;
            },
            OnForbidden = _ => throw new ForbiddenException("You are not authorized to access this resource."),
            OnMessageReceived = context =>
            {
                if (context.HttpContext.Request.Cookies.TryGetValue("jwtToken", out string? accessToken))
                {
                    context.Token = accessToken; 
                }

                return Task.CompletedTask;
            }
        };
    }
}
