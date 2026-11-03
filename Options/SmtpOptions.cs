namespace InscriptionApp.Api.Options;

public class SmtpOptions
{
    public const string SectionName = "Smtp";

    public bool Enabled { get; set; } = false;   // false = on journalise sans envoyer
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 587;
    public bool EnableSsl { get; set; } = true;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string SenderAddress { get; set; } = "no-reply@inscriptionapp.local";
    public string Provider { get; set; } = "Smtp";   // "Smtp" ou "External"
    public int MaxRetryAttempts { get; set; } = 3;
    public int MaxEmailsPerMinute { get; set; } = 30;
}