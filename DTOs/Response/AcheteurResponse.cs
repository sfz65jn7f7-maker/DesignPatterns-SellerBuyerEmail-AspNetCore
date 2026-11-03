namespace InscriptionApp.Api.DTOs.Response;

public class AcheteurResponse : UtilisateurResponse
{
    public string AdresseLivraison { get; set; } = string.Empty;
}