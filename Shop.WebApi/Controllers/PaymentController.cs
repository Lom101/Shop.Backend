using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shop.WebAPI.Data;
using Shop.WebAPI.Dtos;
using Shop.WebAPI.Dtos.Order.Responses;
using Shop.WebAPI.Dtos.OrderItem.Responses;
using Shop.WebAPI.Entities;
using Shop.WebAPI.Enums;
using Stripe;
using Stripe.Checkout;
using Address = Shop.WebAPI.Entities.Address;
using Product = Shop.WebAPI.Entities.Product;

namespace Shop.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly ShopApplicationContext _context;
    private readonly ILogger<PaymentController> _logger; // Логгер


    public PaymentController(ShopApplicationContext context, ILogger<PaymentController> logger)
    {
        _context = context;
        _logger = logger;
        StripeConfiguration.ApiKey = "sk_test_51Q53yhKGJp4CXm6iutUKXwzbHDRF4GHNMrfRugSHUauQg2UUbblxgfUtBLDOgpYxwQ1Ijy2SPbbh1AZceJYW2r7g00AIilYc4K";
    }

    [HttpPost("create-payment-intent")]
    public async Task<IActionResult> CreatePaymentIntent([FromBody] PaymentRequest request)
        {
            var orderItems = request.OrderItems;
            var models = new List<Model>();
    
            // Получаем модели и рассчитываем общую сумму
            foreach (var orderItem in orderItems)
            {
                var model = _context.Models
                    .Where(m => m.Id == orderItem.ModelId)
                    .Select(m => new 
                    {
                        m.Id,
                        m.Price,
                        m.Product.Name,
                        ModelSize = m.ModelSizes.FirstOrDefault(s => s.SizeId == orderItem.SizeId)
                    })
                    .FirstOrDefault();
    
                if (model == null || model.ModelSize == null)
                {
                    return BadRequest($"Model with ID {orderItem.ModelId} or Size with ID {orderItem.SizeId} not found");
                }
    
                models.Add(new Model
                {
                    Id = model.Id,
                    Price = model.Price,    
                    Product = new Product() {Name = model.Name}
                });
            }
    
            // Рассчитываем общую сумму заказа (в центах)
            long totalAmount = (long)(models.Sum(m => m.Price * orderItems.First(o => o.ModelId == m.Id).Quantity) * 100);
    
            // Формируем Payment Intent с метаданными
            var options = new PaymentIntentCreateOptions
            {
                Amount = totalAmount, // Сумма заказа в центах
                Currency = "usd",
                Metadata = new Dictionary<string, string>
                {
                    { "user_id", request.UserId },
                    { "order_items", JsonConvert.SerializeObject(orderItems) }
                }
            };
    
            var service = new PaymentIntentService();
            var paymentIntent = await service.CreateAsync(options);
    
            return Ok(paymentIntent.ClientSecret);
        }
    
    [HttpGet("get-payment-intent/{paymentIntentId}")]
    public async Task<IActionResult> GetPaymentIntent(string paymentIntentId)
    {
        try
        {
            var service = new PaymentIntentService();
            var paymentIntent = await service.GetAsync(paymentIntentId);
    
            // Возвращаем метаданные клиенту
            return Ok(paymentIntent.Metadata);
        }
        catch (StripeException e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
    
    [HttpPost("create_order")]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest2 request)
    {
        var paymentIntentId = request.PaymentIntentId;
        try
        {
            // Check if the order already exists with the given PaymentIntent
            if (_context.Orders.Any(x => x.PaymentIntentId == paymentIntentId))
            {
                return BadRequest(new { message = "Order already created with this PaymentIntent" });
            }

            var service = new PaymentIntentService();
            var paymentIntent = await service.GetAsync(paymentIntentId);

            // Extract metadata from PaymentIntent
            string orderItemsMetadata = paymentIntent.Metadata.TryGetValue("order_items", out var orderItemsData) ? orderItemsData : null;
            string userId = paymentIntent.Metadata.TryGetValue("user_id", out var userIdData) ? userIdData : null;

            if (orderItemsMetadata != null)
            {
                // Deserialize order items
                var orderItems = JsonConvert.DeserializeObject<List<OrderItem>>(orderItemsMetadata);

                // Extract shipping address and phone
                var shippingAddress = paymentIntent.Shipping.Address;
                var phone = paymentIntent.Shipping.Phone;

                if (shippingAddress == null)
                {
                    return BadRequest(new { message = "Shipping address is missing." });
                }

                // Check and create address
                var existingAddress = _context.Addresses.FirstOrDefault(a => a.UserId == userId && a.AddressName == shippingAddress.Line1);
                int addressId;

                if (existingAddress != null)
                {
                    // Use existing address
                    addressId = existingAddress.Id;
                }
                else
                {
                    // Create new address
                    var newAddress = new Address
                    {
                        AddressName = shippingAddress.Line1,
                        UserId = userId
                    };

                    _context.Addresses.Add(newAddress);
                    await _context.SaveChangesAsync();
                    addressId = newAddress.Id;
                }

                // Create new order
                var newOrder = new Order
                {
                    AddressId = addressId,
                    Status = OrderStatus.Processed,
                    PaymentIntentId = paymentIntentId,
                    UserId = userId,
                    ContactPhone = phone // Store the contact phone
                };

                _context.Orders.Add(newOrder);
                await _context.SaveChangesAsync();

                // Add order items
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

                    _context.OrderItems.Add(orderItem);
                }

                await _context.SaveChangesAsync(); // Save changes to the database

                return Ok(new GetOrderResponse
                {
                    Id = newOrder.Id,
                    AddressId = newOrder.AddressId,
                    Status = newOrder.Status,
                    PaymentIntentId = newOrder.PaymentIntentId,
                    UserId = newOrder.UserId,
                    ContactPhone = newOrder.ContactPhone,
                    OrderItems = newOrder.OrderItems.Select(oi => new GetOrderItemResponse
                    {
                        SizeId = oi.SizeId,
                        ModelId = oi.ModelId,
                        Quantity = oi.Quantity
                    }).ToList()
                });
            }

            return BadRequest(new { message = "Failed to retrieve order items." });
        }
        catch (StripeException e)
        {
            return BadRequest(new { message = e.Message });
        }
        catch (Exception ex)
        {
            // Log unhandled exceptions
            _logger.LogError(ex, "An error occurred while creating an order.");
            return StatusCode(500, "Internal server error");
        }
    }
}

// [HttpPost("stripe_webhooks")]
// public async Task<IActionResult> StripeWebHook()
// {
//     var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
//
//     try
//     {
//         var stripeEvent = EventUtility.ParseEvent(json);
//
//         // Handle the event
//         if (stripeEvent.Type == Events.PaymentIntentSucceeded)
//         {
//             var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
//             // Вывод всех метаданных для отладки
//             Console.WriteLine("Metadata: " + JsonConvert.SerializeObject(paymentIntent.Metadata));
//             // Then define and call a method to handle the successful payment intent.
//             // handlePaymentIntentSucceeded(paymentIntent);
//             
//         }
//         else if (stripeEvent.Type == Events.PaymentMethodAttached)
//         {
//             var paymentMethod = stripeEvent.Data.Object as PaymentMethod;
//             // Then define and call a method to handle the successful attachment of a PaymentMethod.
//             // handlePaymentMethodAttached(paymentMethod);
//         }
//         // ... handle other event types
//         else
//         {
//             // Unexpected event type
//             Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
//         }
//         return Ok();
//     }
//     catch (StripeException e)
//     {
//         return BadRequest();
//     }
// }