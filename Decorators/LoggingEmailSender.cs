using System.Diagnostics;
using InscriptionApp.Api.Models;
using InscriptionApp.Api.Services.Interfaces;

namespace InscriptionApp.Api.Decorators;

public class LoggingEmailSender : IEmailSender
{
    private readonly IEmailSender _inner;
    private readonly ILogger<LoggingEmailSender> _logger;

    public LoggingEmailSender(IEmailSender inner, ILogger<LoggingEmailSender> logger)
    {
        _inner = inner;
        _logger = logger;
    }

    public async Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default)
    {
        var watch = Stopwatch.StartNew();
        _logger.LogInformation("Envoi d'un email à {To}...", message.To);
        try
        {
            await _inner.SendAsync(message, cancellationToken);
            _logger.LogInformation("Email traité pour {To} en {Ms} ms", message.To, watch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Échec de l'envoi à {To} après {Ms} ms", message.To, watch.ElapsedMilliseconds);
            throw;
        }
    }
}