namespace InscriptionApp.Api.Exceptions;

public class EmailDejaUtiliseException : Exception
{
    public EmailDejaUtiliseException(string email)
        : base($"L'email {email} est déjà utilisé.") { }
}