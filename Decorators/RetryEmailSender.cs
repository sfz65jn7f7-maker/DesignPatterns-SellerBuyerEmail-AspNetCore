using InscriptionApp.Api.Exceptions;
using InscriptionApp.Api.Models;
using InscriptionApp.Api.Services.Interfaces;

namespace InscriptionApp.Api.Decorators;

public class RetryEmailSender : IEmailSender
{
    private readonly IEmailSender _inner;
    private readonly ILogger<RetryEmailSender> _logger;
    private readonly int _maxAttempts;
    private readonly TimeSpan _baseDelay;

    public RetryEmailSender(IEmailSender inner, ILogger<RetryEmailSender> logger, int maxAttempts, TimeSpan baseDelay)
    {
        _inner = inner;
        _logger = logger;
        _maxAttempts = Math.Max(1, maxAttempts);
        _baseDelay = baseDelay;
    }

    public async Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default)
    {
        for (var attempt = 1; ; attempt++)
        {
            try
            {
                await _inner.SendAsync(message, cancellationToken);
                return;
            }
            catch (EmailSendingException) when (attempt < _maxAttempts)
            {
                var delay = _baseDelay * attempt;   // 0,5 s puis 1 s, etc.
                _logger.LogWarning("Tentative {Attempt}/{Max} échouée pour {To}, nouvel essai dans {Delay} ms",
                    attempt, _maxAttempts, message.To, delay.TotalMilliseconds);
                await Task.Delay(delay, cancellationToken);
            }
        }
    }
}