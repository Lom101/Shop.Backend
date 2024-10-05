using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shop.WebAPI.Data;
using Shop.WebAPI.Dtos;
using Shop.WebAPI.Entities;
using Stripe;
using Stripe.Checkout;
using Product = Shop.WebAPI.Entities.Product;

namespace Shop.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly ShopApplicationContext _context;

    public PaymentController(ShopApplicationContext context)
    {
        _context = context;
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
                    { "order_items", JsonConvert.SerializeObject(orderItems) }
                }
            };
    
            var service = new PaymentIntentService();
            var paymentIntent = await service.CreateAsync(options);
    
            return Ok(paymentIntent.ClientSecret);
        }
    
    
    [HttpPost("stripe_webhooks")]
    public async Task<IActionResult> StripeWebHook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

        try
        {
            var stripeEvent = EventUtility.ParseEvent(json);

            // Handle the event
            if (stripeEvent.Type == Events.PaymentIntentSucceeded)
            {
                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                // Вывод всех метаданных для отладки
                Console.WriteLine("Metadata: " + JsonConvert.SerializeObject(paymentIntent.Metadata));
                // Then define and call a method to handle the successful payment intent.
                // handlePaymentIntentSucceeded(paymentIntent);
                
            }
            else if (stripeEvent.Type == Events.PaymentMethodAttached)
            {
                var paymentMethod = stripeEvent.Data.Object as PaymentMethod;
                // Then define and call a method to handle the successful attachment of a PaymentMethod.
                // handlePaymentMethodAttached(paymentMethod);
            }
            // ... handle other event types
            else
            {
                // Unexpected event type
                Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
            }
            return Ok();
        }
        catch (StripeException e)
        {
            return BadRequest();
        }
    }

}
