using FarmProject.Domain.Models;

namespace FarmProject.Application.PairingService;

public interface IPairingRepository
{
    public PairingProcess Create(PairingProcess pair);
    public PairingProcess? GetById(int id);
    public List<PairingProcess> GetAll();
    public PairingProcess Update(PairingProcess pair);
    public int GetLastId();
}
