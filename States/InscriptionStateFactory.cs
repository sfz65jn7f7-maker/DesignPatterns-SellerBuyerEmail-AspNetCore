using InscriptionApp.Api.Enums;

namespace InscriptionApp.Api.States;

public static class InscriptionStateFactory
{
    public static IInscriptionState From(StatutInscription statut) => statut switch
    {
        StatutInscription.EnAttente => new EnAttenteState(),
        StatutInscription.Confirmee => new ConfirmeeState(),
        StatutInscription.Suspendue => new SuspendueState(),
        _ => throw new NotSupportedException($"Statut inconnu : {statut}")
    };
}