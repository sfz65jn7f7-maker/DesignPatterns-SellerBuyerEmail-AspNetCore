using InscriptionApp.Api.Enums;

namespace InscriptionApp.Api.States;

public class SuspendueState : InscriptionStateBase
{
    public override StatutInscription Statut => StatutInscription.Suspendue;

    public override IInscriptionState Reactiver() => new ConfirmeeState();
}