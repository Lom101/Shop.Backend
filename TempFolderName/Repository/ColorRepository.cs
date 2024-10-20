using Microsoft.EntityFrameworkCore;
using Shop.WebAPI.Data;
using Shop.WebAPI.Entities;
using Shop.WebAPI.Repository.Interfaces;

namespace Shop.WebAPI.Repository;

public class ColorRepository : IColorRepository
{
    private readonly ShopApplicationContext _context;

    public ColorRepository(ShopApplicationContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Color>> GetAllAsync()
    {
        return await _context.Colors.ToListAsync();
    }

    public async Task<Color?> GetByIdAsync(int id)
    {
        return await _context.Colors.FindAsync(id);
    }

    public async Task<bool> AddAsync(Color color)
    {
        await _context.Colors.AddAsync(color);
        return await SaveChangesAsync();
    }

    public async Task<bool> UpdateAsync(Color color)
    {
        _context.Colors.Update(color);
        return await SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var color = await GetByIdAsync(id);
        if (color == null) return false;

        _context.Colors.Remove(color);
        return await SaveChangesAsync();
    }

    private async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}