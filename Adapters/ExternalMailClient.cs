namespace InscriptionApp.Api.Adapters;

public class ExternalMailClient
{
    private readonly ILogger<ExternalMailClient> _logger;

    public ExternalMailClient(ILogger<ExternalMailClient> logger) => _logger = logger;

    // Signature différente de IEmailSender.SendAsync
    public Task<bool> PostMessageAsync(string recipient, string title, string htmlContent, string[] copyList)
    {
        _logger.LogInformation("[API externe] Message « {Title} » posté pour {Recipient}", title, recipient);
        return Task.FromResult(true);
    }
}