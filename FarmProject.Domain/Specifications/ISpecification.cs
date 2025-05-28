using System.Linq.Expressions;

namespace FarmProject.Domain.Specifications;

public interface ISpecification<T>
{
    Expression<Func<T, bool>> ToExpression();
}
