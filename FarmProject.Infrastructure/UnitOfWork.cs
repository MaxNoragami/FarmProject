﻿using FarmProject.Application;
using FarmProject.Application.BreedingRabbitsService;
using FarmProject.Application.CageService;
using FarmProject.Application.FarmTaskService;
using FarmProject.Application.PairingService;

namespace FarmProject.Infrastructure;

public class UnitOfWork(FarmDbContext context, 
                        IBreedingRabbitRepository breedingRabbitRepository,
                        IPairingRepository pairingRepository,
                        IFarmTaskRepository farmTaskRepository,
                        ICageRepository cageRepository
                ) : IUnitOfWork
{
    private readonly FarmDbContext _context = context;

    public IBreedingRabbitRepository BreedingRabbitRepository => breedingRabbitRepository;

    public IPairingRepository PairingRepository => pairingRepository;

    public IFarmTaskRepository FarmTaskRepository => farmTaskRepository;

    public ICageRepository CageRepository => cageRepository;

    public async Task BeginTransactionAsync()
        => await _context.Database.BeginTransactionAsync();

    public async Task CommitTransactionAsync()
        => await _context.Database.CommitTransactionAsync();

    public async Task RollbackTransactionAsync()
        => await _context.Database.RollbackTransactionAsync();

    public async Task SaveAsync()
        => await _context.SaveChangesAsync();
}
