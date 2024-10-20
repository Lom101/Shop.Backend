using Shop.WebAPI.Entities;

namespace Shop.WebAPI.Repository.Interfaces;

public interface IOrderRepository
{
    Task AddOrderAsync(Order order);
    Task AddOrderItemAsync(OrderItem orderItem);
    Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);
}