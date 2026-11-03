using InscriptionApp.Api.DTOs.Request;
using InscriptionApp.Api.DTOs.Response;
using InscriptionApp.Api.Events;
using InscriptionApp.Api.Mappers;
using InscriptionApp.Api.Models;
using InscriptionApp.Api.Repositories.Interfaces;
using InscriptionApp.Api.Services.Implementations.Base;
using InscriptionApp.Api.Services.Interfaces;

namespace InscriptionApp.Api.Services.Implementations;

public class AcheteurService
    : InscriptionServiceBase<Acheteur, AcheteurCreateRequest, AcheteurResponse>, IAcheteurService
{
    private readonly IAcheteurRepository _acheteurRepository;

    public AcheteurService(
        IAcheteurRepository repository,
        IMapper<Acheteur, AcheteurCreateRequest, AcheteurResponse> mapper,
        IEventPublisher publisher)
        : base(repository, mapper, publisher)
    {
        _acheteurRepository = repository;
    }

    protected override Task<bool> EmailExisteAsync(string email)
        => _acheteurRepository.EmailExistsAsync(email);
}