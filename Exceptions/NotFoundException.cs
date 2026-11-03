namespace InscriptionApp.Api.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string entityName, object key)
        : base($"{entityName} avec l'identifiant {key} introuvable.") { }
}