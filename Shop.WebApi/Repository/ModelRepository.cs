using Microsoft.EntityFrameworkCore;
using Shop.WebAPI.Data;
using Shop.WebAPI.Entities;
using Shop.WebAPI.Repository.Interfaces;

namespace Shop.WebAPI.Repository;

public class ModelRepository : IModelRepository
{
    private readonly ShopApplicationContext _context;

    public ModelRepository(ShopApplicationContext context)
    {
        _context = context;
    }

    public async Task<Model> GetByIdAsync(int id)
    {
        return await _context.Models
            .Include(m => m.Product)
            .Include(m => m.Color)
            .Include(m => m.ModelSizes).ThenInclude(ms => ms.Size)
            .Include(m => m.Photos)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IEnumerable<Model>> GetAllAsync()
    {
        return await _context.Models
            .Include(m => m.Product)
            .Include(m => m.Color)
            .Include(m => m.ModelSizes).ThenInclude(ms => ms.Size)
            .Include(m => m.Photos)
            .ToListAsync();
    }

    public async Task AddAsync(Model model)
    {
        await _context.Models.AddAsync(model);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Model model)
    {
        _context.Models.Update(model);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Model model)
    {
        _context.Models.Remove(model);
        await _context.SaveChangesAsync();
    }
}