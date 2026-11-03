using InscriptionApp.Api.DTOs.Request;
using InscriptionApp.Api.DTOs.Response;
using InscriptionApp.Api.Models;

namespace InscriptionApp.Api.Mappers;

public class VendeurMapper : IMapper<Vendeur, VendeurCreateRequest, VendeurResponse>
{
    public Vendeur ToEntity(VendeurCreateRequest request) => new()
    {
        Nom = request.Nom.Trim(),
        Prenom = request.Prenom.Trim(),
        Email = request.Email.Trim().ToLowerInvariant(),
        Langue = request.Langue,
        NomBoutique = request.NomBoutique.Trim()
    };

    public VendeurResponse ToResponse(Vendeur entity) => new()
    {
        Id = entity.Id,
        Nom = entity.Nom,
        Prenom = entity.Prenom,
        Email = entity.Email,
        Statut = entity.Statut.ToString(),
        Langue = entity.Langue.ToString(),
        DateInscription = entity.DateInscription,
        NomBoutique = entity.NomBoutique
    };
}