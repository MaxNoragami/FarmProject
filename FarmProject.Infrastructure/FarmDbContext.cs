using FarmProject.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FarmProject.Infrastructure;

public class FarmDbContext(DbContextOptions<FarmDbContext> options) : DbContext(options)
{
    public DbSet<Rabbit> Rabbits { get; set; } = default!;
    public DbSet<Pair> Pairs { get; set; } = default!;
    public DbSet<FarmEvent> FarmEvents { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FarmDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
