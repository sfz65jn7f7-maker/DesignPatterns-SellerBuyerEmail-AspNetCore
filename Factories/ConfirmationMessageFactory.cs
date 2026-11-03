using InscriptionApp.Api.Factories.Kits;
using InscriptionApp.Api.Models;

namespace InscriptionApp.Api.Factories;

public abstract class ConfirmationMessageFactory<TUser> where TUser : Utilisateur
{
    // Factory Method : la sous-classe décide quel message fabriquer,
    // avec les pièces du kit (Abstract Factory) qu'on lui fournit.
    public abstract EmailMessage Create(TUser user, INotificationKitFactory kit);
}