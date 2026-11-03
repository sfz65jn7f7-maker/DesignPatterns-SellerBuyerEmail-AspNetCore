using InscriptionApp.Api.Models;

namespace InscriptionApp.Api.Factories.Kits;

public interface IGabaritMessage
{
    (string Sujet, string Corps) PourVendeur(Vendeur vendeur);
    (string Sujet, string Corps) PourAcheteur(Acheteur acheteur);
}