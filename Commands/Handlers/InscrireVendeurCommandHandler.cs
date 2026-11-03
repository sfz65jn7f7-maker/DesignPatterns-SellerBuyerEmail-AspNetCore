using InscriptionApp.Api.DTOs.Response;
using InscriptionApp.Api.Services.Interfaces;

namespace InscriptionApp.Api.Commands.Handlers;

public class InscrireVendeurCommandHandler : ICommandHandler<InscrireVendeurCommand, VendeurResponse>
{
    private readonly IVendeurService _service;

    public InscrireVendeurCommandHandler(IVendeurService service) => _service = service;

    public Task<VendeurResponse> HandleAsync(InscrireVendeurCommand command, CancellationToken cancellationToken = default)
        => _service.InscrireAsync(command.Request);
}