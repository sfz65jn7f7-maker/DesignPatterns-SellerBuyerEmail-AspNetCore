using InscriptionApp.Api.DTOs.Response;
using InscriptionApp.Api.Services.Interfaces;

namespace InscriptionApp.Api.Commands.Handlers;

public class InscrireAcheteurCommandHandler : ICommandHandler<InscrireAcheteurCommand, AcheteurResponse>
{
    private readonly IAcheteurService _service;

    public InscrireAcheteurCommandHandler(IAcheteurService service) => _service = service;

    public Task<AcheteurResponse> HandleAsync(InscrireAcheteurCommand command, CancellationToken cancellationToken = default)
        => _service.InscrireAsync(command.Request);
}