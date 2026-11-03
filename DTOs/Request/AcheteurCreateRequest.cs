using System.ComponentModel.DataAnnotations;

namespace InscriptionApp.Api.DTOs.Request;

public class AcheteurCreateRequest : UtilisateurCreateRequest
{
    [Required, StringLength(300)]
    public string AdresseLivraison { get; set; } = string.Empty;
}