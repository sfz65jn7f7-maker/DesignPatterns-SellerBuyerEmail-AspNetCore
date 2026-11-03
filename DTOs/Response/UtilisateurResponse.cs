namespace InscriptionApp.Api.DTOs.Response;

public abstract class UtilisateurResponse
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Statut { get; set; } = string.Empty;
    public DateTime DateInscription { get; set; }
    public string Langue { get; set; } = string.Empty;
}