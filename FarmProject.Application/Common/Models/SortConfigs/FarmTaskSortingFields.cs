namespace FarmProject.Application.Common.Models.SortConfigs;

public class FarmTaskSortingFields
{
    public const string Id = "id";
    public const string CreatedOn = "createdOn";
    public const string DueOn = "dueOn";

    public static readonly Dictionary<string, string> PropertyPaths =
        new(StringComparer.OrdinalIgnoreCase)
        {
            [Id] = "Id",
            [CreatedOn] = "CreatedOn",
            [DueOn] = "DueOn"
        };

    public static HashSet<string> AllowedSortFields =>
        PropertyPaths.Keys.ToHashSet(StringComparer.OrdinalIgnoreCase);
}
