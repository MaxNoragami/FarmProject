using FarmProject.Application.IdentityService;

namespace FarmProject.API.Dtos.Identity;

public record RegisterUserRequestDto(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    UserRole Role);

public static class RegisterUserRequestMapper
{
    public static RegisterUserRequest ToRegisterUserRequest(
        this RegisterUserRequestDto registerUserRequestDto)
        => new RegisterUserRequest(
            registerUserRequestDto.Email,
            registerUserRequestDto.Password,
            registerUserRequestDto.FirstName,
            registerUserRequestDto.LastName,
            registerUserRequestDto.Role);
}