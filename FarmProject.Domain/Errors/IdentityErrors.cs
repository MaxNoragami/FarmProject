using FarmProject.Domain.Common;

namespace FarmProject.Domain.Errors;

public static class IdentityErrors
{
    public static class Codes
    {
        public const string RegistrationFailed = "Validation.RegistrationFailed";
    }

    public static Error InvalidCredentials => new(
        "Identity.InvalidCredentials", "Invalid email or password introduced");

    public static Error UserAlreadyExists => new(
        "Identity.UserAlreadyExists", "A user with this email already exists");

    public static Error InvalidRole => new(
        "Identity.InvalidRole", "The specified role is not valid");

    public static Error RegistrationFailed(string details)
        => new(
            Codes.RegistrationFailed, $"Registration failed: {details}");
}
