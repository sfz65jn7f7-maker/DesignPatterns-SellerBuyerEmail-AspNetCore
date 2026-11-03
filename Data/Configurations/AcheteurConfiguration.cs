using InscriptionApp.Api.Enums;
using InscriptionApp.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InscriptionApp.Api.Data.Configurations;

public class AcheteurConfiguration : IEntityTypeConfiguration<Acheteur>
{
    public void Configure(EntityTypeBuilder<Acheteur> builder)
    {
        builder.ToTable("Acheteurs");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Nom).IsRequired().HasMaxLength(100);
        builder.Property(a => a.Prenom).IsRequired().HasMaxLength(100);
        builder.Property(a => a.Email).IsRequired().HasMaxLength(200);
        builder.Property(a => a.AdresseLivraison).IsRequired().HasMaxLength(300);
        builder.Property(a => a.Statut).HasConversion<string>().HasMaxLength(20);
        builder.Property(a => a.Langue).HasConversion<string>().HasMaxLength(5).HasDefaultValue(Langue.Fr);
        builder.HasIndex(a => a.Email).IsUnique();
    }
}
