using InscriptionApp.Api.Data;
using InscriptionApp.Api.Models;
using InscriptionApp.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InscriptionApp.Api.Repositories.Implementations;

public class VendeurRepository : IVendeurRepository
{
    private readonly AppDbContext _context;

    public VendeurRepository(AppDbContext context) => _context = context;

    public async Task<Vendeur?> GetByIdAsync(int id)
        => await _context.Vendeurs.FindAsync(id);

    public async Task<IReadOnlyList<Vendeur>> GetAllAsync()
        => await _context.Vendeurs.AsNoTracking().ToListAsync();

    public async Task AddAsync(Vendeur entity)
    {
        await _context.Vendeurs.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Vendeur entity)
    {
        _context.Vendeurs.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> EmailExistsAsync(string email)
        => await _context.Vendeurs.AnyAsync(v => v.Email == email);
}
