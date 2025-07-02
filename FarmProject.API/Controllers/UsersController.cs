using FarmProject.API.Dtos.Identity;
using FarmProject.Application.IdentityService;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace FarmProject.API.Controllers;

[Route("api/users")]
public class UsersController(
        IUserService userService)
    : AppBaseController
{
    private readonly IUserService _userService = userService;

    [HttpPost]
    [Route("register")]
    public async Task<ActionResult<AuthenticationResult>> Register(RegisterUserRequestDto registerUserRequestDto)
    {
        var request = registerUserRequestDto.ToRegisterUserRequest();

        var result = await _userService.RegisterUserAsync(request);

        if (result.IsSuccess)
            return Ok(result.Value);
        else
            return HandleError<AuthenticationResult>(result.Error);
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<AuthenticationResult>> Login(LoginUserRequestDto loginUserRequestDto)
    {
        var request = loginUserRequestDto.ToLoginUserRequest();

        var result = await _userService.LoginUserAsync(request);

        if (result.IsSuccess)
            return Ok(result.Value);
        else
            return HandleError<AuthenticationResult>(result.Error);
    }
}