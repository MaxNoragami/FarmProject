using FarmProject.Application.IdentityService;

namespace FarmProject.API.Dtos.Identity;

public record LoginUserRequestDto(
    string Email,
    string Password);

public static class LoginUserRequestMapper
{
    public static LoginUserRequest ToLoginUserRequest(
        this LoginUserRequestDto loginUserRequestDto)
        => new LoginUserRequest(
            loginUserRequestDto.Email,
            loginUserRequestDto.Password);
}