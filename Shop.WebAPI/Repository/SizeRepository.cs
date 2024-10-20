using Microsoft.EntityFrameworkCore;
using Shop.WebAPI.Data;
using Shop.WebAPI.Entities;
using Shop.WebAPI.Repository.Interfaces;

namespace Shop.WebAPI.Repository;

public class SizeRepository : ISizeRepository
{
    private readonly ShopApplicationContext _context;

    public SizeRepository(ShopApplicationContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Size>> GetAllSizesAsync()
    {
        return await _context.Sizes.ToListAsync();
    }

    public async Task<Size> GetSizeByIdAsync(int id)
    {
        return await _context.Sizes.FirstOrDefaultAsync(s => s.Id == id);
    }
}