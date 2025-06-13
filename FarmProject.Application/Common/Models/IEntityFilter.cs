using System.Linq.Expressions;

namespace FarmProject.Application.Common.Models;

public interface IEntityFilter<T>
{
    Expression<Func<T, bool>> ToExpression();
    FilterLogicalOperators LogicalOperator { get; set; }
}
