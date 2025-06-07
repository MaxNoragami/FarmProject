using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;
using System.Linq.Expressions;

namespace FarmProject.Domain.Specifications
{
    public class BreedingRabbitSpecificationByAvailable() : ISpecification<BreedingRabbit>
    {
        public Expression<Func<BreedingRabbit, bool>> ToExpression()
            => breedingRabbit => 
                breedingRabbit.BreedingStatus == BreedingStatus.Available;
    }
}
