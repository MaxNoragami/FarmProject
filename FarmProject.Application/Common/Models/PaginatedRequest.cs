namespace FarmProject.Application.Common.Models;

public class PaginatedRequest<TFilter>
{
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public TFilter Filter { get; set; }
    public SortSpecification Sort { get; set; } = new();
}
