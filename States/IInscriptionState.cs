using InscriptionApp.Api.Enums;

namespace InscriptionApp.Api.States;

public interface IInscriptionState
{
    StatutInscription Statut { get; }
    IInscriptionState Confirmer();
    IInscriptionState Suspendre();
    IInscriptionState Reactiver();
}