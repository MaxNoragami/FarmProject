using FarmProject.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace FarmProject.API.IntegrationTests.Helpers;

public class InMemoryDbContextFactory : IDisposable
{
    private readonly FarmDbContext _context;

    public InMemoryDbContextFactory(string dbName = "TestDatabase")
    {
        var options = new DbContextOptionsBuilder<FarmDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .ConfigureWarnings(warnings =>
                warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        _context = new FarmDbContext(options);
        _context.Database.EnsureCreated();
    }

    public FarmDbContext GetContext() => _context;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _context.Dispose();
        }
    }
}
