using InscriptionApp.Api.Enums;
using InscriptionApp.Api.Models;

namespace InscriptionApp.Api.Strategies;

public interface INotificationStrategy
{
    CanalNotification Canal { get; }
    Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default);
}