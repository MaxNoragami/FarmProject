namespace FarmProject.Application.Common.Models.SortConfigs;

public class SacrificationSortingFields
{
    public const string Id = "id";
    public const string Amount = "amount";
    public const string BirthDate = "birthDate";

    public static readonly Dictionary<string, string> PropertyPaths =
        new(StringComparer.OrdinalIgnoreCase)
        {
            [Id] = "Id",
            [Amount] = "Amount",
            [BirthDate] = "BirthDate"
        };

    public static HashSet<string> AllowedSortFields =>
        PropertyPaths.Keys.ToHashSet(StringComparer.OrdinalIgnoreCase);
}
