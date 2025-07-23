namespace FarmProject.Application.Common.Models.SortConfigs;

public class CustomerSortingFields
{
    public const string Id = "id";
    public const string FirstName = "firstName";
    public const string LastName = "lastName";
    public const string Email = "email";

    public static readonly Dictionary<string, string> PropertyPaths =
        new(StringComparer.OrdinalIgnoreCase)
        {
            [Id] = "Id",
            [FirstName] = "FirstName",
            [LastName] = "LastName",
            [Email] = "Email"
        };

    public static HashSet<string> AllowedSortFields =>
        PropertyPaths.Keys.ToHashSet(StringComparer.OrdinalIgnoreCase);
}
