namespace FarmProject.Application.Common.Models;

public class PaginatedResult<T>(
        int pageIndex,
        int pageSize,
        int totalPages,
        List<T> items
    ) where T : class
{
    public int PageIndex => pageIndex;
    public int PageSize => pageSize;
    public int TotalPages => totalPages;
    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;
    public IReadOnlyCollection<T> Items => items;
}
