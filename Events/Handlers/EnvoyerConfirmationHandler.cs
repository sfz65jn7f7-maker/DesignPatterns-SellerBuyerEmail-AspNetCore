using InscriptionApp.Api.Enums;
using InscriptionApp.Api.Factories;
using InscriptionApp.Api.Factories.Kits;
using InscriptionApp.Api.Models;
using InscriptionApp.Api.Strategies;

namespace InscriptionApp.Api.Events.Handlers;

public class EnvoyerConfirmationHandler<TUser> : IEventHandler<UtilisateurInscritEvent<TUser>>
    where TUser : Utilisateur
{
    private readonly ConfirmationMessageFactory<TUser> _messageFactory;
    private readonly NotificationKitProvider _kits;
    private readonly NotificationContext _notifications;

    public EnvoyerConfirmationHandler(
        ConfirmationMessageFactory<TUser> messageFactory,
        NotificationKitProvider kits,
        NotificationContext notifications)
    {
        _messageFactory = messageFactory;
        _kits = kits;
        _notifications = notifications;
    }

    public async Task HandleAsync(UtilisateurInscritEvent<TUser> @event, CancellationToken cancellationToken = default)
    {
        var kit = _kits.Get(@event.User.Langue);                                              // Abstract Factory
        var message = _messageFactory.Create(@event.User, kit);                               // Factory Method + Builder
        await _notifications.SendAsync(CanalNotification.Email, message, cancellationToken);  // Strategy
    }
}