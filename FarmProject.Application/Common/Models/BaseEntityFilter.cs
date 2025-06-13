using System.Linq.Expressions;

namespace FarmProject.Application.Common.Models;

public abstract class BaseEntityFilter<T> : IEntityFilter<T>
{
    public FilterLogicalOperators LogicalOperator { get; set; } = FilterLogicalOperators.And;

    public abstract IEnumerable<Expression<Func<T, bool>>> GetExpressions();

    public Expression<Func<T, bool>> ToExpression()
    {
        var expressions = GetExpressions().ToList();

        if (!expressions.Any())
            return _ => true;

        return CombineExpressions(expressions);
    }

    private Expression<Func<T, bool>> CombineExpressions(
        List<Expression<Func<T, bool>>> expressions)
    {
        if (expressions.Count == 0)
            return _ => true;

        var result = expressions[0];

        for (int i = 1; i < expressions.Count; i++)
            result = (LogicalOperator == FilterLogicalOperators.And)
                        ? Combine(result, expressions[i], FilterLogicalOperators.And)
                        : Combine(result, expressions[i], FilterLogicalOperators.Or);

        return result;
    }

    private static Expression<Func<T, bool>> Combine(
        Expression<Func<T, bool>> expr1, 
        Expression<Func<T, bool>> expr2,
        FilterLogicalOperators filterOperator)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var body1 = ReplaceParameter(expr1.Body, expr1.Parameters[0], parameter);
        var body2 = ReplaceParameter(expr2.Body, expr2.Parameters[0], parameter);

        return (filterOperator == FilterLogicalOperators.And)
                    ? Expression.Lambda<Func<T, bool>>(
                        Expression.AndAlso(body1, body2), parameter)
                    : Expression.Lambda<Func<T, bool>>(
                        Expression.OrElse(body1, body2), parameter);
    }

    private static Expression ReplaceParameter(
        Expression body,
        ParameterExpression oldParameter,
        ParameterExpression newParameter
    )
        => new ParameterReplacer(oldParameter, newParameter).Visit(body);

    private sealed class ParameterReplacer(
            ParameterExpression oldParameter,
            ParameterExpression newParameter)
        : ExpressionVisitor
    {
        private readonly ParameterExpression _oldParameter = oldParameter;
        private readonly ParameterExpression _newParameter = newParameter;

        protected override Expression VisitParameter(ParameterExpression node)
            => (node == _oldParameter)
                ? _newParameter
                : base.VisitParameter(node);
    } 
}
