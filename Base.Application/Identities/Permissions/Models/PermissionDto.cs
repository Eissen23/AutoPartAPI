using System;
using System.Collections.Generic;
using System.Text;
using Base.Application.Common.Interface;

namespace Base.Application.Identities.Permissions.Models;

public class PermissionDto : IDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
