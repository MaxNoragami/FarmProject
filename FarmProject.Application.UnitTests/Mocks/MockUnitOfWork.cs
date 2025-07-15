using FarmProject.Application.BreedingRabbitsService;
using FarmProject.Application.CageService;
using FarmProject.Application.CustomerService;
using FarmProject.Application.FarmTaskService;
using FarmProject.Application.PairingService;
using FarmProject.Infrastructure.Repositories;

namespace FarmProject.Application.UnitTests.Mocks;

public class MockUnitOfWork(
        IBreedingRabbitRepository breedingRabbitRepository = null,
        IPairingRepository pairingRepository = null,
        IFarmTaskRepository farmTaskRepository = null,
        ICageRepository cageRepository = null,
        ICustomerRepository customerRepository = null) 
    : IUnitOfWork
{
    public IBreedingRabbitRepository BreedingRabbitRepository => breedingRabbitRepository;
    public IPairingRepository PairingRepository => pairingRepository;
    public IFarmTaskRepository FarmTaskRepository => farmTaskRepository;
    public ICageRepository CageRepository => cageRepository;
    public ICustomerRepository CustomerRepository => customerRepository;

    public bool TransactionStarted { get; private set; }
    public bool TransactionCommitted { get; private set; }
    public bool TransactionRolledBack { get; private set; }
    public int SaveAsyncCallCount { get; private set; }

    public Task BeginTransactionAsync()
    {
        TransactionStarted = true;
        return Task.CompletedTask;
    }

    public Task CommitTransactionAsync()
    {
        if (!TransactionStarted)
            throw new InvalidOperationException("Cannot commit transaction that hasn't been started");

        TransactionCommitted = true;
        return Task.CompletedTask;
    }

    public Task RollbackTransactionAsync()
    {
        if (!TransactionStarted)
            throw new InvalidOperationException("Cannot rollback transaction that hasn't been started");

        TransactionRolledBack = true;
        return Task.CompletedTask;
    }

    public Task SaveAsync()
    {
        SaveAsyncCallCount++;
        return Task.CompletedTask;
    }
}
