using AutoMapper;
using Shop.WebAPI.Dtos.Order.Requests;
using Shop.WebAPI.Dtos.Order.Responses;
using Shop.WebAPI.Entities;
using Shop.WebAPI.Enums;
using Shop.WebAPI.Repository.Interfaces;
using Shop.WebAPI.Services.Interfaces;

namespace Shop.WebAPI.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public OrderService(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<GetOrderResponse> GetOrderByIdAsync(int id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        return _mapper.Map<GetOrderResponse>(order);
    }

    public async Task<IEnumerable<GetOrderResponse>> GetAllOrdersAsync()
    {
        var orders = await _orderRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<GetOrderResponse>>(orders);
    }

    public async Task<int> AddOrderAsync(CreateOrderRequest orderDto)
    {
        var order = _mapper.Map<Order>(orderDto);
        order.OrderDate = DateTime.UtcNow;
        order.Status = OrderStatus.Pending; // Пример установки статуса по умолчанию
        await _orderRepository.AddAsync(order);
        return order.Id;
    }

    public async Task<bool> UpdateOrderAsync(UpdateOrderRequest orderDto)
    {
        var order = _mapper.Map<Order>(orderDto);
        var existingOrder = await _orderRepository.GetByIdAsync(order.Id);
        if (existingOrder == null)
        {
            return false; // Заказ не найден
        }
        await _orderRepository.UpdateAsync(order);
        return true;
    }

    public async Task<bool> DeleteOrderAsync(int id)
    {
        var existingOrder = await _orderRepository.GetByIdAsync(id);
        if (existingOrder == null)
        {
            return false; // Заказ не найден
        }
        await _orderRepository.DeleteAsync(id);
        return true;
    }
}