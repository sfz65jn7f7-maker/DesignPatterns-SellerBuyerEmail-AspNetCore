using InscriptionApp.Api.Models;

namespace InscriptionApp.Api.Services.Interfaces;

public interface IEmailSender
{
    Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default);
}