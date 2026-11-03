using InscriptionApp.Api.Enums;

namespace InscriptionApp.Api.Exceptions;

public class TransitionEtatInvalideException : Exception
{
    public TransitionEtatInvalideException(StatutInscription etatActuel, string action)
        : base($"Impossible d'exécuter l'action « {action} » depuis l'état {etatActuel}.") { }
}