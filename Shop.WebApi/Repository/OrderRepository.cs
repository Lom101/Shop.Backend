﻿using Microsoft.EntityFrameworkCore;
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

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        // return await _context.Orders
        //     .Include(o => o.OrderItems)
        //     .ThenInclude(oi => oi.Product) // Включаем продукты в заказах
        //     .ToListAsync();
        return await _context.Orders.ToListAsync();
    }

    public async Task<Order> GetByIdAsync(int id)
    {
        // return await _context.Orders
        //     .Include(o => o.OrderItems)
        //     .ThenInclude(oi => oi.Product) // Включаем продукты в заказах
        //     .FirstOrDefaultAsync(o => o.Id == id);
        return await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
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
}