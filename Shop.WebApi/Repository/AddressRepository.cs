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

    public async Task<IEnumerable<Address>> GetByUserIdAsync(string userId)
    {
        return await _context.Addresses.Where(a => a.UserId == userId).ToListAsync();
    }
}