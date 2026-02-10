using System;
using System.Collections.Generic;
using System.Text;
using Application.Identities;
using Application.Identities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Host.Common.Exception;

namespace Infrastructure.Identities;

public class UserService(
       UserManager<ApplicationUser> userManager
    ) : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<Guid> CreateUserAsync(CreateUserRequest request)
    {
        if (request.Password != request.ConfirmPassword)
        {
            throw new ArgumentException("Password and Confirm Password do not match.");
        }

        var user = new ApplicationUser
        {
            UserName = request.Username,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Gender = request.Gender,
            DateOfBirth = request.DateOfBirth,
            IsActive = true
        };

        if (request.PhoneNumber != null)
        {
            user.PhoneNumber = request.PhoneNumber;
        }

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            
            throw new InternalServerException("Failed to create user");
        }

        return Guid.Parse(user.Id);
    }

    public async Task<Guid> CreateUserByAdminAsync(CreateUserByAdminRequest request)
    {
        var user = new ApplicationUser
        {
            UserName = request.UserName,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Gender = request.Gender,
            DateOfBirth = request.DateOfBirth,
            IsActive = true,
            JobPositionId = request.JobPositionId.HasValue ? new Guid(request.JobPositionId.Value.ToString()) : Guid.Empty,
            DepartmentId = request.DepartmentId.HasValue ? new Guid(request.DepartmentId.Value.ToString()) : Guid.Empty
        };

        if (request.PhoneNumber != null)
        {
            user.PhoneNumber = request.PhoneNumber;
        }

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            throw new InternalServerException("Failed to create user");
        }

        return Guid.Parse(user.Id);
    }

    public async Task<UserDetailDto> GetCurrentUser(CancellationToken ct)
    {
        //var user = await _userManager.Users
        //    .AsNoTracking()
        //    .Where(u => u.Id == _userManager.GetUserId(Thread.CurrentPrincipal))
        throw new NotImplementedException();
    }

    public Task<Guid> UpdateUserAsync(UpdateUserRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<Guid> UpdateUserByManagerAsync(UpdateUserByManagerRequest request)
    {
        throw new NotImplementedException();
    }
}
