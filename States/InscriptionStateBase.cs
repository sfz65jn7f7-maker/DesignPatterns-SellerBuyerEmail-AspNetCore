using InscriptionApp.Api.Enums;
using InscriptionApp.Api.Exceptions;

namespace InscriptionApp.Api.States;

public abstract class InscriptionStateBase : IInscriptionState
{
    public abstract StatutInscription Statut { get; }

    public virtual IInscriptionState Confirmer()
        => throw new TransitionEtatInvalideException(Statut, nameof(Confirmer));

    public virtual IInscriptionState Suspendre()
        => throw new TransitionEtatInvalideException(Statut, nameof(Suspendre));

    public virtual IInscriptionState Reactiver()
        => throw new TransitionEtatInvalideException(Statut, nameof(Reactiver));
}