using FarmProject.Application.Common;
using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Identity;

namespace FarmProject.Application.IdentityService;

public class ValidationUserService(
        IUserService inner,
        ValidationHelper validationHelper
    ) : IUserService
{
    private readonly IUserService _inner = inner;
    private readonly ValidationHelper _validationHelper = validationHelper;

    public Task<Result<AuthenticationResult>> LoginUserAsync(LoginUserRequest request)
        => _validationHelper.ValidateAndExecute(
                new LoginUserParam(request),
                () => _inner.LoginUserAsync(request));

    public Task<Result<AuthenticationResult>> RegisterUserAsync(RegisterUserRequest request)
        => _validationHelper.ValidateAndExecute(
                new RegisterUserParam(request),
                () => _inner.RegisterUserAsync(request));
}

public record LoginUserParam(LoginUserRequest Request);
public record RegisterUserParam(RegisterUserRequest Request);