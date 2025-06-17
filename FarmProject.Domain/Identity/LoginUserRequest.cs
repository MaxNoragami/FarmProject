namespace FarmProject.Domain.Identity;

public record LoginUserRequest(
    string Email,
    string Password);
