namespace FarmProject.Application.Common.Models;

public static class QueryableExtension
{
    public static IQueryable<T> ApplyFilter<T>(
        this IQueryable<T> query,
        IEntityFilter<T>? filter)
    {
        if (filter == null)
            return query;

        var expression = filter.ToExpression();
        return query.Where(expression);
    }
}
