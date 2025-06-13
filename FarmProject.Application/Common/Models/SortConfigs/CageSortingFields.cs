namespace FarmProject.Application.Common.Models.SortConfigs;

public class CageSortingFields
{
    public const string Id = "id";
    public const string Name = "name";

    public static readonly Dictionary<string, string> PropertyPaths =
        new(StringComparer.OrdinalIgnoreCase)
        {
            [Id] = "Id",
            [Name] = "Name"
        };

    public static HashSet<string> AllowedSortFields =>
        PropertyPaths.Keys.ToHashSet(StringComparer.OrdinalIgnoreCase);
}
