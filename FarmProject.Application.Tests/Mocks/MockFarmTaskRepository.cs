using FarmProject.Application.FarmTaskService;
using FarmProject.Domain.Models;
using FarmProject.Domain.Specifications;

namespace FarmProject.Application.Tests.Mocks
{
    public class MockFarmTaskRepository : IFarmTaskRepository
    {
        private readonly List<FarmTask> _tasks = [];
        private int _nextId = 1;

        public Task<FarmTask> AddAsync(FarmTask task)
        {
            if (task.Id == 0)
                task.Id = _nextId++;

            _tasks.Add(task);
            return Task.FromResult(task);
        }

        public Task<List<FarmTask>> FindAsync(ISpecification<FarmTask> specification)
        {
            var expression = specification.ToExpression();
            var compiledExpression = expression.Compile();
            var result = _tasks.Where(compiledExpression).ToList();
            return Task.FromResult(result);
        }

        public Task<List<FarmTask>> GetAllAsync()
            => Task.FromResult(_tasks.ToList());

        public Task<FarmTask?> GetByIdAsync(int taskId)
            => Task.FromResult(_tasks.FirstOrDefault(t => t.Id == taskId));

        public Task RemoveAsync(FarmTask task)
        {
            _tasks.Remove(task);
            return Task.CompletedTask;
        }

        public Task<FarmTask> UpdateAsync(FarmTask task)
        {
            var existingTask = _tasks.FirstOrDefault(t => t.Id == task.Id);
            if (existingTask != null)
            {
                _tasks.Remove(existingTask);
                _tasks.Add(task);
            }
            return Task.FromResult(task);
        }
    }
}
