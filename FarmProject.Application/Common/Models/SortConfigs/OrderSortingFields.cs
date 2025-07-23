namespace FarmProject.Application.Common.Models.SortConfigs;

public class OrderSortingFields
{
    public const string Id = "id";
    public const string OrderDate = "orderDate";

    public static readonly Dictionary<string, string> PropertyPaths =
        new(StringComparer.OrdinalIgnoreCase)
        {
            [Id] = "Id",
            [OrderDate] = "OrderDate"
        };

    public static HashSet<string> AllowedSortFields =>
        PropertyPaths.Keys.ToHashSet(StringComparer.OrdinalIgnoreCase);
}
