using InscriptionApp.Api.Commands;
using InscriptionApp.Api.DTOs.Request;
using InscriptionApp.Api.DTOs.Response;
using InscriptionApp.Api.Mediator;
using InscriptionApp.Api.Services.Interfaces;

namespace InscriptionApp.Api.Services.Implementations;

public class InscriptionFacade : IInscriptionFacade
{
    private readonly IMediator _mediator;

    public InscriptionFacade(IMediator mediator) => _mediator = mediator;

    public Task<VendeurResponse> InscrireVendeurAsync(VendeurCreateRequest request, CancellationToken cancellationToken = default)
        => _mediator.SendAsync(new InscrireVendeurCommand(request), cancellationToken);

    public Task<AcheteurResponse> InscrireAcheteurAsync(AcheteurCreateRequest request, CancellationToken cancellationToken = default)
        => _mediator.SendAsync(new InscrireAcheteurCommand(request), cancellationToken);
}