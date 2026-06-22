using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Application.Identities.Users;

public record CreateUserRequest 
{
    public string Email { get; init; } = default!;
    public string Username { get; set; } = default!;
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
    public DateTime DateOfBirth { get; init; }

    public string Password { get; init; } = default!;
    public string? ConfirmPassword { get; init; } = default!;
    public string? PhoneNumber { get; init; } 
};

public record CreateUserByAdminRequest
{

    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
    public string Email { get; init; } = default!;
    public string Username { get; init; } = default!;
    public string Password { get; init; } = default!;

    public string? ConfirmPassword { get; init; }

    public string? PhoneNumber { get; init; }
    public string? Gender { get; init; }
    public DateTime DateOfBirth { get; init; }
    public int? AddressLocationId { get; init; }
    public int? JobPositionId { get; init; }
    public int? DepartmentId { get; init; }

}