using FarmProject.Domain.Models;

namespace FarmProject.Domain.Specifications;

public class FarmEventSpecificationsByDate(DateTime date) : ISpecification<FarmEvent>
{
    private readonly DateTime _date = date.Date;

    public bool IsSatisfiedBy(FarmEvent entity)
        => entity.DueOn.Date == _date;
}

public class FarmEventSpecificationPending() : ISpecification<FarmEvent>
{
    public bool IsSatisfiedBy(FarmEvent entity)
        => !entity.IsCompleted;
}