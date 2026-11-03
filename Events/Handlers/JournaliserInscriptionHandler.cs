using InscriptionApp.Api.Models;

namespace InscriptionApp.Api.Events.Handlers;

public class JournaliserInscriptionHandler<TUser> : IEventHandler<UtilisateurInscritEvent<TUser>>
    where TUser : Utilisateur
{
    private readonly ILogger<JournaliserInscriptionHandler<TUser>> _logger;

    public JournaliserInscriptionHandler(ILogger<JournaliserInscriptionHandler<TUser>> logger)
        => _logger = logger;

    public Task HandleAsync(UtilisateurInscritEvent<TUser> @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Nouvelle inscription : {Type} #{Id} ({Email})",
            typeof(TUser).Name, @event.User.Id, @event.User.Email);
        return Task.CompletedTask;
    }
}