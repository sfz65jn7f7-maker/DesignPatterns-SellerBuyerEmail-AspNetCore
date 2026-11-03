using InscriptionApp.Api.DTOs.Request;
using InscriptionApp.Api.DTOs.Response;
using InscriptionApp.Api.Models;

namespace InscriptionApp.Api.Mappers;

public class AcheteurMapper : IMapper<Acheteur, AcheteurCreateRequest, AcheteurResponse>
{
    public Acheteur ToEntity(AcheteurCreateRequest request) => new()
    {
        Nom = request.Nom.Trim(),
        Prenom = request.Prenom.Trim(),
        Email = request.Email.Trim().ToLowerInvariant(),
        Langue = request.Langue,
        AdresseLivraison = request.AdresseLivraison.Trim()
    };

    public AcheteurResponse ToResponse(Acheteur entity) => new()
    {
        Id = entity.Id,
        Nom = entity.Nom,
        Prenom = entity.Prenom,
        Email = entity.Email,
        Statut = entity.Statut.ToString(),
        Langue = entity.Langue.ToString(),
        DateInscription = entity.DateInscription,
        AdresseLivraison = entity.AdresseLivraison
    };
}