using InscriptionApp.Api.Models;

namespace InscriptionApp.Api.Builders;

public class EmailMessageBuilder
{
    private string? _to;
    private string? _from;
    private string? _subject;
    private string? _body;
    private bool _isHtml;
    private readonly List<string> _cc = new();

    public EmailMessageBuilder To(string to) { _to = to; return this; }
    public EmailMessageBuilder From(string from) { _from = from; return this; }
    public EmailMessageBuilder WithSubject(string subject) { _subject = subject; return this; }
    public EmailMessageBuilder WithBody(string body, bool isHtml = false)
    {
        _body = body;
        _isHtml = isHtml;
        return this;
    }
    public EmailMessageBuilder AddCc(string cc) { _cc.Add(cc); return this; }

    public EmailMessage Build()
    {
        if (string.IsNullOrWhiteSpace(_to))
            throw new InvalidOperationException("Le destinataire est obligatoire.");
        if (string.IsNullOrWhiteSpace(_subject))
            throw new InvalidOperationException("Le sujet est obligatoire.");
        if (string.IsNullOrWhiteSpace(_body))
            throw new InvalidOperationException("Le corps du message est obligatoire.");

        return new EmailMessage
        {
            To = _to,
            From = _from,
            Subject = _subject,
            Body = _body,
            IsHtml = _isHtml,
            Cc = _cc.ToList()
        };
    }
}