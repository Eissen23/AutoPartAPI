using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Identities.Users;

public record UpdateUserRequest
(
    Guid Id,
    string? FirstName, 
    string? LastName, 
    string? PhoneNumber, 
    string? Email, 
    string? Gender, 
    string? Phone, 
    DateTime Dob
);

public record UpdateUserByManagerRequest
(
    Guid Id,
    string? FirstName,
    string? LastName,
    string? Email,
    string? UserName,
    string? Password,
    string? ConfirmPassword,
    string? PhoneNumber,
    string? Gender,
    DateTime DateOfBirth,
    int? AddressLocationId,
    int? JobPositionId,
    int? DepartmentId
);