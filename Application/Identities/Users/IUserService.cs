using System;
using System.Collections.Generic;
using System.Text;
using Base.Application.Common.Interface;

namespace Base.Application.Identities.Users;

public interface IUserService : ITransientService
{
    Task<Guid> CreateUserAsync(CreateUserRequest request);
    Task<Guid> UpdateUserAsync(UpdateUserRequest request);

    Task<Guid> CreateUserByAdminAsync(CreateUserByAdminRequest request);
    Task<Guid> UpdateUserByManagerAsync(UpdateUserByManagerRequest request);

    Task<UserDetailDto> GetCurrentUser(CancellationToken ct);
}
