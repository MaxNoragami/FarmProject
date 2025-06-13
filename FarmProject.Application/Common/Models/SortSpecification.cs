namespace FarmProject.Application.Common.Models;

public class SortSpecification
{
    public string Sort { get; set; } = string.Empty;
    public SortDirection SortDirection { get; set; } = SortDirection.Ascending;

    public List<SortOrder> ToSortOrders(HashSet<string> allowedFields)
    {
        var sortOrders = new List<SortOrder>();

        if (string.IsNullOrWhiteSpace(Sort))
            return sortOrders;

        var fields = Sort.Split(',', StringSplitOptions.RemoveEmptyEntries);

        foreach (var field in fields)
        {
            var parts = field.Trim().Split(':', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 0)
                continue;

            var propertyName = parts[0].Trim();

            if (!allowedFields.Contains(propertyName, StringComparer.OrdinalIgnoreCase))
                continue;

            var direction = SortDirection;

            if (parts.Length > 1)
            {
                if (parts[1].Equals("desc", StringComparison.OrdinalIgnoreCase))
                    direction = SortDirection.Descending;
                else if (parts[1].Equals("asc", StringComparison.OrdinalIgnoreCase))
                    direction = SortDirection.Ascending;
            }

            sortOrders.Add(new SortOrder
            {
                PropertyName = propertyName,
                Direction = direction
            }); 
        }

        return sortOrders;
    }
}
