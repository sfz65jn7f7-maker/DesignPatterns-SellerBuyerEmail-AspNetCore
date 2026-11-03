using InscriptionApp.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace InscriptionApp.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Vendeur> Vendeurs => Set<Vendeur>();
    public DbSet<Acheteur> Acheteurs => Set<Acheteur>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
