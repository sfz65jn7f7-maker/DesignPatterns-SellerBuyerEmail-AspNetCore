using System.Globalization;
using InscriptionApp.Api.Enums;

namespace InscriptionApp.Api.Factories.Kits;

public class KitFrancaisFactory : INotificationKitFactory
{
    public Langue Langue => Langue.Fr;

    public IFormateurDate CreerFormateurDate() => new FormateurDate(new CultureInfo("fr-FR"));

    // Le gabarit français est toujours assemblé avec le formateur français : famille cohérente.
    public IGabaritMessage CreerGabarit() => new GabaritFrancais(CreerFormateurDate());
}