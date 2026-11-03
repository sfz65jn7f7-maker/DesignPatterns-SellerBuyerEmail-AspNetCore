using InscriptionApp.Api.Models;

namespace InscriptionApp.Api.Factories.Kits;

public class GabaritFrancais : IGabaritMessage
{
    private readonly IFormateurDate _dates;

    public GabaritFrancais(IFormateurDate dates) => _dates = dates;

    public (string Sujet, string Corps) PourVendeur(Vendeur v) => (
        "Bienvenue sur notre plateforme de vente",
        $"Bonjour {v.Prenom} {v.Nom}, votre boutique « {v.NomBoutique} » est enregistrée depuis le " +
        $"{_dates.Formater(v.DateInscription)}. Vous pouvez dès maintenant publier vos produits." +
        "\n\nCordialement,\nL'équipe InscriptionApp");

    public (string Sujet, string Corps) PourAcheteur(Acheteur a) => (
        "Votre inscription est confirmée",
        $"Bonjour {a.Prenom} {a.Nom}, votre compte acheteur a été créé le {_dates.Formater(a.DateInscription)}. " +
        $"Adresse de livraison enregistrée : {a.AdresseLivraison}." +
        "\n\nCordialement,\nL'équipe InscriptionApp");
}