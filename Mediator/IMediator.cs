using InscriptionApp.Api.Commands;

namespace InscriptionApp.Api.Mediator;

public interface IMediator
{
    Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default);
}