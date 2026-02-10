using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Host.Authentication;

/// <summary>
/// Base class for custom application claims.
/// </summary>
public abstract class BaseClaim
{
    /// <summary>
    /// Gets the claim type for this custom claim.
    /// </summary>
    public abstract string ClaimType { get; }

    /// <summary>
    /// Gets the claim value.
    /// </summary>
    public abstract string ClaimValue { get; }

    /// <summary>
    /// Converts the custom claim to a System.Security.Claims.Claim object.
    /// </summary>
    public Claim ToClaim()
    {
        return new Claim(ClaimType, ClaimValue);
    }
}

/// <summary>
/// Custom claim for user application profile information.
/// </summary>
public class UserApplicationClaim : BaseClaim
{
    public Guid UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}".Trim();
    public string Gender { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public Guid JobPositionId { get; set; }
    public Guid DepartmentId { get; set; }
    public bool IsActive { get; set; }

    public override string ClaimType => "user_application_profile";

    public override string ClaimValue
    {
        get
        {
            var claimData = new
            {
                UserId,
                FirstName,
                LastName,
                Gender,
                DateOfBirth,
                JobPositionId,
                DepartmentId,
                IsActive
            };

            return System.Text.Json.JsonSerializer.Serialize(claimData);
        }
    }

    public static UserApplicationClaim? FromClaim(Claim claim)
    {
        if (claim.Type != "user_application_profile")
        {
            return null;
        }

        try
        {
            var data = System.Text.Json.JsonDocument.Parse(claim.Value);
            var root = data.RootElement;

            return new UserApplicationClaim
            {
                UserId = Guid.Parse(root.GetProperty(nameof(UserId)).GetString() ?? Guid.Empty.ToString()),
                FirstName = root.GetProperty(nameof(FirstName)).GetString() ?? string.Empty,
                LastName = root.GetProperty(nameof(LastName)).GetString() ?? string.Empty,
                Gender = root.GetProperty(nameof(Gender)).GetString() ?? string.Empty,
                DateOfBirth = root.TryGetProperty(nameof(DateOfBirth), out var dob) && dob.ValueKind != System.Text.Json.JsonValueKind.Null
                    ? DateTime.Parse(dob.GetString() ?? DateTime.MinValue.ToString())
                    : null,
                JobPositionId = Guid.Parse(root.GetProperty(nameof(JobPositionId)).GetString() ?? Guid.Empty.ToString()),
                DepartmentId = Guid.Parse(root.GetProperty(nameof(DepartmentId)).GetString() ?? Guid.Empty.ToString()),
                IsActive = root.GetProperty(nameof(IsActive)).GetBoolean()
            };
        }
        catch
        {
            return null;
        }
    }
}

/// <summary>
/// Custom claim for user permissions/capabilities.
/// </summary>
public class UserPermissionsClaim : BaseClaim
{
    public List<string> Permissions { get; set; } = [];

    public override string ClaimType => "user_permissions";

    public override string ClaimValue
    {
        get => string.Join(",", Permissions);
    }

    public static UserPermissionsClaim? FromClaim(Claim claim)
    {
        if (claim.Type != "user_permissions")
        {
            return null;
        }

        var permissions = claim.Value.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
        return new UserPermissionsClaim { Permissions = permissions };
    }
}
