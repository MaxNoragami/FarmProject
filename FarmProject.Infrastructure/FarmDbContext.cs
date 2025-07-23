using FarmProject.Domain.Events;
using FarmProject.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FarmProject.Infrastructure;

public class FarmDbContext(DbContextOptions<FarmDbContext> options) : DbContext(options)
{
    public DbSet<BreedingRabbit> BreedingRabbits { get; set; } = default!;
    public DbSet<Pair> Pairs { get; set; } = default!;
    public DbSet<FarmTask> FarmTasks { get; set; } = default!;
    public DbSet<Cage> Cages { get; set; } = default!;
    public DbSet<Customer> Customers { get; set; } = default!;
    public DbSet<Sacrification> Sacrifications { get; set; } = default!;
    public DbSet<Order> Orders { get; set; } = default!;
    public DbSet<OrderRequest> OrderRequests { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FarmDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
        modelBuilder.Ignore<DomainEvent>();
    }
}
