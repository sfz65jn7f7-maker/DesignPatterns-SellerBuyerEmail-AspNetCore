using InscriptionApp.Api.Exceptions;
using InscriptionApp.Api.Models;
using InscriptionApp.Api.Services.Interfaces;

namespace InscriptionApp.Api.Proxies;

public class RateLimitedEmailSenderProxy : IEmailSender
{
    private readonly IEmailSender _inner;
    private readonly ILogger<RateLimitedEmailSenderProxy> _logger;
    private readonly int _maxPerMinute;
    private readonly Queue<DateTime> _sentAt = new();
    private readonly object _lock = new();

    public RateLimitedEmailSenderProxy(IEmailSender inner, ILogger<RateLimitedEmailSenderProxy> logger, int maxPerMinute)
    {
        _inner = inner;
        _logger = logger;
        _maxPerMinute = Math.Max(1, maxPerMinute);
    }

    public Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            var now = DateTime.UtcNow;
            while (_sentAt.Count > 0 && now - _sentAt.Peek() > TimeSpan.FromMinutes(1))
                _sentAt.Dequeue();

            if (_sentAt.Count >= _maxPerMinute)
            {
                _logger.LogWarning("Limite de {Max} emails/minute atteinte, envoi refusé pour {To}", _maxPerMinute, message.To);
                throw new EmailSendingException("Limite d'envoi atteinte, réessayez plus tard.");
            }

            _sentAt.Enqueue(now);
        }

        return _inner.SendAsync(message, cancellationToken);
    }
}