using InscriptionApp.Api.Enums;

namespace InscriptionApp.Api.Factories.Kits;

public interface INotificationKitFactory
{
    Langue Langue { get; }
    IGabaritMessage CreerGabarit();
    IFormateurDate CreerFormateurDate();
}