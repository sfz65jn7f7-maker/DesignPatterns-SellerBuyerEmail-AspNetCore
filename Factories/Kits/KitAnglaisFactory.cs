using System.Globalization;
using InscriptionApp.Api.Enums;

namespace InscriptionApp.Api.Factories.Kits;

public class KitAnglaisFactory : INotificationKitFactory
{
    public Langue Langue => Langue.En;

    public IFormateurDate CreerFormateurDate() => new FormateurDate(new CultureInfo("en-US"));

    public IGabaritMessage CreerGabarit() => new GabaritAnglais(CreerFormateurDate());
}