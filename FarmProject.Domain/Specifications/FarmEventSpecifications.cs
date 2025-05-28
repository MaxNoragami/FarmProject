using FarmProject.Domain.Models;
using System.Linq.Expressions;

namespace FarmProject.Domain.Specifications;

public class FarmEventSpecificationsByDate(DateTime date) : ISpecification<FarmEvent>
{
    private readonly DateTime _date = date.Date;

    public Expression<Func<FarmEvent, bool>> ToExpression()
        => farmEvent => farmEvent.DueOn.Date == _date;
}

public class FarmEventSpecificationPending() : ISpecification<FarmEvent>
{
    public Expression<Func<FarmEvent, bool>> ToExpression()
        => farmEvent => !farmEvent.IsCompleted;
}