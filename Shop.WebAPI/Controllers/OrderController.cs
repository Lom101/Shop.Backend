using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shop.WebAPI.Data;
using Shop.WebAPI.Dtos;
using Shop.WebAPI.Dtos.Order.Requests;
using Shop.WebAPI.Dtos.Order.Responses;
using Shop.WebAPI.Dtos.OrderItem.Responses;
using Shop.WebAPI.Entities;
using Shop.WebAPI.Enums;
using Shop.WebAPI.Repository;
using Shop.WebAPI.Repository.Interfaces;
using Stripe;
using Stripe.Checkout;
using Address = Shop.WebAPI.Entities.Address;
using Product = Shop.WebAPI.Entities.Product;

namespace Shop.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly ShopApplicationContext _context;
    private readonly ILogger<OrderController> _logger;
    private readonly PaymentIntentService _paymentIntentService;
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public OrderController(ShopApplicationContext context, ILogger<OrderController> logger, IOrderRepository orderRepository, IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _paymentIntentService = new PaymentIntentService();
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    [HttpPost("create-payment-intent")]
    [Authorize]
    public async Task<IActionResult> CreatePaymentIntent([FromBody] CreatePaymentIntentRequest request)
    {
        var models = new List<Model>();
        foreach (var orderItem in request.OrderItems)
        {
            var model = await _context.Models
                .Where(m => m.Id == orderItem.ModelId)
                .Select(m => new 
                {
                    m.Id,
                    m.Price,
                    m.Product.Name,
                    ModelSize = m.ModelSizes.FirstOrDefault(s => s.SizeId == orderItem.SizeId)
                })
                .FirstOrDefaultAsync();

            if (model == null || model.ModelSize == null)
                return BadRequest("One or more models or sizes not found.");

            models.Add(new Model
            {
                Id = model.Id,
                Price = model.Price,
                Product = new Product() { Name = model.Name }
            });
        }

        long totalAmount = (long)(models.Sum(m => m.Price * request.OrderItems.First(o => o.ModelId == m.Id).Quantity) * 100);
        var options = new PaymentIntentCreateOptions
        {
            Amount = totalAmount,
            Currency = "usd",
            Metadata = new Dictionary<string, string>
            {
                { "user_id", request.UserId },
                { "order_items", JsonConvert.SerializeObject(request.OrderItems) }
            }
        };

        try
        {
            var paymentIntent = await _paymentIntentService.CreateAsync(options);
            return Ok(paymentIntent.ClientSecret);
        }
        catch (StripeException e)
        {
            return BadRequest(new { message = e.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating payment intent.");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("get-payment-intent/{paymentIntentId}")]
    [Authorize]
    public async Task<IActionResult> GetPaymentIntent(string paymentIntentId)
    {
        try
        {
            var paymentIntent = await _paymentIntentService.GetAsync(paymentIntentId);
            return Ok(paymentIntent.Metadata);
        }
        catch (StripeException e)
        {
            return BadRequest(new { message = e.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving payment intent.");
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpPost("create_order")]
    [Authorize]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        try
        {
            if (_context.Orders.Any(x => x.PaymentIntentId == request.PaymentIntentId))
                return BadRequest(new { message = "Order already created with this PaymentIntent" });

            var paymentIntent = await _paymentIntentService.GetAsync(request.PaymentIntentId);
            var orderItemsMetadata = paymentIntent.Metadata.TryGetValue("order_items", out var orderItemsData) ? orderItemsData : null;
            var userId = paymentIntent.Metadata.TryGetValue("user_id", out var userIdData) ? userIdData : null;

            if (orderItemsMetadata == null || userId == null)
                return BadRequest(new { message = "Failed to retrieve order details." });

            var orderItems = JsonConvert.DeserializeObject<List<OrderItem>>(orderItemsMetadata);
            var shippingAddress = paymentIntent.Shipping.Address;
            if (shippingAddress == null) throw new ArgumentException("Shipping address is missing.");

            // Проверяем, есть ли существующий адрес
            var existingAddress = await _context.Addresses
                .FirstOrDefaultAsync(a => a.UserId == userId && a.AddressName == shippingAddress.Line1);

            int addressId;
            if (existingAddress != null)
            {
                addressId = existingAddress.Id;
            }
            else
            {
                // Создаем новый адрес прямо здесь
                var newAddress = new Address
                {
                    AddressName = shippingAddress.Line1,
                    UserId = userId
                };

                _context.Addresses.Add(newAddress);
                await _context.SaveChangesAsync();
                addressId = newAddress.Id; // Получаем новый адрес
            }

            var newOrder = new Order
            {
                AddressId = addressId,
                Status = OrderStatus.Processed,
                PaymentIntentId = paymentIntent.Id,
                UserId = userId,
                ContactPhone = paymentIntent.Shipping.Phone
            };

            await _orderRepository.AddOrderAsync(newOrder); // Используем репозиторий для добавления заказа

            foreach (var item in orderItems)
            {
                var orderItem = new OrderItem
                {
                    OrderId = newOrder.Id,
                    ModelId = item.ModelId,
                    Quantity = item.Quantity,
                    SizeId = item.SizeId,
                    Amount = _context.Models.FirstOrDefault(x => x.Id == item.ModelId)?.Price ?? 0
                };
                await _orderRepository.AddOrderItemAsync(orderItem); // Используем репозиторий для добавления элементов заказа
            }

            var response = new GetOrderResponse
            {
                Id = newOrder.Id,
                AddressId = newOrder.AddressId,
                Status = newOrder.Status,
                PaymentIntentId = newOrder.PaymentIntentId,
                UserId = newOrder.UserId,
                ContactPhone = newOrder.ContactPhone,
                OrderItems = orderItems.Select(oi => new GetOrderItemResponse
                {
                    SizeId = oi.SizeId,
                    ModelId = oi.ModelId,
                    Quantity = oi.Quantity
                }).ToList()
            };

            return Ok(response);
        }
        catch (StripeException e)
        {
            return BadRequest(new { message = e.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating the order.");
            return StatusCode(500, "Internal server error");
        }
    }
    
    // GET: api/orders/me  
    [HttpGet("me")]
    [Authorize(Roles = "User,Admin")] 
    public async Task<IActionResult> GetMyAddresses()
    {
            // Получаем UserId из Claims
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Пользователь не авторизован.");
            }
            
            // Получаем заказы пользователя
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);

            var response = _mapper.Map<IEnumerable<GetOrderResponse>>(orders);
            return Ok(response);
    }
}
