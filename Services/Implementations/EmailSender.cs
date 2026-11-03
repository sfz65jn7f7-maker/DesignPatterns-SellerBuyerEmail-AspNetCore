using System.Net;
using System.Net.Mail;
using InscriptionApp.Api.Exceptions;
using InscriptionApp.Api.Models;
using InscriptionApp.Api.Options;
using InscriptionApp.Api.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace InscriptionApp.Api.Services.Implementations;

public class EmailSender : IEmailSender
{
    private readonly SmtpOptions _options;
    private readonly ILogger<EmailSender> _logger;
    private readonly Guid _instanceId = Guid.NewGuid();

    public EmailSender(IOptions<SmtpOptions> options, ILogger<EmailSender> logger)
    {
        _options = options.Value;
        _logger = logger;

        // Ce message ne doit apparaître qu'UNE SEULE FOIS : c'est la preuve du Singleton.
        _logger.LogInformation("EmailSender créé (instance {InstanceId})", _instanceId);
    }

    public async Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default)
    {
        if (!_options.Enabled)
        {
            _logger.LogInformation(
                "[Instance {InstanceId}] Email simulé vers {To} | Sujet : {Subject} | Corps : {Body}",
                _instanceId, message.To, message.Subject, message.Body);
            return;
        }

        try
        {
            using var mail = new MailMessage
            {
                From = new MailAddress(message.From ?? _options.SenderAddress),
                Subject = message.Subject,
                Body = message.Body,
                IsBodyHtml = message.IsHtml
            };
            mail.To.Add(message.To);
            foreach (var cc in message.Cc)
                mail.CC.Add(cc);

            using var client = new SmtpClient(_options.Host, _options.Port)
            {
                EnableSsl = _options.EnableSsl,
                Credentials = new NetworkCredential(_options.Username, _options.Password)
            };

            await client.SendMailAsync(mail, cancellationToken);
            _logger.LogInformation("[Instance {InstanceId}] Email envoyé à {To}", _instanceId, message.To);
        }
        catch (Exception ex)
        {
            throw new EmailSendingException($"Échec de l'envoi de l'email à {message.To}.", ex);
        }
    }
}