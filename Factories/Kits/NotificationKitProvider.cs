using InscriptionApp.Api.Enums;

namespace InscriptionApp.Api.Factories.Kits;

public class NotificationKitProvider
{
    private readonly IReadOnlyDictionary<Langue, INotificationKitFactory> _kits;

    public NotificationKitProvider(IEnumerable<INotificationKitFactory> kits)
        => _kits = kits.ToDictionary(k => k.Langue);

    public INotificationKitFactory Get(Langue langue)
        => _kits.TryGetValue(langue, out var kit) ? kit : _kits[Langue.Fr];
}