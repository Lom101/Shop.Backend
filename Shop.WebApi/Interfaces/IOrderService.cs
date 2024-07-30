using Shop.DTO;

namespace Shop.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDTO>> GetAllOrdersAsync();
        Task<OrderDTO> GetOrderByIdAsync(int id);
        Task<OrderDTO> AddOrderAsync(OrderDTO order);
        Task UpdateOrderAsync(OrderDTO order);
        Task DeleteOrderAsync(int id);
        Task<bool> OrderExistsAsync(int id);
    }

}
