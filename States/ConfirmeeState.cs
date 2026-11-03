using InscriptionApp.Api.Enums;

namespace InscriptionApp.Api.States;

public class ConfirmeeState : InscriptionStateBase
{
    public override StatutInscription Statut => StatutInscription.Confirmee;

    public override IInscriptionState Suspendre() => new SuspendueState();
}