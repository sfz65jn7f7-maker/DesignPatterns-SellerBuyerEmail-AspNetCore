using InscriptionApp.Api.DTOs.Request;
using InscriptionApp.Api.DTOs.Response;
using InscriptionApp.Api.Enums;

namespace InscriptionApp.Api.Services.Interfaces;

public interface IAcheteurService
{
    Task<AcheteurResponse> InscrireAsync(AcheteurCreateRequest request);
    Task<AcheteurResponse> GetByIdAsync(int id);
    Task<IReadOnlyList<AcheteurResponse>> GetAllAsync();
    Task<AcheteurResponse> ChangerStatutAsync(int id, ActionStatut action);
}