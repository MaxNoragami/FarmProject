using FarmProject.Application;
using FarmProject.Application.FarmEventsService;
using FarmProject.Application.PairingService;
using FarmProject.Application.RabbitsService;

namespace FarmProject.Infrastructure;

public class UnitOfWork(FarmDbContext context, 
                        IRabbitRepository rabbitRepository,
                        IPairingRepository pairingRepository,
                        IFarmEventRepository farmEventRepository
                ) : IUnitOfWork
{
    private readonly FarmDbContext _context = context;

    public IRabbitRepository RabbitRepository => rabbitRepository;

    public IPairingRepository PairingRepository => pairingRepository;

    public IFarmEventRepository FarmEventRepository => farmEventRepository;

    public async Task BeginTransactionAsync()
        => await _context.Database.BeginTransactionAsync();

    public async Task CommitTransactionAsync()
        => await _context.Database.CommitTransactionAsync();

    public async Task RollbackTransactionAsync()
        => await _context.Database.RollbackTransactionAsync();

    public async Task SaveAsync()
        => await _context.SaveChangesAsync();
}
