using InscriptionApp.Api.Enums;
using InscriptionApp.Api.Models;

namespace InscriptionApp.Api.States;

public class InscriptionContext
{
    private readonly Utilisateur _utilisateur;
    private IInscriptionState _state;

    public InscriptionContext(Utilisateur utilisateur)
    {
        _utilisateur = utilisateur;
        _state = InscriptionStateFactory.From(utilisateur.Statut);
    }

    public void Appliquer(ActionStatut action)
    {
        _state = action switch
        {
            ActionStatut.Confirmer => _state.Confirmer(),
            ActionStatut.Suspendre => _state.Suspendre(),
            ActionStatut.Reactiver => _state.Reactiver(),
            _ => throw new NotSupportedException($"Action inconnue : {action}")
        };

        _utilisateur.Statut = _state.Statut;
    }
}