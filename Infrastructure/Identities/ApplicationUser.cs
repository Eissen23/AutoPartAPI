using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identities;

public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public string? Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }

    public bool IsActive { get; set; }

    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }

    public Guid? JobPositionId { get; set; }
    public Guid? DepartmentId { get; set; }

    // Navigation properties
    public JobPosition? JobPosition { get; set; }
    public Department? Department { get; set; }
}
