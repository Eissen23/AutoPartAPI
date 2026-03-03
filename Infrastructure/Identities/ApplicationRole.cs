using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identities;

public class ApplicationRole : IdentityRole<Guid>
{
    public string? Description { get; set; }

    public int? AccessLevel { get; set; } // Optional: hierarchy support

    public bool IsSystemRole { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}
