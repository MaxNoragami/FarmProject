using FarmProject.Application.CageService;
using FarmProject.Domain.Models;
using FarmProject.Domain.Specifications;
using Microsoft.EntityFrameworkCore;

namespace FarmProject.Infrastructure.Repositories;

public class CageRepository(FarmDbContext context) : ICageRepository
{
    private readonly FarmDbContext _context = context;

    public async Task<Cage> AddAsync(Cage cage)
    {
        _context.Add(cage);
        await _context.SaveChangesAsync();
        return cage;
    }

    public async Task<List<Cage>> FindAsync(ISpecification<Cage> specification)
        => await _context.Cages
            .Include(C => C.BreedingRabbit)
            .Where(specification.ToExpression())
            .ToListAsync();

    public async Task<List<Cage>> GetAllAsync()
        => await _context.Cages
            .Include(c => c.BreedingRabbit)
            .ToListAsync();

    public async Task<Cage?> GetByIdAsync(int cageId)
        => await _context.Cages
            .Include(c => c.BreedingRabbit)
            .FirstOrDefaultAsync(c => c.Id == cageId);

    public async Task<Cage> UpdateAsync(Cage cage)
    {
        _context.Cages.Update(cage);
        await _context.SaveChangesAsync();
        return cage;
    }
}
