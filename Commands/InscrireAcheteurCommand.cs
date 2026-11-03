using InscriptionApp.Api.DTOs.Request;
using InscriptionApp.Api.DTOs.Response;

namespace InscriptionApp.Api.Commands;

public record InscrireAcheteurCommand(AcheteurCreateRequest Request) : ICommand<AcheteurResponse>;