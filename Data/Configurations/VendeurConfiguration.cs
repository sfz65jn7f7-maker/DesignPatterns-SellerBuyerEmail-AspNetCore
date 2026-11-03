using InscriptionApp.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InscriptionApp.Api.Enums;

namespace InscriptionApp.Api.Data.Configurations;

public class VendeurConfiguration : IEntityTypeConfiguration<Vendeur>
{
    public void Configure(EntityTypeBuilder<Vendeur> builder)
    {
        builder.ToTable("Vendeurs");
        builder.HasKey(v => v.Id);

        builder.Property(v => v.Nom).IsRequired().HasMaxLength(100);
        builder.Property(v => v.Prenom).IsRequired().HasMaxLength(100);
        builder.Property(v => v.Email).IsRequired().HasMaxLength(200);
        builder.Property(v => v.NomBoutique).IsRequired().HasMaxLength(150);
        builder.Property(v => v.Statut).HasConversion<string>().HasMaxLength(20);
        builder.Property(v => v.Langue).HasConversion<string>().HasMaxLength(5).HasDefaultValue(Langue.Fr);
        builder.HasIndex(v => v.Email).IsUnique();
    }
}
