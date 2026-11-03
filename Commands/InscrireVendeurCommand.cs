using InscriptionApp.Api.DTOs.Request;
using InscriptionApp.Api.DTOs.Response;

namespace InscriptionApp.Api.Commands;

public record InscrireVendeurCommand(VendeurCreateRequest Request) : ICommand<VendeurResponse>;