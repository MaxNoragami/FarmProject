using System.Linq.Expressions;
using System.Reflection;

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

    public static IQueryable<T> ApplySorting<T>(
        this IQueryable<T> query,
        IList<SortOrder> sortOrders,
        IDictionary<string, string>? fieldMappings = null)
    {
        if (sortOrders == null || !sortOrders.Any())
            return query;

        IOrderedQueryable<T>? orderedQuery = null;
        bool isFirst = true;

        foreach (var sortOrder in sortOrders)
        {
            string propertyPath = sortOrder.PropertyName;
            if (fieldMappings != null &&
                fieldMappings.TryGetValue(sortOrder.PropertyName, out var mappedPath))
                    propertyPath = mappedPath;

            if (isFirst)
            {
                orderedQuery = ApplyOrder(query, propertyPath, sortOrder.Direction, true);
                isFirst = false;
            }
            else if (orderedQuery != null)
                orderedQuery = ApplyOrder(orderedQuery, propertyPath, sortOrder.Direction, false);      
        }

        return orderedQuery ?? query;
    }

    private static IOrderedQueryable<T>? ApplyOrder<T>(
    IQueryable<T> query,
    string propertyPath,
    SortDirection direction,
    bool isFirstSort)
    {
        var parameter = Expression.Parameter(typeof(T), "x");

        var propertyExpression = GetPropertyExpression(parameter, propertyPath);
        if (propertyExpression == null)
            return isFirstSort ? query.OrderBy(x => 0) : (IOrderedQueryable<T>)query;

        var propertyType = propertyExpression.Type;
        var lambdaExpression = Expression.Lambda(propertyExpression, parameter);

        string methodName;
        if (isFirstSort)
            methodName = direction == SortDirection.Ascending ? "OrderBy" : "OrderByDescending";
        else
            methodName = direction == SortDirection.Ascending ? "ThenBy" : "ThenByDescending";

        var sortMethod = typeof(Queryable).GetMethods()
            .Where(m => m.Name == methodName && m.IsGenericMethodDefinition)
            .Where(m => m.GetParameters().Length == 2)
            .Single();

        var genericMethod = sortMethod.MakeGenericMethod(typeof(T), propertyType);

        return genericMethod.Invoke(
            null, [query, lambdaExpression]) as IOrderedQueryable<T>;
    }

    private static Expression? GetPropertyExpression(ParameterExpression parameter, string propertyPath)
    {
        var properties = propertyPath.Split('.');
        Expression propertyExpression = parameter;

        foreach (var property in properties)
        {
            var propertyInfo = propertyExpression.Type.GetProperty(property,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo == null)
                return null;

            propertyExpression = Expression.Property(propertyExpression, propertyInfo);
        }

        return propertyExpression;
    }
}
