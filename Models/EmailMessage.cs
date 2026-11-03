namespace InscriptionApp.Api.Models;

public class EmailMessage
{
    public string To { get; init; } = string.Empty;
    public string? From { get; init; }
    public string Subject { get; init; } = string.Empty;
    public string Body { get; init; } = string.Empty;
    public bool IsHtml { get; init; }
    public IReadOnlyList<string> Cc { get; init; } = Array.Empty<string>();
}