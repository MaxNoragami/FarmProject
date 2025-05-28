using FarmProject.Application.FarmEventsService;
using FarmProject.Application.PairingService;
using FarmProject.Application.RabbitsService;

namespace FarmProject.Application;

public interface IUnitOfWork
{
    public IRabbitRepository RabbitRepository { get; }
    public IPairingRepository PairingRepository { get; }
    public IFarmEventRepository FarmEventRepository { get; }

    public Task SaveAsync();
    public Task BeginTransactionAsync();
    public Task CommitTransactionAsync();
    public Task RollbackTransactionAsync();
}
