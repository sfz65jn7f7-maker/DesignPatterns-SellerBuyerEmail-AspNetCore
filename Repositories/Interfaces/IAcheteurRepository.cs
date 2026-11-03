using InscriptionApp.Api.Models;

namespace InscriptionApp.Api.Repositories.Interfaces;

public interface IAcheteurRepository : IRepository<Acheteur>
{
    Task<bool> EmailExistsAsync(string email);
}
