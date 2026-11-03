using InscriptionApp.Api.Models;

namespace InscriptionApp.Api.Repositories.Interfaces;

public interface IVendeurRepository : IRepository<Vendeur>
{
    Task<bool> EmailExistsAsync(string email);
}
