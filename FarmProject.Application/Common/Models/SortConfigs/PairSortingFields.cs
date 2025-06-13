namespace FarmProject.Application.Common.Models.SortConfigs;

public class PairSortingFields
{
    public const string Id = "id";
    public const string MaleRabbitId = "maleRabbitId";
    public const string StartDate = "startDate";
    public const string EndDate = "endDate";

    public static readonly Dictionary<string, string> PropertyPaths =
        new(StringComparer.OrdinalIgnoreCase)
        {
            [Id] = "Id",
            [MaleRabbitId] = "MaleRabbitId",
            [StartDate] = "StartDate",
            [EndDate] = "EndDate"
        };

    public static HashSet<string> AllowedSortFields =>
        PropertyPaths.Keys.ToHashSet(StringComparer.OrdinalIgnoreCase);
}
