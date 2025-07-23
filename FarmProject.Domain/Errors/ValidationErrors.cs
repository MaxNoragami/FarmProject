using FarmProject.Domain.Common;

namespace FarmProject.Domain.Errors;

public static class ValidationErrors
{
    public static class Codes
    {
        public const string ValidationFailed = "Validation.Failed";
        public const string DuplicateName = "Validation.DuplicateName";
        public const string DuplicateEmail = "Validation.DuplicateEmail";
        public const string DuplicatePhoneNum = "Validation.DuplicatePhoneNum";
        public const string InvalidInput = "Validation.InvalidInput";
    }

    public static Error Failed(string message) =>
        new Error(Codes.ValidationFailed, message);

    public static Error DuplicateName(string entityType, string name) =>
        new Error(Codes.DuplicateName, $"A {entityType} with name '{name}' already exists.");

    public static Error DuplicateEmail(string entityType, string email) =>
        new Error(Codes.DuplicateEmail, $"A {entityType} with email '{email}' already exists.");

    public static Error DuplicatePhoneNum(string entityType, string phoneNum) =>
        new Error(Codes.DuplicatePhoneNum, $"A {entityType} with phone number '{phoneNum}' already exists.");

    public static Error InvalidInput(string message) =>
        new Error(Codes.InvalidInput, message);
}
