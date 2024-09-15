using Microsoft.EntityFrameworkCore;
using Shop.WebAPI.Data;
using Shop.WebAPI.Entities;
using Shop.WebAPI.Repository.Interfaces;

namespace Shop.WebAPI.Repository;

public class AddressRepository : IAddressRepository
{
    private readonly ShopApplicationContext _context;

    public AddressRepository(ShopApplicationContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Address>> GetAllAsync()
    {
        // return await _context.Addresses.Include(c => c.User).ToListAsync();
        return await _context.Addresses.ToListAsync();
    }

    public async Task<Address> GetByIdAsync(int id)
    {
        // return await _context.Addresses.Include(c => c.User)
        //     .FirstOrDefaultAsync(c => c.Id == id);
        return await _context.Addresses.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddAsync(Address address)
    {
        await _context.Addresses.AddAsync(address);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Address address)
    {
        _context.Addresses.Update(address);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var address = await GetByIdAsync(id);
        if (address != null)
        {
            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();
        }
    }
}