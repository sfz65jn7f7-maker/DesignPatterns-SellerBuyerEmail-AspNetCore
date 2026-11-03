using InscriptionApp.Api.DTOs.Request;
using InscriptionApp.Api.DTOs.Response;
using InscriptionApp.Api.Events;
using InscriptionApp.Api.Mappers;
using InscriptionApp.Api.Models;
using InscriptionApp.Api.Repositories.Interfaces;
using InscriptionApp.Api.Services.Implementations.Base;
using InscriptionApp.Api.Services.Interfaces;

namespace InscriptionApp.Api.Services.Implementations;

public class VendeurService
    : InscriptionServiceBase<Vendeur, VendeurCreateRequest, VendeurResponse>, IVendeurService
{
    private readonly IVendeurRepository _vendeurRepository;

    public VendeurService(
        IVendeurRepository repository,
        IMapper<Vendeur, VendeurCreateRequest, VendeurResponse> mapper,
        IEventPublisher publisher)
        : base(repository, mapper, publisher)
    {
        _vendeurRepository = repository;
    }

    protected override Task<bool> EmailExisteAsync(string email)
        => _vendeurRepository.EmailExistsAsync(email);
}