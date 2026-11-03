using InscriptionApp.Api.DTOs.Request;
using InscriptionApp.Api.DTOs.Response;
using InscriptionApp.Api.Enums;

namespace InscriptionApp.Api.Services.Interfaces;

public interface IVendeurService
{
    Task<VendeurResponse> InscrireAsync(VendeurCreateRequest request);
    Task<VendeurResponse> GetByIdAsync(int id);
    Task<IReadOnlyList<VendeurResponse>> GetAllAsync();
    Task<VendeurResponse> ChangerStatutAsync(int id, ActionStatut action);
}