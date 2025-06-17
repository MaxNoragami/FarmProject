using FarmProject.Domain.Common;
using FarmProject.Domain.Identity;
using System.Security.Authentication;
using System.Security.Claims;

namespace FarmProject.Application.IdentityService;

public interface IUserService
{
    public Task<Result<AuthenticationResult>> RegisterUserAsync(
        RegisterUserRequest request);
    public Task<Result<AuthenticationResult>> LoginUserAsync(
        LoginUserRequest request);
}
