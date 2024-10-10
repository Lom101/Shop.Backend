using Shop.WebAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Shop.WebAPI.Data;
using Shop.WebAPI.Repository.Interfaces;

namespace Shop.WebAPI.Repositories
{
    public class OrderRepository : IOrderRepository 
    {
        private readonly ShopApplicationContext _context;

        public OrderRepository(ShopApplicationContext context)
        {
            _context = context;
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Model)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Model)
                .ToListAsync();
        }

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var order = await GetByIdAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Order>> GetByUserId(string userId)
        {
            return await _context.Orders
                .Include(o => o.ShippingAddress)
                .Include(o => o.OrderItems).ThenInclude(oi => oi.Model)
                .Include(o => o.OrderItems).ThenInclude(oi => oi.Size)
                .Include(o => o.OrderItems).ThenInclude(oi => oi.Model).ThenInclude(m => m.Color)
                .Include(o => o.OrderItems).ThenInclude(oi => oi.Model).ThenInclude(m => m.Photos)
                .Include(o => o.OrderItems).ThenInclude(oi => oi.Model).ThenInclude(m => m.Product)

                // .Include(p => p.Models).ThenInclude(m => m.Color)
                // .Include(p => p.Models).ThenInclude(m => m.ModelSizes).ThenInclude(ms => ms.Size)
                // .Include(p => p.Models).ThenInclude(m => m.Photos)
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }
    }
}