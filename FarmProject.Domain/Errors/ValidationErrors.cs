using FarmProject.Domain.Common;

namespace FarmProject.Domain.Errors;

public class ValidationErrors
{
    public static class Codes
    {
        public const string ValidationFailed = "Validation.Failed";
        public const string DuplicateName = "Validation.DuplicateName";
        public const string InvalidInput = "Validation.InvalidInput";
    }

    public static Error Failed(string message) =>
        new Error(Codes.ValidationFailed, message);

    public static Error DuplicateName(string entityType, string name) =>
        new Error(Codes.DuplicateName, $"A {entityType} with name '{name}' already exists.");

    public static Error InvalidInput(string message) =>
        new Error(Codes.InvalidInput, message);
}
