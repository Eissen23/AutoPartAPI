using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Identities.Users;

public record UserDetailDto(
    string Id,
    string? UserName,
    string? Email,
    string? PhoneNumber,
    string? FirstName,
    string? LastName,
    string? Gender,
    DateTime? DateOfBirth,
    bool IsActive,
    Guid JobPositionId,
    Guid DepartmentId,
    string? JobPositionName,
    string? DepartmentName
);

