using InscriptionApp.Api.Enums;
using InscriptionApp.Api.Models;

namespace InscriptionApp.Api.Strategies;

public class NotificationContext
{
    private readonly IReadOnlyDictionary<CanalNotification, INotificationStrategy> _strategies;

    public NotificationContext(IEnumerable<INotificationStrategy> strategies)
        => _strategies = strategies.ToDictionary(s => s.Canal);

    public Task SendAsync(CanalNotification canal, EmailMessage message, CancellationToken cancellationToken = default)
    {
        if (!_strategies.TryGetValue(canal, out var strategy))
            throw new NotSupportedException($"Aucune stratégie enregistrée pour le canal {canal}.");

        return strategy.SendAsync(message, cancellationToken);
    }
}