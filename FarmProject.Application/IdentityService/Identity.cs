namespace FarmProject.Application.IdentityService;

public record AuthenticationResult(
    string Token);

public record LoginUserRequest(
    string Email,
    string Password);

public record RegisterUserRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    UserRole Role);

public enum UserRole
{
    Logistics,
    Worker
}
