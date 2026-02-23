using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Identities.Tokens;
using Host.Authentication;
using Shared.Common.Exceptions;
using Infrastructure.Auth;
using Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Identities;

public class TokenService(
    IOptions<JwtSettings> jwtSettings,
    UserManager<ApplicationUser> userManager,
    ApplicationDbContext context
    ) : ITokenService
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ApplicationDbContext _context = context;

    public async Task<TokenResponse> CreateTokenAsync(TokenRequest request, HttpContext httpContext)
    {
        var user = await FindUserByEmailOrName(request.LoginCredentials.Trim().Normalize())
            ?? throw new NotFoundException($"User with Username: {request.LoginCredentials} not found");

        if (!user.IsActive)
        {
            throw new UnauthorizedException("User account is not active.");
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordValid)
        {
            throw new UnauthorizedException("Invalid login credentials.");
        }

        var token = await GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays);

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = refreshTokenExpiryTime;

        await _userManager.UpdateAsync(user);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true, // Chỉ sử dụng trên HTTPS
            Expires = DateTimeOffset.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays) // Thời gian tồn tại của cookie
        };

        httpContext.Response.Cookies.Append("jwtToken", token, cookieOptions);
        httpContext.Response.Cookies.Append("refreshToken", user.RefreshToken, cookieOptions);
        httpContext.Response.Cookies.Append("refreshTokenExpiryTime", user.RefreshTokenExpiryTime.ToString("o"), cookieOptions);

        return new TokenResponse(token, refreshToken, refreshTokenExpiryTime);
    }

    private async Task<ApplicationUser?> FindUserByEmailOrName(string loginCredentials)
    {
        var user = await _userManager.FindByEmailAsync(loginCredentials);

        if (user != null)
        {
            return user;
        }

        // Nếu không tìm thấy bằng email, thử tìm bằng username
        user = await _userManager.FindByNameAsync(loginCredentials);

        return user;
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

        var token = await GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays);

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

    internal async Task<string> GenerateJwtToken(ApplicationUser user)
    {
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
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

        var userRoles = await _userManager.GetRolesAsync(user);
        foreach (var role in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var token = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes),
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
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
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
