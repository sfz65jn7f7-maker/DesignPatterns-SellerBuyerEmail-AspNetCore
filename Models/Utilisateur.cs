using InscriptionApp.Api.Enums;

namespace InscriptionApp.Api.Models;

public abstract class Utilisateur
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public StatutInscription Statut { get; set; } = StatutInscription.EnAttente;
    public Langue Langue { get; set; } = Langue.Fr;
    public DateTime DateInscription { get; set; } = DateTime.UtcNow;
}