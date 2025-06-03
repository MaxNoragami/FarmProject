using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;
using System.Linq.Expressions;

namespace FarmProject.Domain.Specifications;

public class CageSpecificationByUnoccupied(Gender gender) : ISpecification<Cage>
{
    public Expression<Func<Cage, bool>> ToExpression()
        => cage =>
            cage.OffspringCount == 0 &&
            (gender == Gender.Female
                ? cage.FemaleBreedingRabbit == null
                : cage.MaleBreedingRabbit == null);
}
