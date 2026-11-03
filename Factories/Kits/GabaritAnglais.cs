using InscriptionApp.Api.Models;

namespace InscriptionApp.Api.Factories.Kits;

public class GabaritAnglais : IGabaritMessage
{
    private readonly IFormateurDate _dates;

    public GabaritAnglais(IFormateurDate dates) => _dates = dates;

    public (string Sujet, string Corps) PourVendeur(Vendeur v) => (
        "Welcome to our marketplace",
        $"Hello {v.Prenom} {v.Nom}, your shop “{v.NomBoutique}” has been registered on " +
        $"{_dates.Formater(v.DateInscription)}. You can now publish your products." +
        "\n\nBest regards,\nThe InscriptionApp team");

    public (string Sujet, string Corps) PourAcheteur(Acheteur a) => (
        "Your registration is confirmed",
        $"Hello {a.Prenom} {a.Nom}, your buyer account was created on {_dates.Formater(a.DateInscription)}. " +
        $"Delivery address on file: {a.AdresseLivraison}." +
        "\n\nBest regards,\nThe InscriptionApp team");
}