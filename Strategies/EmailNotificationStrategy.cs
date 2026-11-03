using InscriptionApp.Api.Enums;
using InscriptionApp.Api.Models;
using InscriptionApp.Api.Services.Interfaces;

namespace InscriptionApp.Api.Strategies;

public class EmailNotificationStrategy : INotificationStrategy
{
    private readonly IEmailSender _emailSender;

    public EmailNotificationStrategy(IEmailSender emailSender) => _emailSender = emailSender;

    public CanalNotification Canal => CanalNotification.Email;

    public Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default)
        => _emailSender.SendAsync(message, cancellationToken);
}