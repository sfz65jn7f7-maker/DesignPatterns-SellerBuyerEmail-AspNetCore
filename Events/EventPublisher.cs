using Microsoft.Extensions.DependencyInjection;

namespace InscriptionApp.Api.Events;

public class EventPublisher : IEventPublisher
{
    private readonly IServiceProvider _provider;
    private readonly ILogger<EventPublisher> _logger;

    public EventPublisher(IServiceProvider provider, ILogger<EventPublisher> logger)
    {
        _provider = provider;
        _logger = logger;
    }

    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
    {
        var handlers = _provider.GetServices<IEventHandler<TEvent>>();

        foreach (var handler in handlers)
        {
            try
            {
                await handler.HandleAsync(@event, cancellationToken);
            }
            catch (Exception ex)
            {
                // Un abonné qui échoue ne doit ni bloquer les autres, ni annuler l'inscription.
                _logger.LogWarning(ex, "L'abonné {Handler} a échoué pour l'événement {Event}",
                    handler.GetType().Name, typeof(TEvent).Name);
            }
        }
    }
}