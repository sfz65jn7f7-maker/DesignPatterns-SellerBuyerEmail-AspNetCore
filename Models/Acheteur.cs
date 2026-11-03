namespace InscriptionApp.Api.Models;

public class Acheteur : Utilisateur
{
    public string AdresseLivraison { get; set; } = string.Empty;
}
