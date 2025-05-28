using FarmProject.Domain.Models;

namespace FarmProject.Application.RabbitsService;

public interface IRabbitRepository
{
    public Task<Rabbit> AddAsync(Rabbit rabbit);
    public Task<Rabbit?> GetByIdAsync(int rabbitId);
    public Task<List<Rabbit>> GetAllAsync();
    public Task<Rabbit> UpdateAsync(Rabbit rabbit);
    public Task RemoveAsync(Rabbit rabbit);
}
