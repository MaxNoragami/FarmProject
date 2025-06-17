namespace FarmProject.Domain.Identity;

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