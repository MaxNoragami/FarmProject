using FarmProject.Application.FarmTaskService;
using FarmProject.Application.PairingService;
using FarmProject.Application.BreedingRabbitsService;

namespace FarmProject.Application;

public interface IUnitOfWork
{
    public IBreedingRabbitRepository BreedingRabbitRepository { get; }
    public IPairingRepository PairingRepository { get; }
    public IFarmTaskRepository FarmTaskRepository { get; }

    public Task SaveAsync();
    public Task BeginTransactionAsync();
    public Task CommitTransactionAsync();
    public Task RollbackTransactionAsync();
}
