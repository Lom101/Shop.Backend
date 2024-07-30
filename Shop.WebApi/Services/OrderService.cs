using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shop.DTO;
using Shop.Entities;
using Shop.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Services
{
    public class OrderService : IOrderService
    {
        private readonly ShopApplicationContext _context;
        private readonly IMapper _mapper;

        public OrderService(ShopApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDTO>> GetAllOrdersAsync()
        {
            var orders = await _context.Orders.ToListAsync();
            return _mapper.Map<IEnumerable<OrderDTO>>(orders);
        }

        public async Task<OrderDTO> GetOrderByIdAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return null;
            }

            return _mapper.Map<OrderDTO>(order);
        }

        public async Task<OrderDTO> AddOrderAsync(OrderDTO orderDto)
        {
            var order = _mapper.Map<Order>(orderDto);
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return _mapper.Map<OrderDTO>(order);
        }

        public async Task UpdateOrderAsync(OrderDTO orderDto)
        {
            var order = await _context.Orders.FindAsync(orderDto.Id);
            if (order == null)
            {
                throw new KeyNotFoundException("Order not found");
            }

            _mapper.Map(orderDto, order);

            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                throw new KeyNotFoundException("Order not found");
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> OrderExistsAsync(int id)
        {
            return await _context.Orders.AnyAsync(e => e.Id == id);
        }
    }
}
