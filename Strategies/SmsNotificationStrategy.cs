using InscriptionApp.Api.Enums;
using InscriptionApp.Api.Models;

namespace InscriptionApp.Api.Strategies;

public class SmsNotificationStrategy : INotificationStrategy
{
    private readonly ILogger<SmsNotificationStrategy> _logger;

    public SmsNotificationStrategy(ILogger<SmsNotificationStrategy> logger) => _logger = logger;

    public CanalNotification Canal => CanalNotification.Sms;

    public Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default)
    {
        // Simulation : un vrai SMS demanderait un numéro de téléphone, que nos utilisateurs n'ont pas.
        _logger.LogInformation("SMS simulé pour {To} : {Body}", message.To, message.Body);
        return Task.CompletedTask;
    }
}