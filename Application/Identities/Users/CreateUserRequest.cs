using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Identities.Users;

public record CreateUserRequest(
    string Email,
    string Username,
    string FirstName,
    string LastName,
    string Gender,
    DateTime DateOfBirth,
    string Password,
    string ConfirmPassword,
    string? PhoneNumber
);

public record CreateUserByAdminRequest(
    string FirstName,
    string LastName,
    string Email,
    string UserName,
    string Password,
    string? ConfirmPassword,
    string? PhoneNumber,
    string? Gender,
    DateTime DateOfBirth,
    int? AddressLocationId,
    int? JobPositionId,
    int? DepartmentId
);