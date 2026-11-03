using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using InscriptionApp.Api.Enums;

namespace InscriptionApp.Api.DTOs.Request;

public abstract class UtilisateurCreateRequest
{
    [Required, StringLength(100)]
    public string Nom { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Prenom { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(200)]
    public string Email { get; set; } = string.Empty;

    // Optionnel : "Fr" par défaut. Accepte "Fr" ou "En" en texte.
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [EnumDataType(typeof(Langue))]
    public Langue Langue { get; set; } = Langue.Fr;
}