using FarmProject.Domain.Common;

namespace FarmProject.Application.IdentityService;

public interface IUserService
{
    public Task<Result<AuthenticationResult>> RegisterUserAsync(
        RegisterUserRequest request);
    public Task<Result<AuthenticationResult>> LoginUserAsync(
        LoginUserRequest request);
}
