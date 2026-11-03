using InscriptionApp.Api.Builders;
using InscriptionApp.Api.Factories.Kits;
using InscriptionApp.Api.Models;

namespace InscriptionApp.Api.Factories;

public class VendeurMessageFactory : ConfirmationMessageFactory<Vendeur>
{
    public override EmailMessage Create(Vendeur vendeur, INotificationKitFactory kit)
    {
        var (sujet, corps) = kit.CreerGabarit().PourVendeur(vendeur);

        return new EmailMessageBuilder()
            .To(vendeur.Email)
            .WithSubject(sujet)
            .WithBody(corps)
            .Build();
    }
}