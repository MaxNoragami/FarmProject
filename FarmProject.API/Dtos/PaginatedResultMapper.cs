using FarmProject.Application.Common.Models;

namespace FarmProject.API.Dtos;

public static class PaginatedResultMapper
{
    public static PaginatedResult<T> ToPaginatedResult<G, T>(
        this PaginatedResult<G> paginatedResult,
        List<T> newItems
    ) 
            where G : class
            where T : class
        => new PaginatedResult<T>(
            pageIndex: paginatedResult.PageIndex,
            pageSize: paginatedResult.PageIndex,
            totalPages: paginatedResult.TotalPages,
            items: newItems);
}
