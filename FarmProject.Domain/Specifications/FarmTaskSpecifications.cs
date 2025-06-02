using FarmProject.Domain.Models;
using System.Linq.Expressions;

namespace FarmProject.Domain.Specifications;

public class FarmTaskSpecificationsByDate(DateTime date) : ISpecification<FarmTask>
{
    private readonly DateTime _date = date.Date;

    public Expression<Func<FarmTask, bool>> ToExpression()
        => farmTask => farmTask.DueOn.Date == _date;
}

public class FarmTaskSpecificationPending() : ISpecification<FarmTask>
{
    public Expression<Func<FarmTask, bool>> ToExpression()
        => farmTask => !farmTask.IsCompleted;
}