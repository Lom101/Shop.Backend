using Shop.WebAPI.Dtos.Order.Requests;
using Shop.WebAPI.Dtos.Order.Responses;
using Shop.WebAPI.Entities;
using Shop.WebAPI.Services.Interfaces;
using AutoMapper;
using Shop.WebAPI.Repository.Interfaces;

namespace Shop.WebAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetOrderResponse>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<GetOrderResponse>>(orders);
        }

        public async Task<GetOrderResponse> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            return _mapper.Map<GetOrderResponse>(order);
        }


        public async Task<int> AddOrderAsync(CreateOrderRequest request)
        {
            // Логика для создания заказа
            
            // var order = new Order
            // {
            //     UserId = orderRequest.UserId,
            //     Items = orderRequest.Items,
            //     TotalAmount = orderRequest.TotalAmount,
            //     CreatedAt = DateTime.UtcNow,
            // };
            
            var order = _mapper.Map<Order>(request);
            await _orderRepository.AddAsync(order);
            return order.Id;
        }

        
        public async Task<bool> UpdateOrderAsync(UpdateOrderRequest request)
        {
            var order = await _orderRepository.GetByIdAsync(request.Id);
            if (order == null) return false;

            _mapper.Map(request, order);
            await _orderRepository.UpdateAsync(order);
            return true;
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) return false;

            await _orderRepository.DeleteAsync(id);
            return true;
        }

        public async Task<IEnumerable<GetOrderResponse>> GetOrdersByUserId(string userId)
        {
            var orders = await _orderRepository.GetByUserId(userId);
            return _mapper.Map<IEnumerable<GetOrderResponse>>(orders);
        }
    }
}