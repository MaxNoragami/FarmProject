using FarmProject.Application.BreedingRabbitsService;
using FarmProject.Application.CageService;
using FarmProject.Application.CustomerService;
using FarmProject.Application.FarmTaskService;
using FarmProject.Application.OrderRequestService;
using FarmProject.Application.OrderService;
using FarmProject.Application.PairingService;
using FarmProject.Application.SacrificationService;

namespace FarmProject.Application;

public interface IUnitOfWork
{
    public IBreedingRabbitRepository BreedingRabbitRepository { get; }
    public IPairingRepository PairingRepository { get; }
    public IFarmTaskRepository FarmTaskRepository { get; }
    public ICageRepository CageRepository { get; }
    public ICustomerRepository CustomerRepository { get; }
    public IOrderRepository OrderRepository { get; }
    public IOrderRequestRepository OrderRequestRepository { get; }
    public ISacrificationRepository SacrificationRepository { get; }

    public Task SaveAsync();
    public Task BeginTransactionAsync();
    public Task CommitTransactionAsync();
    public Task RollbackTransactionAsync();
}
