using FarmProject.Application.PairingService;
using FarmProject.Domain.Models;

namespace FarmProject.Infrastructure;

public class PairingRepository : IPairingRepository
{
    private readonly List<PairingProcess> _pairs = new();

    public PairingProcess Create(PairingProcess pair)
    {
        _pairs.Add(pair);
        return pair;
    }

    public List<PairingProcess> GetAll()
        => _pairs;

    public PairingProcess? GetById(int id)
        => _pairs.SingleOrDefault(p => p.Id == id);

    public int GetLastId()
        => _pairs.Any()
            ? _pairs.Max(p => p.Id) + 1
            : 1;

    public PairingProcess Update(PairingProcess pair)
    {
        var index = _pairs.FindIndex(p => p.Id == pair.Id);

        if (index < 0)
            throw new ArgumentException("Pair could not be updated");

        _pairs[index] = pair;

        return pair;
    }
}
