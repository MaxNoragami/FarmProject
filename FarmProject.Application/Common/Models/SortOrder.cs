namespace FarmProject.Application.Common.Models;

public class SortOrder
{
    public string PropertyName { get; set; } = string.Empty;
    public SortDirection Direction { get; set; } = SortDirection.Ascending;
}
