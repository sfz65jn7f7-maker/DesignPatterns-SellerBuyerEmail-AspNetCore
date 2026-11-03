using InscriptionApp.Api.Exceptions;
using InscriptionApp.Api.Models;
using InscriptionApp.Api.Services.Interfaces;

namespace InscriptionApp.Api.Adapters;

public class ExternalMailAdapter : IEmailSender
{
    private readonly ExternalMailClient _client;

    public ExternalMailAdapter(ExternalMailClient client) => _client = client;

    public async Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default)
    {
        var ok = await _client.PostMessageAsync(
            message.To,
            message.Subject,
            message.Body,
            message.Cc.ToArray());

        if (!ok)
            throw new EmailSendingException($"Le fournisseur externe a refusé l'email pour {message.To}.");
    }
}