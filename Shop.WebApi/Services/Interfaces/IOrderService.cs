using Shop.WebAPI.Dtos.Order.Requests;
using Shop.WebAPI.Dtos.Order.Responses;

namespace Shop.WebAPI.Services.Interfaces;

public interface IOrderService
{
    Task<GetOrderResponse> GetOrderByIdAsync(int id);
    Task<IEnumerable<GetOrderResponse>> GetAllOrdersAsync();
    Task<int> AddOrderAsync(CreateOrderRequest orderDto);
    Task<bool> UpdateOrderAsync(UpdateOrderRequest orderDto);
    Task<bool> DeleteOrderAsync(int id);
    Task<IEnumerable<GetOrderResponse>> GetOrdersByUserId(string userId);
}