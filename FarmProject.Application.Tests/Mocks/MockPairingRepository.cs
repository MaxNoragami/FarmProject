using FarmProject.Application.PairingService;
using FarmProject.Domain.Models;

namespace FarmProject.Application.Tests.Mocks
{
    public class MockPairingRepository : IPairingRepository
    {
        private readonly List<Pair> _pairs = [];
        private int _nextId = 1;

        public Task<Pair> AddAsync(Pair pair)
        {
            if (pair.Id == 0)
                pair.Id = _nextId++;

            _pairs.Add(pair);
            return Task.FromResult(pair);
        }

        public Task<List<Pair>> GetAllAsync()
            => Task.FromResult(_pairs.ToList());

        public Task<Pair?> GetByIdAsync(int pairId)
            => Task.FromResult(_pairs.FirstOrDefault(p => p.Id == pairId));

        public Task<Pair?> GetMostRecentPairByBreedingRabbitIdsAsync(int breedingRabbitId1, int breedingRabbitId2)
        {
            return Task.FromResult(_pairs.FirstOrDefault(p =>
                p.FemaleRabbit?.Id == breedingRabbitId1 && p.MaleRabbitId == breedingRabbitId2));
        }

        public Task RemoveAsync(Pair pair)
        {
            _pairs.Remove(pair);
            return Task.CompletedTask;
        }

        public Task<Pair> UpdateAsync(Pair pair)
        {
            var existingPair = _pairs.FirstOrDefault(p => p.Id == pair.Id);
            if (existingPair != null)
            {
                _pairs.Remove(existingPair);
                _pairs.Add(pair);
            }
            return Task.FromResult(pair);
        }
    }
}
