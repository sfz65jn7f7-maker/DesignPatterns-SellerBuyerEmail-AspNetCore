using InscriptionApp.Api.Models;

namespace InscriptionApp.Api.Events;

public record UtilisateurInscritEvent<TUser>(TUser User) where TUser : Utilisateur;