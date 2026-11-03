namespace InscriptionApp.Api.Exceptions;

public class EmailSendingException : Exception
{
    public EmailSendingException(string message, Exception? innerException = null)
        : base(message, innerException) { }
}