using InscriptionApp.Api.Data;
using InscriptionApp.Api.Models;
using InscriptionApp.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InscriptionApp.Api.Repositories.Implementations;

public class AcheteurRepository : IAcheteurRepository
{
    private readonly AppDbContext _context;

    public AcheteurRepository(AppDbContext context) => _context = context;

    public async Task<Acheteur?> GetByIdAsync(int id)
        => await _context.Acheteurs.FindAsync(id);

    public async Task<IReadOnlyList<Acheteur>> GetAllAsync()
        => await _context.Acheteurs.AsNoTracking().ToListAsync();

    public async Task AddAsync(Acheteur entity)
    {
        await _context.Acheteurs.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Acheteur entity)
    {
        _context.Acheteurs.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> EmailExistsAsync(string email)
        => await _context.Acheteurs.AnyAsync(a => a.Email == email);
}
