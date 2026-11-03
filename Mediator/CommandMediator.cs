using InscriptionApp.Api.Commands;

namespace InscriptionApp.Api.Mediator;

public class CommandMediator : IMediator
{
    private readonly IServiceProvider _provider;
    private readonly ILogger<CommandMediator> _logger;

    public CommandMediator(IServiceProvider provider, ILogger<CommandMediator> logger)
    {
        _provider = provider;
        _logger = logger;
    }

    public async Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default)
    {
        var commandType = command.GetType();
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(commandType, typeof(TResponse));

        var handler = _provider.GetService(handlerType)
            ?? throw new InvalidOperationException($"Aucun handler enregistré pour la commande {commandType.Name}.");

        _logger.LogInformation("Commande {Command} transmise à {Handler}", commandType.Name, handler.GetType().Name);

        // dynamic : le bon HandleAsync est choisi à l'exécution, et les exceptions
        // du service (409, 404...) remontent telles quelles jusqu'au filtre global.
        return await ((dynamic)handler).HandleAsync((dynamic)command, cancellationToken);
    }
}