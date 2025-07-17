namespace FarmProject.Application.Common.Models.SortConfigs;

public class OrderRequestSortingFields
{
    public const string Id = "id";
    public const string Amount = "amount";

    public static readonly Dictionary<string, string> PropertyPaths =
        new(StringComparer.OrdinalIgnoreCase)
        {
            [Id] = "Id",
            [Amount] = "Amount"
        };

    public static HashSet<string> AllowedSortFields =>
        PropertyPaths.Keys.ToHashSet(StringComparer.OrdinalIgnoreCase);
}
