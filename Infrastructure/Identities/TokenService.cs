using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Identities.Tokens;
using Infrastructure.Auth;
using Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Host.Authentication;
using Host.Common.Exception;
using Host.Common.Exceptions;

namespace Infrastructure.Identities;

public class TokenService(
    IOptions<JwtSettings> jwtSettings,
    UserManager<ApplicationUser> userManager,
    ApplicationDbContext context
    ) : ITokenService
{
    private readonly IOptions<JwtSettings> _jwtSettings = jwtSettings;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ApplicationDbContext _context = context;

    public async Task<TokenResponse> CreateTokenAsync(TokenRequest request)
    {
        var user = (await _userManager.FindByEmailAsync(request.LoginCredentials) 
            ?? await _userManager.FindByNameAsync(request.LoginCredentials)) ?? throw new UnauthorizedException("Invalid login credentials.");

        if (!user.IsActive)
        {
            throw new UnauthorizedException("User account is not active.");
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordValid)
        {
            throw new UnauthorizedException("Invalid login credentials.");
        }

        var token = GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.Value.RefreshTokenExpirationInDays);

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = refreshTokenExpiryTime;

        await _userManager.UpdateAsync(user);

        return new TokenResponse(token, refreshToken, refreshTokenExpiryTime);
    }

    public async Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var principal = GetPrincipalFromExpiredToken(request.Token) ?? throw new UnauthorizedException("Invalid token.");
        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedException("Invalid token.");

        var user = await _userManager.FindByIdAsync(userIdClaim.Value);
        if (user is null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime < DateTime.UtcNow)
        {
            throw new UnauthorizedException("Invalid refresh token.");
        }

        var token = GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.Value.RefreshTokenExpirationInDays);

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = refreshTokenExpiryTime;

        await _userManager.UpdateAsync(user);

        return new TokenResponse(token, refreshToken, refreshTokenExpiryTime);
    }

    public async Task<string> LogoutAsync()
    {
        await Task.CompletedTask;
        return "Logged out successfully.";
    }

    internal string GenerateJwtToken(ApplicationUser user)
    {
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Value.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
            new(ClaimTypes.Name, user.UserName ?? string.Empty),
            new("FirstName", user.FirstName ?? string.Empty),
            new("LastName", user.LastName ?? string.Empty),
        };

        // Add custom application profile claim
        var userProfileClaim = new UserApplicationClaim
        {
            UserId = Guid.Parse(user.Id),
            FirstName = user.FirstName ?? string.Empty,
            LastName = user.LastName ?? string.Empty,
            Gender = user.Gender ?? string.Empty,
            DateOfBirth = user.DateOfBirth,
            JobPositionId = user.JobPositionId ?? Guid.Empty,
            DepartmentId = user.DepartmentId ?? Guid.Empty,
            IsActive = user.IsActive
        };
        claims.Add(userProfileClaim.ToClaim());

        var userRoles = _userManager.GetRolesAsync(user).Result;
        foreach (var role in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var token = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.Value.TokenExpirationInMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        try
        {
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Value.Key);
            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            return principal;
        }
        catch
        {
            return null;
        }
    }
}
