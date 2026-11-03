using System.ComponentModel.DataAnnotations;

namespace InscriptionApp.Api.DTOs.Request;

public class VendeurCreateRequest : UtilisateurCreateRequest
{
    [Required, StringLength(150)]
    public string NomBoutique { get; set; } = string.Empty;
}