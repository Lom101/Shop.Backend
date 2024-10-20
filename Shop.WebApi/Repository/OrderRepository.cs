using Microsoft.EntityFrameworkCore;
using Shop.WebAPI.Data;
using Shop.WebAPI.Entities;
using Shop.WebAPI.Repository.Interfaces;

namespace Shop.WebAPI.Repository;

public class OrderRepository : IOrderRepository
{
    private readonly ShopApplicationContext _context;

    public OrderRepository(ShopApplicationContext context)
    {
        _context = context;
    }

    public async Task AddOrderAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
    }

    public async Task AddOrderItemAsync(OrderItem orderItem)
    {
        _context.OrderItems.Add(orderItem);
        await _context.SaveChangesAsync();
    }
    
    public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId)
    {
        return await _context.Orders
            //.Include(o => o.User) // потом убрать
            .Include(o => o.OrderItems).ThenInclude(oi => oi.Model).ThenInclude(m => m.Photos)
            .Include(o => o.OrderItems).ThenInclude(oi => oi.Model).ThenInclude(m => m.Color)
            .Include(o => o.OrderItems).ThenInclude(oi => oi.Model).ThenInclude(m => m.Product)
            .Include(o => o.OrderItems).ThenInclude(oi => oi.Size)

            .Where(o => o.UserId == userId)
            .ToListAsync();
    }
}