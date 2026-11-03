using InscriptionApp.Api.DTOs.Request;
using InscriptionApp.Api.DTOs.Response;

namespace InscriptionApp.Api.Services.Interfaces;

public interface IInscriptionFacade
{
    Task<VendeurResponse> InscrireVendeurAsync(VendeurCreateRequest request, CancellationToken cancellationToken = default);
    Task<AcheteurResponse> InscrireAcheteurAsync(AcheteurCreateRequest request, CancellationToken cancellationToken = default);
}