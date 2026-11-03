using InscriptionApp.Api.Enums;

namespace InscriptionApp.Api.States;

public class EnAttenteState : InscriptionStateBase
{
    public override StatutInscription Statut => StatutInscription.EnAttente;

    public override IInscriptionState Confirmer() => new ConfirmeeState();
    public override IInscriptionState Suspendre() => new SuspendueState();
}