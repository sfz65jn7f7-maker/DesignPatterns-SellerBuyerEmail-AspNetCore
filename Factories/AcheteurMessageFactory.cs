using InscriptionApp.Api.Builders;
using InscriptionApp.Api.Factories.Kits;
using InscriptionApp.Api.Models;

namespace InscriptionApp.Api.Factories;

public class AcheteurMessageFactory : ConfirmationMessageFactory<Acheteur>
{
    public override EmailMessage Create(Acheteur acheteur, INotificationKitFactory kit)
    {
        var (sujet, corps) = kit.CreerGabarit().PourAcheteur(acheteur);

        return new EmailMessageBuilder()
            .To(acheteur.Email)
            .WithSubject(sujet)
            .WithBody(corps)
            .Build();
    }
}