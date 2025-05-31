using FarmProject.Application;
using FarmProject.Application.FarmTaskService;
using FarmProject.Application.PairingService;
using FarmProject.Application.RabbitsService;

namespace FarmProject.Infrastructure;

public class UnitOfWork(FarmDbContext context, 
                        IRabbitRepository rabbitRepository,
                        IPairingRepository pairingRepository,
                        IFarmTaskRepository farmTaskRepository
                ) : IUnitOfWork
{
    private readonly FarmDbContext _context = context;

    public IRabbitRepository RabbitRepository => rabbitRepository;

    public IPairingRepository PairingRepository => pairingRepository;

    public IFarmTaskRepository FarmTaskRepository => farmTaskRepository;

    public async Task BeginTransactionAsync()
        => await _context.Database.BeginTransactionAsync();

    public async Task CommitTransactionAsync()
        => await _context.Database.CommitTransactionAsync();

    public async Task RollbackTransactionAsync()
        => await _context.Database.RollbackTransactionAsync();

    public async Task SaveAsync()
        => await _context.SaveChangesAsync();
}
