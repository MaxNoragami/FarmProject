using FarmProject.Application.Common;
using FarmProject.Domain.Common;

namespace FarmProject.Application.IdentityService;

public class LoggingUserService(
        IUserService userService,
        LoggingHelper loggingHelper) 
    : IUserService
{
    private readonly IUserService _userService = userService;
    private readonly LoggingHelper _loggingHelper = loggingHelper;

    public async Task<Result<AuthenticationResult>> LoginUserAsync(LoginUserRequest request)
        => await _loggingHelper.LogOperation(
            LoggingUtilities.FormatMethodCall(
                nameof(LoginUserAsync),
                (nameof(request), request)
            ),
            async () =>
                await _userService.LoginUserAsync(request));

    public async Task<Result<AuthenticationResult>> RegisterUserAsync(RegisterUserRequest request)
        => await _loggingHelper.LogOperation(
            LoggingUtilities.FormatMethodCall(
                nameof(RegisterUserAsync),
                (nameof(request), request)
            ),
            async () =>
                await _userService.RegisterUserAsync(request));
}
