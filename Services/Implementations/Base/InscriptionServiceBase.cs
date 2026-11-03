using InscriptionApp.Api.DTOs.Request;
using InscriptionApp.Api.DTOs.Response;
using InscriptionApp.Api.Events;
using InscriptionApp.Api.Exceptions;
using InscriptionApp.Api.Mappers;
using InscriptionApp.Api.Models;
using InscriptionApp.Api.Repositories.Interfaces;
using InscriptionApp.Api.Enums;
using InscriptionApp.Api.States;

namespace InscriptionApp.Api.Services.Implementations.Base;

public abstract class InscriptionServiceBase<TEntity, TRequest, TResponse>
    where TEntity : Utilisateur
    where TRequest : UtilisateurCreateRequest
    where TResponse : UtilisateurResponse
{
    private readonly IRepository<TEntity> _repository;
    private readonly IMapper<TEntity, TRequest, TResponse> _mapper;
    private readonly IEventPublisher _publisher;

    protected InscriptionServiceBase(
        IRepository<TEntity> repository,
        IMapper<TEntity, TRequest, TResponse> mapper,
        IEventPublisher publisher)
    {
        _repository = repository;
        _mapper = mapper;
        _publisher = publisher;
    }

    // ===== TEMPLATE METHOD : le squelette reste FIXE =====
    public async Task<TResponse> InscrireAsync(TRequest request)
    {
        await ValiderAsync(request);                    // 1. valider
        var entity = _mapper.ToEntity(request);         // 2. convertir
        await _repository.AddAsync(entity);             // 3. enregistrer
        await NotifierAsync(entity);                    // 4. annoncer l'inscription
        return _mapper.ToResponse(entity);              // 5. répondre
    }

    protected abstract Task<bool> EmailExisteAsync(string email);

    protected virtual async Task ValiderAsync(TRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        if (await EmailExisteAsync(email))
            throw new EmailDejaUtiliseException(email);
    }

    // Le service ne connaît plus l'email : il publie seulement l'événement (Observer)
    protected virtual Task NotifierAsync(TEntity entity)
        => _publisher.PublishAsync(new UtilisateurInscritEvent<TEntity>(entity));

    public async Task<TResponse> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id)
                     ?? throw new NotFoundException(typeof(TEntity).Name, id);
        return _mapper.ToResponse(entity);
    }

    public async Task<IReadOnlyList<TResponse>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        return entities.Select(_mapper.ToResponse).ToList();
    }

    public async Task<TResponse> ChangerStatutAsync(int id, ActionStatut action)
    {
        var entity = await _repository.GetByIdAsync(id)
                     ?? throw new NotFoundException(typeof(TEntity).Name, id);

        new InscriptionContext(entity).Appliquer(action);   // State : valide et applique la transition

        await _repository.UpdateAsync(entity);
        return _mapper.ToResponse(entity);
    }
}