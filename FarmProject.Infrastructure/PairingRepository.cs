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
        var requestPair = _pairs.SingleOrDefault(p => p.Id == pair.Id);

        if (requestPair == null)
            throw new ArgumentException("Rabbit couldn't be found.");

        requestPair = pair;
        return requestPair;
    }
}
