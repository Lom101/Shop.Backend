using Microsoft.EntityFrameworkCore;
using Shop.WebAPI.Entities;
using Shop.WebAPI.Repository.Interfaces;
using Shop.WebAPI.Data;

namespace Shop.WebAPI.Repository;

public class BrandRepository : IBrandRepository
{
    private readonly ShopApplicationContext _context;

    public BrandRepository(ShopApplicationContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Brand>> GetAllAsync()
    {
        return await _context.Brands.ToListAsync();
    }

    public async Task<Brand> GetByIdAsync(int id)
    {
        return await _context.Brands.FindAsync(id);
    }

    public async Task<bool> AddAsync(Brand brand)
    {
        await _context.Brands.AddAsync(brand);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateAsync(Brand brand)
    {
        _context.Brands.Update(brand);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var brand = await GetByIdAsync(id);
        if (brand == null)
        {
            return false;
        }

        _context.Brands.Remove(brand);
        return await _context.SaveChangesAsync() > 0;
    }
}