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

    public async Task<Color> GetByIdAsync(int id)
    {
        return await _context.Colors.FindAsync(id);
    }
}
